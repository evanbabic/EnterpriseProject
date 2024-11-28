using EnterpriseProject.Entities;
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

        public IEnumerable<Project> GetProjects(int userId) {
            return _dbContext.Projects.ToList();
        }

        public Project? GetProject(int projectId) {
            return _dbContext.Projects.FirstOrDefault(p => p.ProjectId == projectId);
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
