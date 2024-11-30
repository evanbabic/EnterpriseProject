using EnterpriseProject.Entities;
using EnterpriseProject.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseProject.Operations.Controllers
{
	public class ResumeController(IResumeRepository resumeRepository, IWebHostEnvironment webHostEnvironment) : Controller
	{
		private readonly IResumeRepository resumeRepository = resumeRepository;
		private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;

		[HttpGet]
		public IActionResult ViewResume(int id)
		{
			Resume? resume = resumeRepository.GetResume(id);

			return View(resume);
		}

		[HttpPost]
		public IActionResult DeleteResume(int userId)
		{
			Resume? userResume = resumeRepository.GetResumes().FirstOrDefault(resume => resume.UserId == userId);
			if (userResume == null)
			{
				return RedirectToAction(nameof(UserController.Profile), nameof(Entities.User), new { id = userId });
			}

			string userResumePath = Path.Combine(webHostEnvironment.WebRootPath, Resume.Directory, userId.ToString(), userResume.FilePath);
			System.IO.File.Delete(userResumePath);

			resumeRepository.DeleteResume(userResume.ResumeId);

			return RedirectToAction(nameof(UserController.Profile), nameof(Entities.User), new { id = userId });
		}

		[HttpPost]
		public async Task<IActionResult> UploadResume(int userId, IFormFile resumeFile)
		{
			if (resumeFile == null || resumeFile.Length == 0 || !Path.GetExtension(resumeFile.FileName).Equals(Resume.FileExtension, StringComparison.CurrentCultureIgnoreCase))
			{
				return RedirectToAction(nameof(UserController.Profile), nameof(Entities.User), new { id = userId });
			}

			string resumeDirectory = Path.Combine(webHostEnvironment.WebRootPath, Resume.Directory);
			if (!Directory.Exists(resumeDirectory)) Directory.CreateDirectory(resumeDirectory);
			string userResumeDirectory = Path.Combine(resumeDirectory, userId.ToString());
			if (!Directory.Exists(userResumeDirectory)) Directory.CreateDirectory(userResumeDirectory);

			using (FileStream fileStream = new(Path.Combine(userResumeDirectory, resumeFile.FileName), FileMode.Create))
			{
				await resumeFile.CopyToAsync(fileStream);
			}

			Resume? userResume = resumeRepository.GetResumes().FirstOrDefault(resume => resume.UserId == userId);
			if (userResume != null)
			{
				if (userResume.FilePath != resumeFile.FileName)
				{
					string oldFilePath = Path.Combine(userResumeDirectory, userResume.FilePath);
					System.IO.File.Delete(oldFilePath);

					userResume.FilePath = resumeFile.FileName;
					resumeRepository.UpdateResume(userResume);
				}
			}
			else
			{
				userResume = new Resume
				{
					FilePath = resumeFile.FileName,
					UserId = userId
				};
				resumeRepository.AddResume(userResume);
			}

			return RedirectToAction(nameof(ViewResume), new { id = userResume.ResumeId });
		}
	}
}
