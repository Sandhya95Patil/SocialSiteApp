using BusinessLayer.Interface;
using CommonLayer.Model;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Interface;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class PostBL : IPostBL
    {
        IPostRL postRL;
        public PostBL(IPostRL postRL)
        {
            this.postRL = postRL;
        }
        public async Task<PostModel> AddPost(IFormFile file, int userId)
        {
            try
            {
                if (file != null)
                {
                    var response = await this.postRL.AddPost(file, userId);
                    return response;
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

        public async Task<string> DeletePost(int userId, int postId)
        {
            try
            {
                if (userId > 0 && postId > 0)
                {
                    var response = await this.postRL.DeletePost(userId, postId);
                    return response;
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

        public IList<PostModel> GetAllPosts(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    return this.postRL.GetAllPosts(userId);
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

        public IList<PostModel> GetAllPostsWithComments(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<LikesModel> Like(LikeShowModel likeShowModel, int likeById, int postId)
        {
            try
            {
                if (likeShowModel != null)
                {
                    return this.postRL.Like(likeShowModel, likeById, postId);
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
                    return this.postRL.LikesForPost(userId, postId);
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
