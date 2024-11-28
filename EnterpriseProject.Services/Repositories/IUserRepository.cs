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
        User GetUserDetails(int userId);
        void CreateUser(User user);
        void EditUser(int userId);
        void DeleteUser(int userId);
    }
}
