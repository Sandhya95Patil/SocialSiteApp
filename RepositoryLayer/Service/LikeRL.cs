using CommonLayer.Model;
using CommonLayer.Show;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class LikeRL : ILikeRL
    {
        private readonly AppDBContext appDBContext;
        IConfiguration configuration;

        public LikeRL(AppDBContext appDBContext, IConfiguration configuration)
        {
            this.appDBContext = appDBContext;
            this.configuration = configuration;
        }
        public async Task<LikesModel> Like(LikeShowModel likeShowModel, int likeById)
        {
            var postExist = this.appDBContext.Posts.FirstOrDefault(g => g.Id == likeShowModel.PostId);
            var users = this.appDBContext.Registrations.FirstOrDefault(g => g.Id == likeById);
            var likes = this.appDBContext.Likes.FirstOrDefault(g => g.PostId == likeShowModel.PostId && g.LikeById == likeById);
            if (postExist != null && users != null && likes == null)
            {
                var data = new LikesModel()
                {
                    UserId = postExist.UserId,
                    PostId = postExist.Id,
                    LikeById = likeById,
                    CreatedDate = DateTime.Now
                };
                this.appDBContext.Likes.Add(data);
                var result = await this.appDBContext.SaveChangesAsync();
                if (result > 0)
                {
                    var response = new LikesModel()
                    {
                        Id = data.Id,
                        PostId = data.PostId,
                        UserId = data.UserId,
                        LikeById = data.LikeById,
                        CreatedDate = data.CreatedDate
                    };
                    return response;
                }
                return null;
            }
            return null;
        }

        public IList<LikesModel> LikesForPost(int userId, int postId)
        {
            try
            {
                IList<LikesModel> likesList = new List<LikesModel>();
                var likes = from table in this.appDBContext.Likes where table.UserId == userId && table.PostId == postId select table;
                if (likes != null)
                {
                    foreach (var like in likes)
                    {
                        likesList.Add(like);
                    }
                    return likesList;
                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
