using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class CommentRL : ICommentRL
    {
        AppDBContext appDBContext;

        public CommentRL(AppDBContext appDBContext)
        {
            this.appDBContext = appDBContext;
        }

        public async Task<CommentResponseModel> AddComment(CommentShowModel commentShowModel, int commentById)
        {
            try
            {
                var postExist = this.appDBContext.Posts.FirstOrDefault(g => g.Id == commentShowModel.PostId && g.UserId == commentShowModel.UserId);
                var users = this.appDBContext.Registrations.FirstOrDefault(g => g.Id == commentById);
                if (postExist != null && users != null)
                {
                    var data = new CommentsModel()
                    {
                        PostId = postExist.Id,
                        UserId = postExist.UserId,
                        CommentById = users.Id,
                        Comment = commentShowModel.Comment,
                        CreatedDate = DateTime.Now
                    };
                    this.appDBContext.Comments.Add(data);
                    var result = await this.appDBContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        var response = new CommentResponseModel()
                        {
                            Id = data.Id,
                            PostId = data.PostId,
                            UserId = data.UserId,
                            CommentById = data.CommentById,
                            Comment = data.Comment,
                            Name = users.FirstName,
                            CreatedDate = data.CreatedDate
                        };
                        return response;
                    }
                    return null;
                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public (List<CommentsModel>,List<RegistrationModel>) GetAllComments(int userId, int postId)
        {
            IList<CommentsModel> commentsList = new List<CommentsModel>();
            IList<RegistrationModel> userList = new List<RegistrationModel>();
            var comments = from table in this.appDBContext.Comments where table.UserId == userId && table.PostId == postId select table;
            var users = from table in this.appDBContext.Registrations select table;
                foreach (var comment in comments)
                {
                    commentsList.Add(comment);
                }
                
                foreach (var user in users)
                {
                    userList.Add(user);
                }
                return (commentsList.ToList(), userList.ToList());
        }
    }
}
