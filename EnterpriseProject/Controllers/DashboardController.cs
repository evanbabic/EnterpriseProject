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
        public async Task<IActionResult> Login(string loginIdentifier, string password)
        {
            // Retrieve user by username or email
            var user = _userRepository.GetUserByUsernameOrEmail(loginIdentifier);

            if (user != null)
            {
                // Check if the stored password is hashed
                bool isPasswordHashed = user.Password.StartsWith("$2") && user.Password.Length == 60;

                bool isPasswordValid;

                if (isPasswordHashed)
                {
                    // Verify hashed password
                    isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
                }
                else
                {
                    // Compare plaintext password
                    isPasswordValid = user.Password == password;

                    // Optionally: hash the plaintext password after successful login
                    if (isPasswordValid)
                    {
                        user.Password = BCrypt.Net.BCrypt.HashPassword(password);
                        _userRepository.UpdateUser(user); // Ensure this updates the database
                    }
                }

                if (isPasswordValid)
                {
                    // Password matches, proceed with authentication
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.UserId.ToString())
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Dashboard");
                }
            }

            // Handle invalid login attempt
            ViewBag.ErrorMessage = "Invalid username/email or password.";
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

            // Hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                UserName = userName,
                Email = email,
                Password = hashedPassword // Storing the hashed password
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

            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
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
