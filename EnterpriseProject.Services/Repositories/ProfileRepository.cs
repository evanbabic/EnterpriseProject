using EnterpriseProject.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public class ProfileRepository : IProfileServices
    {
        private readonly AppDbContext _dbContext;

        public ProfileRepository(AppDbContext dbContext) { _dbContext = dbContext; }


        public Profile getProfile(int id)
        {
            var profile = _dbContext.Profiles.Find(id);

            if (profile == null) { return null; }

            return profile;
        }

        public void editProfile()
        { //To-do }
        }
    }
}
