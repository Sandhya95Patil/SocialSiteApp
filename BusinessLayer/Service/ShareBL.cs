using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Interface;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class ShareBL : IShareBL
    {
        IShareRL shareRL;
        public ShareBL(IShareRL shareRL)
        {
            this.shareRL = shareRL;
        }
        public Task<ShareModel> SharePost(int shareById, int postId)
        {
            try
            {
                return this.shareRL.SharePost(shareById, postId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
