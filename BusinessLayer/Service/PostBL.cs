using BusinessLayer.Interface;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Interface;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessLayer.Service
{
    public class PostBL : IPostBL
    {
        IPostRL postRL;
        public PostBL(IPostRL postRL)
        {
            this.postRL = postRL;
        }

      

        public async Task<PostModel> AddPost(IFormFile file, int userId, string text, string siteUrl)
        {
            try
            {
                 var response = await this.postRL.AddPost(file, userId, text, siteUrl);
                 return response;
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

        public Task<CommentResponseModel> AddComment(CommentShowModel commentShowModel, int commentById, int postId)
        {
            try
            {
                if (postId > 0)
                {
                    return this.postRL.AddComment(commentShowModel, commentById, postId);
                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }


        public IList<CommentsModel> GetAllComments(int userId, int postId)
        {
            try
            {
                if (postId > 0)
                {
                    return this.postRL.GetAllComments(userId, postId);
                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public Task<ShareModel> SharePost(int shareById, int postId)
        {
            try
            {
                if (postId > 0)
                {
                    return this.postRL.SharePost(shareById, postId);

                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public IList<ShareModel> NumberOfShares(int userId, int postId)
        {
            try
            {
                if (postId > 0 && userId > 0)
                {
                    return this.postRL.NumberOfShares(userId, postId);

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
