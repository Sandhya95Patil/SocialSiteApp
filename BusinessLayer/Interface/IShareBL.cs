using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IShareBL
    {
        Task<ShareModel> SharePost(int shareById, int postId);
    }
}
