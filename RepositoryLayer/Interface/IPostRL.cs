using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IPostRL
    {
        Task<PostModel> AddPost(IFormFile file, int userId);

        Task<string> DeletePost(int userId, int postId);
        IList<PostModel> GetAllPosts(int userId);

        IList<PostModel> GetAllPostsWithComments(int userId);

        Task<LikesModel> Like(LikeShowModel likeShowModel, int likeById, int postId);

        IList<LikesModel> LikesForPost(int userId, int postId);

        Task<CommentResponseModel> AddComment(CommentShowModel commentShowModel, int commentById, int postId);
        IList<CommentsModel> GetAllComments(int userId, int postId);
    }
}
