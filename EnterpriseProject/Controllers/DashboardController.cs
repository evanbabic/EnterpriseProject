using EnterpriseProject.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index() {
            int userId = 2;

            var User = _userRepository.GetUserDetails(userId);
            return View(User);
        }


        public IActionResult Login() {
            return View();
        }

        public IActionResult Register()
        {
            return View();
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
