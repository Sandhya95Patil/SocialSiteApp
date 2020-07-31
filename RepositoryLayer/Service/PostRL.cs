//-----------------------------------------------------------------------
// <copyright file="PostRL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace RepositoryLayer.Service
{
    using CommonLayer.Model;
    using CommonLayer.Response;
    using CommonLayer.Show;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using RepositoryLayer.Context;
    using RepositoryLayer.Interface;
    using RepositoryLayer.PostImage;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Post Class
    /// </summary>
    public class PostRL : IPostRL
    {
        private readonly AppDBContext appDBContext;
        IConfiguration configuration;

        public PostRL(AppDBContext appDBContext, IConfiguration configuration)
        {
            this.appDBContext = appDBContext;
            this.configuration = configuration;
        }

        /// <summary>
        /// Add post 
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
                string imageUrl = null;
                var checkEmailId = this.appDBContext.Registrations.FirstOrDefault(g => g.Id==userId);
                if (checkEmailId != null)
                {
                    if (file != null)
                    {
                        ImageUpload imageUpload = new ImageUpload(this.configuration, file);
                        imageUrl = imageUpload.Upload(file);
                    }

                    var postDetails = new PostModel()
                    {
                       UserId=userId,
                       Text=text,
                       ImageUrl = imageUrl,
                       SiteUrl = siteUrl,
                       CreatedDate=DateTime.Now

                    };
                    this.appDBContext.Posts.Add(postDetails);
                    await this.appDBContext.SaveChangesAsync();

                    var postData = this.appDBContext.Posts.FirstOrDefault(g => g.UserId == userId && g.Text == text && g.ImageUrl == imageUrl && g.SiteUrl == siteUrl);
                        var response = new PostModel()
                        {
                            Id = postData.Id,
                            UserId=postData.UserId,
                            Text = postData.Text,
                            ImageUrl=postData.ImageUrl,
                            SiteUrl=postData.SiteUrl,
                            CreatedDate=postData.CreatedDate
                        };
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

        public bool DeletePost(int userId, int postId)
        {
            try
            {
                var postExist = this.appDBContext.Posts.FirstOrDefault(g => g.UserId == userId && g.Id == postId);
                if (postExist != null)
                {
                    this.appDBContext.Posts.Remove(postExist);
                    this.appDBContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        public IList<PostModel> GetAllPosts(int userId)
        {
            IList<PostModel> postList = new List<PostModel>();
            var posts = from table in this.appDBContext.Posts where table.UserId == userId select table;
                foreach (var post in posts)
                {
                    postList.Add(post);
                }
                if (postList != null)
                {
                    return postList;
                }
               else
               {
                     return null;
               }
        }

        public async Task<LikesModel> Like(int likeById, int postId)
        {
            try
            {
                var isFriends = this.appDBContext.AddFriends.FirstOrDefault(g => g.FriendId == likeById && g.IsConformed == true);
                var postExist = this.appDBContext.Posts.FirstOrDefault(g => g.Id == postId);
                var users = this.appDBContext.Registrations.FirstOrDefault(g => g.Id == likeById);
                var likes = this.appDBContext.Likes.FirstOrDefault(g => g.PostId == postId);
                if (postExist != null && users != null && isFriends != null)
                {
                    if (likes == null)
                    {
                        var data = new LikesModel()
                        {
                            UserId = postExist.UserId,
                            PostId = postExist.Id,
                            LikeById = likeById,
                            CreatedDate = DateTime.Now,
                            Like = true
                        };
                        this.appDBContext.Likes.Add(data);
                        await this.appDBContext.SaveChangesAsync();

                       // var likeDetails = this.appDBContext.Likes.FirstOrDefault(g => g.LikeById == likeById && g.PostId == postId);

                        var response = new LikesModel()
                        {
                            PostId = data.PostId,
                            UserId = data.UserId,
                            LikeById = data.LikeById,
                            Like=data.Like,
                            CreatedDate = data.CreatedDate
                        };
                        return response;
                    }
                    else if(likes.Like == true)
                    {
                        var likeDetails = this.appDBContext.Likes.FirstOrDefault(g => g.LikeById == likeById && g.PostId == postId);
                        likeDetails.Like = false;
                        await this.appDBContext.SaveChangesAsync();

                        var response = new LikesModel()
                        {
                            Id = likeDetails.Id,
                            PostId = likeDetails.PostId,
                            UserId = likeDetails.UserId,
                            LikeById = likeDetails.LikeById,
                            Like=likeDetails.Like,
                            CreatedDate = likeDetails.CreatedDate
                        };
                        return response;
                    }
                    else                     
                    {
                        var likeDetails = this.appDBContext.Likes.FirstOrDefault(g => g.LikeById == likeById && g.PostId == postId);
                        likeDetails.Like = true;
                        await this.appDBContext.SaveChangesAsync();

                        var response = new LikesModel()
                        {
                            Id = likeDetails.Id,
                            PostId = likeDetails.PostId,
                            UserId = likeDetails.UserId,
                            LikeById = likeDetails.LikeById,
                            Like = likeDetails.Like,
                            CreatedDate = likeDetails.CreatedDate
                        };
                        return response;
                    }
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
                IList<LikesModel> likesList = new List<LikesModel>();
                var likes = from table in this.appDBContext.Likes where table.UserId == userId && table.PostId == postId && table.Like == true select table;
                if (likes != null)
                {
                    foreach (var like in likes)
                    {
                        likesList.Add(like);
                    }
                    return likesList;
                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<CommentResponseModel> AddComment(CommentShowModel commentShowModel, int commentById, int postId)
        {
            try
            {
                var isFriends = this.appDBContext.AddFriends.FirstOrDefault(g => g.FriendId == commentById && g.IsConformed == true);
                var postExist = this.appDBContext.Posts.FirstOrDefault(g => g.Id == postId);
                var users = this.appDBContext.Registrations.FirstOrDefault(g => g.Id == commentById);
                if (postExist != null && users != null && isFriends != null)
                {
                    var data = new CommentsModel()
                    {
                        PostId = postExist.Id,
                        UserId = postExist.UserId,
                        CommentById = users.Id,
                        Comment = commentShowModel.Comment,
                        CreatedDate = DateTime.Now
                    };
                    this.appDBContext.Comments.Add(data);
                    var result = await this.appDBContext.SaveChangesAsync();

                    var comments = this.appDBContext.Comments.FirstOrDefault(g => g.CommentById == commentById && g.PostId == postId);
                        var response = new CommentResponseModel()
                        {
                            Id = comments.Id,
                            PostId = comments.PostId,
                            UserId = comments.UserId,
                            CommentById = comments.CommentById,
                            Comment = data.Comment,
                            Name = users.FirstName,
                            CreatedDate = comments.CreatedDate
                        };
                        return response;
                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public bool DeleteComment(int commentById, int postId, int commentId)
        {
            try
            {
                var commentsExist = this.appDBContext.Comments.FirstOrDefault(g => g.CommentById == commentById && g.PostId == postId && g.Id == commentId);
                if (commentsExist != null)
                {
                    this.appDBContext.Comments.Remove(commentsExist);
                    this.appDBContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IList<CommentsModel> GetAllComments(int userId, int postId)
        {
            try
            {
                IList<CommentsModel> commentsList = new List<CommentsModel>();
                var comments = from table in this.appDBContext.Comments where table.UserId == userId && table.PostId == postId select table;
                foreach (var comment in comments)
                {
                    commentsList.Add(comment);
                }
                if (commentsList.Count > 0)
                {
                    return commentsList;
                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public ShareModel SharePost(int shareById, int postId)
        {
            try
            {
                var postExists = this.appDBContext.Posts.FirstOrDefault(g => g.Id == postId);
                if (postExists != null)
                {
                    var shareData = new ShareModel()
                    {
                        PostId = postExists.Id,
                        SharedUserId = shareById,
                        IsRemoved = false,
                        CreatedDate = DateTime.Now
                    };
                    this.appDBContext.Share.Add(shareData);
                    var result = this.appDBContext.SaveChangesAsync();
                    return shareData;
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

        public IList<ShareModel> NumberOfShares(int userId, int postId)
        {
            try
            {
                var shareList = new List<ShareModel>();
                var sharePosts = from table in this.appDBContext.Share where table.PostId == postId select table;
                if (sharePosts != null)
                {
                    foreach (var share in sharePosts)
                    {
                        shareList.Add(share);
                    }
                    return shareList;
                }
                return null;
            }
            catch(Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public bool DeleteSharePost(int userId, int sharePostId)
        {
            try
            {
                var sharePostExists = this.appDBContext.Share.FirstOrDefault(g => g.Id == sharePostId && g.SharedUserId == userId);
                if (sharePostExists != null)
                {
                    sharePostExists.IsRemoved = true;
                    this.appDBContext.SaveChangesAsync();
                    return true;
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
