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
    }
}
