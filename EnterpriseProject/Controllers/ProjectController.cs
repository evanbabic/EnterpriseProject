using EnterpriseProject.Entities;
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

        [HttpGet]
        public IActionResult CreateProject() {
            return View(); 
        }

        [HttpPost]
        public IActionResult CreateProject(Project model)
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            Project project = new Project
            {
                ProjectTitle = model.ProjectTitle,
                Description = model.Description,
                ImagePath = model.ImagePath,
                StartDate = model.StartDate,
                CompletedDate = model.CompletedDate,
                IsPublic = model.IsPublic,
                UserID = userId
            };

            _projectRepository.CreateProject(project);
            return RedirectToAction("ViewProjects");
        }

        [HttpGet]
        public IActionResult EditProject(int id) {
            return View(_projectRepository.GetProject(id));
        }

        [HttpPost]
        public IActionResult EditProject(int id, Project model)
        {
            if (ModelState.IsValid)
            {
                var project = _projectRepository.GetProject(id);           

                if (project == null) { return NotFound(); }
            
                project.ProjectTitle = model.ProjectTitle;
                project.Description = model.Description;
                project.ImagePath = model.ImagePath;
                project.StartDate = model.StartDate;
                project.CompletedDate = model.CompletedDate;
                project.IsPublic = model.IsPublic;

                _projectRepository.EditProject(project);
                return RedirectToAction("ViewProjects");
            }

            return View(model);
        }

        public IActionResult DeleteProject()
        {
            return View();
        }
    }
}
