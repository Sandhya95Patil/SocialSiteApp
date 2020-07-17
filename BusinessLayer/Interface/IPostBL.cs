using CommonLayer.Model;
using CommonLayer.Show;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IPostBL
    {
        Task<PostModel> AddPost(int userId, PostShowModel postShowModel);
    }
}
