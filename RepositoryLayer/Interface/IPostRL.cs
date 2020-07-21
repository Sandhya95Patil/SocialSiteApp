using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RepositoryLayer.Interface
{
    public interface IPostRL
    {
        PostModel AddPost(IFormFile file, int userId, string text, string siteUrl);

        bool DeletePost(int userId, int postId);
        IList<PostModel> GetAllPosts(int userId);

        LikesModel Like(int likeById, int postId);

        IList<LikesModel> LikesForPost(int userId, int postId);

        Task<CommentResponseModel> AddComment(CommentShowModel commentShowModel, int commentById, int postId);
        IList<CommentsModel> GetAllComments(int userId, int postId);
    
        Task<ShareModel> SharePost(int shareById, int postId);

        IList<ShareModel> NumberOfShares(int userId, int postId);
    }
}
