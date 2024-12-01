using EnterpriseProject.Entities;
using EnterpriseProject.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnterpriseProject.Operations.Controllers;

public class UserController(IUserRepository userRepository) : Controller
{
	private readonly IUserRepository userRepository = userRepository;

	[HttpGet]
	public IActionResult Profile(int userId)
	{
		User user = userRepository.GetUserDetails(userId);

		if (user == null) { return NotFound(); }

		Claim? myIdClaim = User.FindFirst(Entities.User.ClaimType);
		int? myId = myIdClaim != null ? int.Parse(myIdClaim.Value) : null;
		ViewBag.IsMyProfile = myId.HasValue && myId == userId;

		return View(user);
	}
}