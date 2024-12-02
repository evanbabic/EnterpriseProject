using EnterpriseProject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public class UserRepository : IUserServices
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetUserDetails(int userId)
        {
            return _dbContext.Users
                .Include(u => u.Profile)
                .Include(u => u.Projects) 
                .Include(u => u.Resume)   
                .FirstOrDefault(u => u.UserId == userId);
        }
        public void AddUser(User user) 
        { 
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        
        }
        public void EditUser(int userId) { }
        public void DeleteUser(int userId) { }

        public User GetUserByEmailAndPassword(string email, string password)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public User GetUserByUsernameOrEmail(string loginIdentifier)
        {
            return _dbContext.Users
                .Where(u => u.UserName.ToLower() == loginIdentifier.ToLower() ||
                            u.Email.ToLower() == loginIdentifier.ToLower())
                .FirstOrDefault();
        }

        public void UpdateUser(User user)
        {
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();
        }


        public bool CheckUserExists(string email)
        {
            return _dbContext.Users.Any(u => u.Email == email);
        }

        public bool VerifyPassword(int userId, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            // hashed passwords 
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }


    }
}
