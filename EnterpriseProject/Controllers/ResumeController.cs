using EnterpriseProject.Entities;
using EnterpriseProject.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseProject.Operations.Controllers
{
    public class ResumeController(IResumeRepository resumeRepository) : Controller
    {
        private readonly IResumeRepository resumeRepository = resumeRepository;

        [HttpGet]
        public IActionResult ViewResume(int id)
        {
            Resume? resume = resumeRepository.GetResume(id);
            
            return View(resume);
        }

        public IActionResult DeleteResume()
        {
            return View();
        }

        public IActionResult UploadResume()
        {
            return View();
        }
    }
}
