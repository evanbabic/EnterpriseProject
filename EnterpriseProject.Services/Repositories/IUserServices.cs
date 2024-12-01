using EnterpriseProject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public interface IUserServices
    {
        User GetUserByEmailAndPassword(string email, string password);
        public User GetUserByUsernameOrEmail(string loginIdentifier);

        void AddUser(User user);
        void EditUser(int userId);
        void DeleteUser(int userId);

        bool CheckUserExists(string email);
        User GetUserDetails(int userId);
        public void UpdateUser(User user);


    }
}
