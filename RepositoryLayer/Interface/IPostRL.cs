//-----------------------------------------------------------------------
// <copyright file="IPostRL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace RepositoryLayer.Interface
{
    using CommonLayer.Model;
    using CommonLayer.Response;
    using CommonLayer.Show;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostRL
    {
        Task<PostModel> AddPost(IFormFile file, int userId, string text, string siteUrl);

        bool DeletePost(int userId, int postId);
        IList<PostModel> GetAllPosts(int userId);

        Task<LikesModel> Like(int likeById, int postId);

        IList<LikesModel> LikesForPost(int userId, int postId);

        Task<CommentResponseModel> AddComment(CommentShowModel commentShowModel, int commentById, int postId);

        bool DeleteComment(int commentById, int postId, int commentId);

        IList<CommentsModel> GetAllComments(int userId, int postId);
    
        ShareModel SharePost(int shareById, int postId);

        IList<ShareModel> NumberOfShares(int userId, int postId);

        bool DeleteSharePost(int userId, int sharePostId);
    }
}
