using CommonLayer.Model;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IPostRL
    {
        Task<PostModel> AddPost(IFormFile file, int userId);

        Task<bool> DeletePost(int userId, int postId);
    }
}
