using CommonLayer.Model;
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
        public async Task<PostModel> AddPost(IFormFile file, int userId)
        {
            try
            {
                var checkEmailId = this.appDBContext.Registrations.FirstOrDefault(g => g.Id==userId);
                if (checkEmailId != null)
                {
                    ImageUpload imageUpload = new ImageUpload(this.configuration, file);
                    var imageUrl = imageUpload.Upload(file);

                    var postDetails = new PostModel()
                    {
                       UserId=userId,
                       Post= imageUrl,
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
                            Post=postDetails.Post,
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
    }
}
