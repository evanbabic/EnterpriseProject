using EnterpriseProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public interface IProfileServices
    {
        Profile getProfile(int id);
        public void editProfile(Profile updatedProfile);
    }
}
