using BusinessLayer.Interface;
using BusinessLayer.Service;
using CommonLayer.Show;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using SocialSiteApp.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SocialSiteAppTestCases
{
    public class SocialAccountTestCases
    {
        IAccountBL accountBL;
        IAccountRL accountRL;
        IConfiguration configuration;
        AccountController accountController;

        public static DbContextOptions<AppDBContext> appDBContext { get; }

        public static string sqlConnectionString = "server=LAPTOP-JQPITHJ9;Database=SocialSiteDB;Trusted_Connection=true; MultipleActiveResultSets = true";

        static SocialAccountTestCases()
        {
            appDBContext = new DbContextOptionsBuilder<AppDBContext>().UseSqlServer(sqlConnectionString).Options;
        }

        public SocialAccountTestCases()
        {
            var context = new AppDBContext(appDBContext);
            accountRL = new AccountRL(context);
            accountBL = new AccountBL(accountRL);
            accountController = new AccountController(accountBL, configuration);

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            configuration = configurationBuilder.Build();
        }

        /// <summary>
        /// given request for user registration valid data return ok object result
        /// </summary>
        [Fact]
        public void Given_Request_For_UserRegistration_ValidData_Return_OkResult()
        {
            var data = new RegistrationShowModel()
            {
                FirstName = "Nidhi",
                LastName = "Patil",
                Email = "ndhuspatil@gmail.com",
                Password = "Nidhu@12",
                MobileNumber = "7897897865"
            };
            var response = accountController.UserSignUp(data);
            Assert.IsType<OkObjectResult>(response);
        }

        /// <summary>
        /// Given request for user registration data not provided return bad request
        /// </summary>
        [Fact]
        public void Given_Request_For_UserReg_DataNotProvided_Return_BadRequest()
        {
            RegistrationShowModel data = null;
            var response = accountController.UserSignUp(data);
            Assert.IsType<BadRequestObjectResult>(response);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Given_Request_For_UserRegistration_EmailAlreadyPresent_Return_ConflictResult()
        {
            var data = new RegistrationShowModel()
            {
                FirstName = "Yash",
                LastName = "More",
                Email = "yashmore@gmail.com",
                Password = "yash",
                MobileNumber = "7897897865"
            };
            var response = accountController.UserSignUp(data);
            Assert.IsType<ConflictObjectResult>(response);
        }

        [Fact]
        public void Given_RequestFor_UserReg_If_EmptyFields_Return_BadRequest()
        {
            var data = new RegistrationShowModel()
            {
                FirstName = "",
                LastName = "",
                Email = "",
                Password = "",
                MobileNumber = ""
            };
            var response = accountController.UserSignUp(data);
            Assert.IsType<BadRequestObjectResult>(response);
        }
    }
}
    
