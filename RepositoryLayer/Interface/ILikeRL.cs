using CommonLayer.Model;
using CommonLayer.Show;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ILikeRL
    {
        Task<LikesModel> Like(LikeShowModel likeShowModel, int likeById);
        IList<LikesModel> LikesForPost(int userId, int postId);
    }
}
