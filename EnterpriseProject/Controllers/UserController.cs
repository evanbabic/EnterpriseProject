using EnterpriseProject.Entities;
using EnterpriseProject.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnterpriseProject.Operations.Controllers;

public class UserController : Controller
{
	private readonly IUserServices _userRepository;
	private readonly IProfileServices _profileRepository;

	public UserController(IUserServices userRepository, IProfileServices profileRepository)
	{
		_userRepository = userRepository;
		_profileRepository = profileRepository;
	}

	[HttpGet]
	public IActionResult Profile(int userId)
	{
		var profile = _profileRepository.getProfile(userId);

		if (profile == null) { return NotFound(); }

		Claim? myIdClaim = User.FindFirst(Entities.User.ClaimType);
		int? myId = myIdClaim != null ? int.Parse(myIdClaim.Value) : null;
		ViewBag.IsMyProfile = myId.HasValue && myId == userId;

		return View(profile);
	}
}