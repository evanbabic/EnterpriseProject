using EnterpriseProject.Entities;
using EnterpriseProject.Services.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnterpriseProject.Operations.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IResumeRepository _resumeRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public DashboardController(IResumeRepository resumeRepository, IProjectRepository projectRepository, IUserRepository userRepository) {
            _resumeRepository = resumeRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _userRepository.GetUserByEmailAndPassword(email, password);
            if (user != null) 
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(Entities.User.ClaimType, user.UserId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.ErrorMessage = "Invalid email or password.";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string userName, string email, string password)
        {
            if (_userRepository.CheckUserExists(email))
            {
                ViewBag.ErrorMessage = "User already exists.";
                return View();
            }

            var user = new User
            {
                UserName = userName,
                Email = email,
                Password = password // will implement hashing.
            };

            _userRepository.AddUser(user);
            return RedirectToAction("Login", "Dashboard");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Dashboard");
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            int userId = int.Parse(User.FindFirst(Entities.User.ClaimType)?.Value ?? "0");
            var user = _userRepository.GetUserDetails(userId);

            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult EditProfile()
        {
            return View();
        }
    }
}
