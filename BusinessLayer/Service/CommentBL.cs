using BusinessLayer.Interface;
using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CommentBL : ICommentBL
    {
        ICommentRL commentRL;
        public CommentBL(ICommentRL commentRL)
        {
            this.commentRL = commentRL;
        }

        public Task<CommentResponseModel> AddComment(CommentShowModel commentShowModel, int commentById)
        {
            try
            {
                if (commentShowModel != null)
                {
                    return this.commentRL.AddComment(commentShowModel, commentById);
                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);

            }
        }

        public (List<CommentsModel>, List<RegistrationModel>) GetAllComments(int userId, int postId)
        {
            try
            {
                return this.commentRL.GetAllComments(userId, postId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
