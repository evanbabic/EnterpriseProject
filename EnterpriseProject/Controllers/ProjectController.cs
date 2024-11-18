using Microsoft.AspNetCore.Mvc;

namespace EnterpriseProject.Operations.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult ViewProjects()
        {
            return View();
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
