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
    public class PostBL : IPostBL
    {
        IPostRL postRL;
        public PostBL(IPostRL postRL)
        {
            this.postRL = postRL;
        }
        public async Task<PostModel> AddPost(int userId, PostShowModel postShowModel)
        {
            try
            {
                if (postShowModel != null)
                {
                    var response = await this.postRL.AddPost(userId, postShowModel);
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
