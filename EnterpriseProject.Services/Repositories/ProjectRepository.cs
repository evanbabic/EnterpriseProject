using EnterpriseProject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public class ProjectRepository: IProjectRepository
    {
        private readonly AppDbContext _dbContext;

        public ProjectRepository(AppDbContext context) {
            _dbContext = context;
        }

        public IEnumerable<Project> GetProjects() {
            return _dbContext.Projects.Include(p => p.User).ToList();
        }

        public Project? GetProject(int projectId) {
            Project? project = _dbContext.Projects
                .Include(p => p.User)
                .FirstOrDefault(p => p.ProjectId == projectId);

            return project;
        }

        public void CreateProject(Project project) {
            Console.WriteLine("Placeholder");
        }

        public void EditProject(int projectId)
        {
            Console.WriteLine("Placeholder");
        }
        public void DeleteProject(int projectId)
        {
            Console.WriteLine("Placeholder");
        }
    }
}
