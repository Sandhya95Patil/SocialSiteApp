using CommonLayer.Model;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IPostBL
    {
        Task<PostModel> AddPost(IFormFile file, int userId);

        Task<string> DeletePost(int userId, int postId);
        IList<PostModel> GetAllPosts(int userId);

        IList<PostModel> GetAllPostsWithComments(int userId);

        Task<LikesModel> Like(LikeShowModel likeShowModel, int likeById, int postId);

        IList<LikesModel> LikesForPost(int userId, int postId);

    }
}
