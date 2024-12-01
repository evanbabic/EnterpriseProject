using EnterpriseProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public interface IProjectServices
    {
        IEnumerable<Project> GetProjects();
        Project? GetProject(int projectId);
        void CreateProject(Project project);
        void EditProject(Project project);
        void DeleteProject(int projectId);
    }
}
