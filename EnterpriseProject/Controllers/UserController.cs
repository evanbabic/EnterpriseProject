using EnterpriseProject.Entities;
using EnterpriseProject.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnterpriseProject.Operations.Controllers;

public class UserController : Controller
{
	private readonly IUserServices _userRepository;
	private readonly IProfileServices _profileRepository;
	private readonly IResumeServices _resumeRepository;

	public UserController(IUserServices userRepository, IProfileServices profileRepository, IResumeServices resumeRepository)
	{
		_userRepository = userRepository;
		_profileRepository = profileRepository;
		_resumeRepository = resumeRepository;
	}

	[HttpGet]
	public IActionResult Profile(int userId)
	{
		var profile = _profileRepository.getProfile(userId);

		if (profile == null) { return NotFound(); }

		var resume = _resumeRepository.GetResumeByUserId(userId);
		profile.User.Resume = resume;

		Claim? myIdClaim = User.FindFirst(Entities.User.ClaimType);
		int? myId = myIdClaim != null ? int.Parse(myIdClaim.Value) : null;
		ViewBag.IsMyProfile = myId.HasValue && myId == userId;

		return View(profile);
	}



    [HttpGet]
    public IActionResult EditProfile(int userId)
    {
        
        var profile = _profileRepository.getProfile(userId);

        if (profile == null)
        {
            return NotFound();
        }


        var loggedInUserId = int.Parse(User.FindFirst(EnterpriseProject.Entities.User.ClaimType)?.Value ?? "0");
        if (loggedInUserId != userId)
        {
            return Unauthorized(); // Prevent unauthorized users 
        }

        return View(profile);
    }


    [HttpPost]
    public async Task<IActionResult> EditProfile(Profile updatedProfile, IFormFile profilePicture, IFormFile bannerImage)
    {
        int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var existingProfile = _profileRepository.getProfile(userId);

        if (existingProfile == null)
        {
            return RedirectToAction("Login", "Dashboard");
        }

        string profileImageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile_images");
        string bannerImageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "banner_images");

        Directory.CreateDirectory(profileImageDirectory);
        Directory.CreateDirectory(bannerImageDirectory);

       
        if (profilePicture != null && profilePicture.Length > 0)
        {
            var profilePictureFileName = Path.GetFileNameWithoutExtension(profilePicture.FileName) + "_"
                + Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);
            var profilePicturePath = Path.Combine(profileImageDirectory, profilePictureFileName);

            
            using (var stream = new FileStream(profilePicturePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

         
            updatedProfile.ProfilePicturePath = "/uploads/profile_images/" + profilePictureFileName;
        }

        if (bannerImage != null && bannerImage.Length > 0)
        {
            var bannerImageFileName = Path.GetFileNameWithoutExtension(bannerImage.FileName) + "_"
                + Guid.NewGuid().ToString() + Path.GetExtension(bannerImage.FileName);
            var bannerImagePath = Path.Combine(bannerImageDirectory, bannerImageFileName);

            // Save the banner image
            using (var stream = new FileStream(bannerImagePath, FileMode.Create))
            {
                await bannerImage.CopyToAsync(stream);
            }

            updatedProfile.BannerImagePath = "/uploads/banner_images/" + bannerImageFileName;
        }

        
        updatedProfile.UserId = userId;

        _profileRepository.editProfile(updatedProfile);

        return RedirectToAction("Profile", "User", new { userId = userId });
    }







}