using EnterpriseProject.Entities;
using EnterpriseProject.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseProject.Operations.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IResumeServices _resumeRepository;
        private readonly IProjectServices _projectRepository;
        private readonly IUserServices _userRepository;
        private readonly ICommentServices _commentRepository;

        //Path of all project images
        private readonly string _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "project_images");

        public ProjectController(IResumeServices resumeRepository, IProjectServices projectRepository, IUserServices userRepository, ICommentServices commentRepository)
        {
            _resumeRepository = resumeRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        public IActionResult ViewProject(int id) {
            var project = _projectRepository.GetProject(id);

            if (project == null) { return NotFound(); }

            List<Comment>? comments = _commentRepository.GetCommentsByProjectId(project.ProjectId);

            project.Comments = comments;

            return View(project);
        }

        public IActionResult ViewProjects() {
            return View(_projectRepository.GetProjects().ToList());
        }

        [HttpGet]
        public IActionResult CreateProject() {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(Project model, IFormFile projectImage)
        {
                if (projectImage != null && projectImage.Length > 0)
                {

                    var fileName = Path.GetFileNameWithoutExtension(projectImage.FileName) + "_"
                        + Guid.NewGuid().ToString() + Path.GetExtension(projectImage.FileName);

                    var filePath = Path.Combine(_imageFolderPath, fileName);

                    Directory.CreateDirectory(_imageFolderPath);

                    using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                        await projectImage.CopyToAsync(fileStream);
                    }

                    model.ImagePath = "/uploads/project_images/" + fileName;
                }

                int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                model.UserID = userId;
                _projectRepository.CreateProject(model);
                return RedirectToAction("ViewProjects");
        }

        [HttpGet]
        public IActionResult EditProject(int id) {

            var project = _projectRepository.GetProject(id);

            if (project == null) { return NotFound(); }

            return View(project);
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

        [HttpGet]
        public IActionResult DeleteProject(int id) {
            var project = _projectRepository.GetProject(id);

            if (project == null) { return NotFound(); }

            return View(project);
        }

        [HttpPost]
        public IActionResult DeleteProjectConfirmed(int id)
        {
            var project = _projectRepository.GetProject(id);
            if (project == null) { return NotFound(); }
            
            _projectRepository.DeleteProject(id);
            return RedirectToAction("ViewProjects");
        }

        [HttpPost]
        public IActionResult AddComment(int projectId, string content)
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            _commentRepository.AddComment(userId, null, projectId, content);
            return RedirectToAction("ViewProject", new { id = projectId });
        }
    }
}
