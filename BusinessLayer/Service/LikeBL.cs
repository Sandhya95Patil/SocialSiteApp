using BusinessLayer.Interface;
using CommonLayer.Model;
using CommonLayer.Show;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class LikeBL : ILikeBL
    {
        ILikeRL likeRL;
        public LikeBL(ILikeRL likeRL)
        {
            this.likeRL = likeRL;
        }

        public Task<LikesModel> Like(LikeShowModel likeShowModel, int likeById)
        {
            try
            {
                if (likeShowModel != null )
                {
                    return this.likeRL.Like(likeShowModel, likeById);
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

        public IList<LikesModel> LikesForPost(int userId, int postId)
        {
            try
            {
                if (userId > 0 && postId > 0)
                {
                    return this.likeRL.LikesForPost(userId, postId);
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
