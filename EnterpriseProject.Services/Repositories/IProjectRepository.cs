﻿using EnterpriseProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetProjects(int userId);
        Project? GetProject(int projectId);
        void CreateProject(Project project);
        void EditProject(int projectId);
        void DeleteProject(int projectId);
    }
}
