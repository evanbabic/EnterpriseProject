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
        private readonly IResumeServices _resumeRepository;
        private readonly IProjectServices _projectRepository;
        private readonly IUserServices _userRepository;

        public DashboardController(IResumeServices resumeRepository, IProjectServices projectRepository, IUserServices userRepository) {
            _resumeRepository = resumeRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        [Route("login")]
        [HttpGet]
        public IActionResult Login() {
            return View();
        }


        [Route("login")]
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
            }

            // Handle invalid login attempt
            ViewBag.ErrorMessage = "Invalid username/email or password.";
            return View();
        }



        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("register")]
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

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Dashboard");
        }


        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to login page if not authenticated
                return RedirectToAction("Login", "Dashboard");
            }

            // User is authenticated, show dashboard
            int userId = int.Parse(User.FindFirst(Entities.User.ClaimType)?.Value ?? "0");
            var user = _userRepository.GetUserDetails(userId);

            if (user == null)
            {
                // If user details are not found, redirect to login
                return RedirectToAction("Login", "Dashboard");
            }

            // Display dashboard content (user details)
            return View(user); // Send user details to the view
        }



        [Route("dashboard/account-settings")]
        [HttpGet]
        public IActionResult AccountSettings()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            // retrieve the userId from the claim
            int userId = int.Parse(User.FindFirst(EnterpriseProject.Entities.User.ClaimType)?.Value ?? "0");

            var user = _userRepository.GetUserDetails(userId);

            if (user == null)
                return RedirectToAction("Login");

            return View(user); // sending the current user to the view
        }


        [Route("dashboard/account-settings")]
        [HttpPost]
        public IActionResult AccountSettings(User updatedUser, string oldPassword)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            int userId = int.Parse(User.FindFirst(EnterpriseProject.Entities.User.ClaimType)?.Value ?? "0");
            var existingUser = _userRepository.GetUserDetails(userId);

            if (existingUser == null)
                return RedirectToAction("Login");

            // Validate the old password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(oldPassword, existingUser.Password);

            if (!isPasswordValid)
            {
                ModelState.AddModelError("OldPassword", "The old password is incorrect.");
                return View(existingUser);
            }

            // Update fields
            existingUser.UserName = updatedUser.UserName;
            existingUser.Email = updatedUser.Email;

            // If a new password is provided, hash and update it
            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password);
            }

            // Save changes
            _userRepository.UpdateUser(existingUser);

            ViewBag.SuccessMessage = "Account settings updated successfully.";
            return View(existingUser);
        }


        [Route("dashboard/profile")]
        public IActionResult Profile()
        {
            return View();
        }

        [Route("dashboard/profile")]
        public IActionResult EditProfile()
        {
            return View();
        }
    }
}
