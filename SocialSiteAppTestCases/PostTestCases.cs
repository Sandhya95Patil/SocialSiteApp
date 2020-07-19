using BusinessLayer.Interface;
using BusinessLayer.Service;
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
using System.Security.Claims;
using System.Text;
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

        [Fact]
        public void Given_Post_Return_OkResult()
        {
            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(new ClaimsPrincipal());

            postController.ControllerContext.HttpContext = contextMock.Object;

            IFormFile file = null;
            string text = "test case";
            string siteUrl = "https://www.c-sharpcorner.com/article/custom-model-validation-in-asp-net-core-3-1/";
            var response = postController.AddPost(file, text, siteUrl);
            Assert.IsType<OkObjectResult>(response);
        }
    }
}
