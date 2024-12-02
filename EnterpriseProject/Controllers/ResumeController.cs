using EnterpriseProject.Entities;
using EnterpriseProject.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EnterpriseProject.Operations.Controllers
{
	[Authorize]
	public class ResumeController(IResumeServices resumeRepository) : Controller
	{
		private static readonly HashSet<string> resumeFileExtensions = [".pdf"];
		private static readonly string resumeFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "resumes");
		private static readonly string relativeResumeFolderPath = "/uploads/resumes";

		private readonly IResumeServices resumeRepository = resumeRepository;

		[HttpGet]
		public IActionResult ViewResume(int resumeId)
		{
			Resume? resume = resumeRepository.GetResume(resumeId);

			if (resume != null) ViewBag.ResumeFilePath = Path.Combine(relativeResumeFolderPath, resume.FilePath);

			Claim? myIdClaim = User.FindFirst(Entities.User.ClaimType);
			int? myId = myIdClaim != null ? int.Parse(myIdClaim.Value) : null;

			ViewBag.IsMyProfile = resume != null && myId.HasValue && resume.UserId == myId;

			return View(resume);
		}

		[HttpPost]
		public IActionResult DeleteResume(int userId)
		{
			Resume? userResume = resumeRepository.GetResumes().FirstOrDefault(resume => resume.UserId == userId);
			if (userResume == null)
			{
				return RedirectToAction(nameof(UserController.Profile), nameof(Entities.User), new { userId });
			}

			string userResumePath = Path.Combine(resumeFolderPath, userResume.FilePath);
			System.IO.File.Delete(userResumePath);

			resumeRepository.DeleteResume(userResume.ResumeId);

			return RedirectToAction(nameof(UserController.Profile), nameof(Entities.User), new { userId });
		}

		[HttpPost]
		public async Task<IActionResult> UploadResume(int userId, IFormFile resumeFile)
		{
			if (resumeFile == null || resumeFile.Length == 0 || !HasValidExtension(resumeFile))
			{
				return RedirectToAction(nameof(UserController.Profile), nameof(Entities.User), new { id = userId });
			}

			_ = Directory.CreateDirectory(resumeFolderPath);

			string fileName = Path.GetFileNameWithoutExtension(resumeFile.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(resumeFile.FileName);

			Resume? userResume = resumeRepository.GetResumes().FirstOrDefault(resume => resume.UserId == userId);
			if (userResume != null)
			{
				string oldFilePath = Path.Combine(resumeFolderPath, userResume.FilePath);
				System.IO.File.Delete(oldFilePath);

				userResume.FilePath = fileName;
				resumeRepository.UpdateResume(userResume);
			}
			else
			{
				userResume = new Resume
				{
					FilePath = fileName,
					UserId = userId
				};
				resumeRepository.AddResume(userResume);
			}

			using (FileStream fileStream = new(Path.Combine(resumeFolderPath, fileName), FileMode.Create))
			{
				await resumeFile.CopyToAsync(fileStream);
			}

			return RedirectToAction(nameof(ViewResume), new { resumeId = userResume.ResumeId });
		}

		private static bool HasValidExtension(IFormFile resumeFile)
		{
			string resumeFileExtension = Path.GetExtension(resumeFile.FileName);
			return resumeFileExtensions.Any(extension => extension.Equals(resumeFileExtension, StringComparison.CurrentCultureIgnoreCase));
		}
	}
}
