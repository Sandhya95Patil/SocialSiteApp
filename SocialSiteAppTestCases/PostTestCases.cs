using BusinessLayer.Interface;
using BusinessLayer.Service;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            Assert.Equal(4, items.Count);
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







        public class TestPrincipal : ClaimsPrincipal
        {
            public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims))
            {
            }
        }

        public class TestIdentity : ClaimsIdentity
        {
            public TestIdentity(params Claim[] claims) : base(claims)
            {
            }
        }
    }
}
