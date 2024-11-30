using EnterpriseProject.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public interface IUserRepository
    {
        User GetUserByEmailAndPassword(string email, string password);
        void AddUser(User user);
        void EditUser(int userId);
        void DeleteUser(int userId);

        bool CheckUserExists(string email);
        User GetUserDetails(int userId);

    }
}
