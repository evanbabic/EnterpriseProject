using EnterpriseProject.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseProject.Operations.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IResumeRepository _resumeRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public ProjectController(IResumeRepository resumeRepository, IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _resumeRepository = resumeRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public IActionResult ViewProject(int id) {
            return View(_projectRepository.GetProject(id));
        }

        public IActionResult ViewProjects()
        {
            return View(_projectRepository.GetProjects().ToList());
        }

        public IActionResult CreateProject()
        {
            return View(); 
        }

        public IActionResult EditProject()
        {
            return View();
        }

        public IActionResult DeleteProject()
        {
            return View();
        }
    }
}
