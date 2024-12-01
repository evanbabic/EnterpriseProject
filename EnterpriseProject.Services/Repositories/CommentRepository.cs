using EnterpriseProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseProject.Services.Repositories
{
    public class CommentRepository : ICommentServices
    {
        private readonly AppDbContext _dbContext;

        public CommentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddComment(int userId, int? profileId, int? projectId, string content)
        {
            var comment = new Comment
            {
                UserId = userId,
                ProfileId = profileId,
                ProjectId = projectId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
        }
    }
}
