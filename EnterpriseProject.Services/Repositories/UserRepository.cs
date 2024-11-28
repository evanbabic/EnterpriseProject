using EnterpriseProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetUserDetails(int userId) {
            return _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public void CreateUser(User user) { }
        public void EditUser(int userId) { }
        public void DeleteUser(int userId) { }
    }
}
