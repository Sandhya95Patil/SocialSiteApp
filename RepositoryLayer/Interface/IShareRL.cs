using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IShareRL
    {
        Task<ShareModel> SharePost(int shareById, int postId);
    }
}
