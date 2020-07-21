using BusinessLayer.Interface;
using BusinessLayer.Service;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using SocialSiteApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Xunit;

namespace SocialSiteAppTestCases
{
    public class PostTestCases
    {
        IPostBL postBL;
        IPostRL postRL;
        PostController postController;
        IConfiguration configuration;

        public static DbContextOptions<AppDBContext> appDBContext { get; }

        public static string sqlConnectString = "server=LAPTOP-JQPITHJ9;Database=SocialSiteDB;Trusted_Connection=true; MultipleActiveResultSets = true";

        static PostTestCases()
        {
            appDBContext = new DbContextOptionsBuilder<AppDBContext>().UseSqlServer(sqlConnectString).Options;
        }

        public PostTestCases()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            configuration = configurationBuilder.Build();

            var context = new AppDBContext(appDBContext);
            postRL = new PostRL(context, configuration);
            postBL = new PostBL(postRL);
            postController = new PostController(postBL, configuration);
        }

        /// <summary>
        /// Given post request all fields null return bad request
        /// </summary>
        [Fact]
        public void Given_Post_Request_All_Fields_Null_Return_BadRequest()
        {
            IFormFile file = null;
            string text = null;
            string siteUrl = null;
            var response = postBL.AddPost(file, 3, text, siteUrl);
            Assert.Null(response);
        }

        /// <summary>
        /// Given add post to social site return ok  
        /// </summary>
        [Fact]
        public void Given_Post_Return_OkResult()
        {
            IFormFile file =null;
            string text = "test case";
            string siteUrl = "https://www.c-sharpcorner.com/article/custom-model-validation-in-asp-net-core-3-1/";
            var response = postBL.AddPost(file, 3, text, siteUrl);
            Assert.IsType<PostModel>(response);
        }

        /// <summary>
        /// Check count of all posts
        /// </summary>
        [Fact]
        public void Check_Count_Of_All_Post()
        {
            int userId = 4;
            var response=postBL.GetAllPosts(userId);
            var items = Assert.IsType<List<PostModel>>(response);
            Assert.Equal(7, items.Count);
        }

        /// <summary>
        /// Check count of all post but user id less than zero return null
        /// </summary>
        [Fact]
        public void Check_Count_Of_All_Post_But_UserId_LessThan_Zero_Return_Null()
        {
            int userId = 0;
            var response = postBL.GetAllPosts(userId);
            Assert.Null(response);
        }

        /// <summary>
        /// delete post if user id & post id match return true
        /// </summary>
        [Fact]
        public void Delete_Post_Return_Ok()
        {
            int userId = 18;
            int postId = 3;
            var response = postBL.DeletePost(userId, postId);
            Assert.True(response);
        }

        /// <summary>
        /// delete post but user id not found return false
        /// </summary>
        [Fact]
        public void Delete_Post_But_UserId_NotFound_Return_False()
        {
            int userId = 100;
            int postId = 20;
            var response = postBL.DeletePost(userId, postId);
            Assert.False(response);
        }

        /// <summary>
        /// Check result of like on post 
        /// </summary>
        [Fact]
        public void Check_Result_Of_Like_OnPost()
        {
            int likeById = 17;
            int postId = 26;
            var response = postBL.Like(likeById, postId);
            Assert.IsType<LikesModel>(response);
        }

        /// <summary>
        /// check result of like on post but like by id not found
        /// </summary>
        [Fact]
        public void Check_Result_Of_Like_OnPost_But_LikeById_NotFound()
        {
            int likeById = 100;
            int postId = 26;
            var response = postBL.Like(likeById, postId);
            Assert.Null(response);
        }

        /// <summary>
        /// Check count of likes on post
        /// </summary>
        [Fact]
        public void Check_Count_Of_Likes_OnPost()
        {
            int userId = 3;
            int postId = 29;
            var response = postBL.LikesForPost(userId, postId);
            var items = Assert.IsType<List<LikesModel>>(response);
            Assert.Equal(1, items.Count());
        }

        /// <summary>
        /// check count of likes on post return not equal
        /// </summary>
        [Fact]
        public void Check_Count_Of_Likes_OnPOst_Return_NotEqual()
        {
            int userId = 3;
            int postId = 29;
            var response = postBL.LikesForPost(userId, postId);
            var items = Assert.IsType<List<LikesModel>>(response);
            Assert.NotEqual(20, items.Count());
        }

        /// <summary>
        /// Check add comment on post 
        /// </summary>
        [Fact]
        public void Check_Add_comment_On_Post_Valid_PostId_Return_Response()
        {
            var comment = new CommentShowModel()
            {
                Comment = "add comment testing time"
            };
            var commentById = 2;
            var postId = 20;
            var response = postBL.AddComment(comment, commentById, postId);
            Assert.IsType<CommentResponseModel>(response);
        }

        /// <summary>
        /// Check add comment on post but invalid post id return  null
        /// </summary>
        [Fact]
        public void Check_Add_Comment_OnPost_But_Invalid_PostId_Return_Null()
        {
            var comment = new CommentShowModel()
            {
                Comment = "add comment testing time"
            };
            var commentById = 2;
            var postId = 50;
            var response = postBL.AddComment(comment, commentById, postId);
            Assert.Null(response);
        }

        /// <summary>
        /// check delete comment on post valid comment id return true
        /// </summary>
        [Fact]
        public void Check_Delete_Comment_OnPost_Valid_CommentId_Return_True()
        {
            var commnetById = 2;
            var postId = 20;
            var commentId = 9;
            var response = postBL.DeleteComment(commnetById, postId, commentId);
            Assert.True(response);
        }

        /// <summary>
        /// chaeck delete comment on post invalid comment id return false
        /// </summary>
        [Fact]
        public void Check_Delete_Comment_OnPost_InValid_CommentId_Return_False()
        {
            var commnetById = 2;
            var postId = 20;
            var commentId = 50;
            var response = postBL.DeleteComment(commnetById, postId, commentId);
            Assert.False(response);
        }

        /// <summary>
        /// Check count of all comments return equal count
        /// </summary>
        [Fact]
        public void Check_Count_Of_AllComments_Return_Equal()
        {
            int userId = 1;
            int postId = 5;
            var response = postBL.GetAllComments(userId, postId);
            var items = Assert.IsType<List<CommentsModel>>(response);
            Assert.Equal(6, items.Count());
        }

        [Fact]
        public void Check_Count_Of_AllComments_But_UserId0_Return_Null()
        {
            int userId = 0;
            int postId = 5;
            var response = postBL.GetAllComments(userId, postId);
            Assert.Null(response);
        }

        [Fact]
        public void Check_Count_Of_AllComments_But_PostId0_Return_Null()
        {
            int userId = 1;
            int postId = 0;
            var response = postBL.GetAllComments(userId, postId);
            Assert.Null(response);
        }
    }
}
