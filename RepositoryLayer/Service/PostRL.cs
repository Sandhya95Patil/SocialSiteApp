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
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RepositoryLayer.Service
{
    public class PostRL : IPostRL
    {
        private readonly AppDBContext appDBContext;
        IConfiguration configuration;

        public PostRL(AppDBContext appDBContext, IConfiguration configuration)
        {
            this.appDBContext = appDBContext;
            this.configuration = configuration;
        }


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
                    var result = await this.appDBContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        var response = new PostModel()
                        {
                            Id = postDetails.Id,
                            UserId=postDetails.UserId,
                            Text = postDetails.Text,
                            ImageUrl=postDetails.ImageUrl,
                            SiteUrl=postDetails.SiteUrl,
                            CreatedDate=postDetails.CreatedDate
                        };
                        return response;
                    }
                    else
                    {
                        return null;
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

        public async Task<string> DeletePost(int userId, int postId)
        {
            try
            {
                var postExist = this.appDBContext.Posts.FirstOrDefault(g => g.UserId == userId && g.Id == postId);
                if (postExist != null)
                {
                    this.appDBContext.Posts.Remove(postExist);
                }
                var result = await this.appDBContext.SaveChangesAsync();
                if (result > 0)
                {
                    return "Post Delete Successfully";
                }
                else
                {
                    return null;
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

        public IList<PostModel> GetAllPostsWithComments(int userId)
        {
            try
            {
                IList<PostModel> postLists = new List<PostModel>();
               /* var postsWithComments = from posts in this.appDBContext.Posts
                                        join comments in this.appDBContext.Comments on posts.UserId equals comments.UserId;*/
                return postLists ;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<LikesModel> Like(LikeShowModel likeShowModel, int likeById, int postId)
        {
            try
            {
                var postExist = this.appDBContext.Posts.FirstOrDefault(g => g.Id == postId);
                var users = this.appDBContext.Registrations.FirstOrDefault(g => g.Id == likeById);
                var likes = this.appDBContext.Likes.FirstOrDefault(g => g.PostId == postId && g.LikeById == likeById);
                if (postExist != null && users != null && likes == null)
                {
                    var data = new LikesModel()
                    {
                        UserId = postExist.UserId,
                        PostId = postExist.Id,
                        LikeById = likeById,
                        CreatedDate = DateTime.Now
                    };
                    this.appDBContext.Likes.Add(data);
                    var result = await this.appDBContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        var response = new LikesModel()
                        {
                            Id = data.Id,
                            PostId = data.PostId,
                            UserId = data.UserId,
                            LikeById = data.LikeById,
                            CreatedDate = data.CreatedDate
                        };
                        return response;
                    }
                    return null;
                }
                return null;
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
                var likes = from table in this.appDBContext.Likes where table.UserId == userId && table.PostId == postId select table;
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
                var postExist = this.appDBContext.Posts.FirstOrDefault(g => g.Id == postId);
                var users = this.appDBContext.Registrations.FirstOrDefault(g => g.Id == commentById);
                if (postExist != null && users != null)
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
                    if (result > 0)
                    {
                        var response = new CommentResponseModel()
                        {
                            Id = data.Id,
                            PostId = data.PostId,
                            UserId = data.UserId,
                            CommentById = data.CommentById,
                            Comment = data.Comment,
                            Name = users.FirstName,
                            CreatedDate = data.CreatedDate
                        };
                        return response;
                    }
                    return null;
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

        public async Task<ShareModel> SharePost(int shareById, int postId)
        {
            try
            {
                var postExists = this.appDBContext.Posts.FirstOrDefault(g => g.Id == postId);
                var shareData = new ShareModel()
                {
                    PostId = postExists.Id,
                    SharedUserId = shareById,
                    IsRemoved = false,
                    CreatedDate = DateTime.Now
                };
                this.appDBContext.Share.Add(shareData);
                var result = await this.appDBContext.SaveChangesAsync();
                if (result > 0)
                {
                    return shareData;
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
    }
}
