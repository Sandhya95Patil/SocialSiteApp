using CommonLayer.Model;
using CommonLayer.Show;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class PostRL : IPostRL
    {
        private readonly AppDBContext appDBContext;
        public PostRL(AppDBContext appDBContext)
        {
            this.appDBContext = appDBContext;
        }
        public async Task<PostModel> AddPost(int userId,PostShowModel postShowModel)
        {
            try
            {
                var checkEmailId = this.appDBContext.Registrations.FirstOrDefault(g => g.Id==userId);
                if (checkEmailId != null)
                {
                    var postDetails = new PostModel()
                    {
                        UserId=userId,
                       Post=postShowModel.Post,
                       CreatedDate=DateTime.Now

                    };
                    this.appDBContext.Posts.Add(postDetails);
                    var result = await this.appDBContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        var response = new PostModel()
                        {
                            Id = postDetails.Id,
                            UserId=postDetails.UserId,
                            Post=postDetails.Post,
                            CreatedDate=postDetails.CreatedDate
                        };
                        return response;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
