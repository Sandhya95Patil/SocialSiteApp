using BusinessLayer.Interface;
using CommonLayer.Model;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Interface;
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
    }
}
