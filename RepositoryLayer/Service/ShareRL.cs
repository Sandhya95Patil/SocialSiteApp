using CommonLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class ShareRL : IShareRL
    {
        AppDBContext appDBContext;
        public ShareRL(AppDBContext appDBContext)
        {
            this.appDBContext = appDBContext;
        }
        public async Task<ShareModel> SharePost(int shareById, int postId)
        {
            var postExists = this.appDBContext.Posts.FirstOrDefault(g => g.Id == postId);
            var shareData = new ShareModel()
            {
                PostId = postExists.Id,
                PostById = postExists.UserId,
                Post = postExists.Post,
                ShareById = shareById,
                CreatedDate = DateTime.Now
            };
            this.appDBContext.Share.Add(shareData);
            var result = await this.appDBContext.SaveChangesAsync();
            if (result > 0)
            {
                return shareData;
            }
            return null;
        }
    }
}
