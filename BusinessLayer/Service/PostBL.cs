//-----------------------------------------------------------------------
// <copyright file="PostBL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace BusinessLayer.Service
{
    using BusinessLayer.Interface;
    using CommonLayer.Model;
    using CommonLayer.Response;
    using CommonLayer.Show;
    using Microsoft.AspNetCore.Http;
    using RepositoryLayer.Interface;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Post class
    /// </summary>
    public class PostBL : IPostBL
    {
        IPostRL postRL;
        public PostBL(IPostRL postRL)
        {
            this.postRL = postRL;
        }

        /// <summary>
        /// add post
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userId"></param>
        /// <param name="text"></param>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public async Task<PostModel> AddPost(IFormFile file, int userId, string text, string siteUrl)
        {
            try
            {
                if (file != null || text != null && siteUrl != null)
                {
                    var response = await this.postRL.AddPost(file, userId, text, siteUrl);
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

        /// <summary>
        /// delete post
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public bool DeletePost(int userId, int postId)
        {
            try
            {
                if (userId > 0 && postId > 0)
                {
                    var response = this.postRL.DeletePost(userId, postId);
                    return response;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// get all posts
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// like on post
        /// </summary>
        /// <param name="likeById"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<LikesModel> Like(int likeById, int postId)
        {
            try
            {
                if (likeById > 0 && postId > 0)
                {
                    return await this.postRL.Like(likeById, postId);
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

        /// <summary>
        /// likes on post
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// add comment
        /// </summary>
        /// <param name="commentShowModel"></param>
        /// <param name="commentById"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<CommentResponseModel> AddComment(CommentShowModel commentShowModel, int commentById, int postId)
        {
            try
            {
                if (postId > 0)
                {
                    return await this.postRL.AddComment(commentShowModel, commentById, postId);
                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// delete comment
        /// </summary>
        /// <param name="commentById"></param>
        /// <param name="postId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool DeleteComment(int commentById, int postId, int commentId)
        {
            try
            {
                if (commentById > 0 && postId > 0 && commentId > 0)
                {
                    return this.postRL.DeleteComment(commentById, postId, commentId);
                }
                return false;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// get all comment
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public IList<CommentsModel> GetAllComments(int userId, int postId)
        {
            try
            {
                if (postId > 0 && userId > 0)
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

        /// <summary>
        /// share post
        /// </summary>
        /// <param name="shareById"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public ShareModel SharePost(int shareById, int postId)
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

        /// <summary>
        /// get all share posts
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
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

        public bool DeleteSharePost(int userId, int sharePostId)
        {
            try
            {
                if (userId > 0 && sharePostId > 0)
                {
                    return this.postRL.DeleteSharePost(userId, sharePostId);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
