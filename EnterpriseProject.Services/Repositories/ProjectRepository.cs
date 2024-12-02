using EnterpriseProject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public class ProjectRepository: IProjectServices
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
                .Include(p => p.Comments)
                .FirstOrDefault(p => p.ProjectId == projectId);

            return project;
        }

        public void CreateProject(Project project) {
            _dbContext.Projects.Add(project);
            _dbContext.SaveChanges();
        }

        public void EditProject(Project project)
        {
            var existingProject = _dbContext.Projects.FirstOrDefault(p => project.ProjectId == p.ProjectId);
            
            if (existingProject != null)
            {
                existingProject.ProjectTitle = project.ProjectTitle;
                existingProject.Description = project.Description;
                existingProject.ImagePath = project.ImagePath;
                existingProject.StartDate = project.StartDate;
                existingProject.CompletedDate = project.CompletedDate;
                existingProject.IsPublic = project.IsPublic;
                _dbContext.SaveChanges();
            }
        }

        public void DeleteProject(int projectId)
        {
            var existingProject = _dbContext.Projects.Find(projectId);
            if (existingProject != null)
            {
                _dbContext.Projects.Remove(existingProject);
                _dbContext.SaveChanges();
            }
        }
    }
}
