using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public interface ICommentServices
    {
        void AddComment(int userId, int? profileId, int? projectId, string content);
    }
}
