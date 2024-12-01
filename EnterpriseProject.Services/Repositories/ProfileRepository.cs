using EnterpriseProject.Entities;
using Microsoft.EntityFrameworkCore;
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


        public Profile getProfile(int userId)
        {
            return _dbContext.Profiles
                .Include(p => p.User)
                .Include(p => p.Comments)
                .FirstOrDefault(p => p.UserId == userId);
        }

        public void editProfile()
        { //To-do }
        }
    }
}
