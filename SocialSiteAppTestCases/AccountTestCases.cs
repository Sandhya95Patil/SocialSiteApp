//-----------------------------------------------------------------------
// <copyright file="AccountTestCases.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace SocialSiteAppTestCases
{
    using BusinessLayer.Interface;
    using BusinessLayer.Service;
    using CommonLayer.Model;
    using CommonLayer.Response;
    using CommonLayer.Show;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using RepositoryLayer.Context;
    using RepositoryLayer.Interface;
    using RepositoryLayer.Service;
    using SocialSiteApp.Controllers;
    using System.Collections.Generic;
    using System.Reflection;
    using Xunit;

    public class AccountTestCases
    {
        IAccountBL accountBL;
        IAccountRL accountRL;
        IConfiguration configuration;
        AccountController accountController;

        public static DbContextOptions<AppDBContext> appDBContext { get; }

        public static string sqlConnectionString = "server=LAPTOP-JQPITHJ9;Database=SocialSiteDB;Trusted_Connection=true; MultipleActiveResultSets = true";

        static AccountTestCases()
        {
            appDBContext = new DbContextOptionsBuilder<AppDBContext>().UseSqlServer(sqlConnectionString).Options;
        }

        public AccountTestCases()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            configuration = configurationBuilder.Build();

            var context = new AppDBContext(appDBContext);
            accountRL = new AccountRL(context, configuration);
            accountBL = new AccountBL(accountRL);
            accountController = new AccountController(accountBL, configuration);
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
        /// given request for user registration email id already present return conflict result
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

        /// <summary>
        /// Given login details return ok result
        /// </summary>
        [Fact]
        public void Given_Login_Details_Return_Ok_Result()
        {
            var data = new LoginShowModel()
            {
                Email = "yashmore@gmail.com",
                Password = "yash"
            };
            var response = accountController.UserLogin(data);
            Assert.IsType<OkObjectResult>(response);
        }

        /// <summary>
        /// given login details not provided return bad object result
        /// </summary>
        [Fact]
        public void Given_Login_Details_NotProvided_Return_BadRequest()
        {
            LoginShowModel data = null;
            var response = accountController.UserLogin(data);
            Assert.IsType<BadRequestObjectResult>(response);
        }

        /// <summary>
        /// Given login details empty fiels=ds return bad request
        /// </summary>
        [Fact]
        public void Given_Login_Details_Empty_Fields_Return_NotFound()
        {
            var data = new LoginShowModel()
            {
                Email = "",
                Password = ""
            };
            var response = accountController.UserLogin(data);
            Assert.IsType<NotFoundObjectResult>(response);
        }

        /// <summary>
        /// given login details email id incorect return not found
        /// </summary>
        [Fact]
        public void Given_Login_Details_EmailId_Incorrect_Return_NotFound()
        {
            var data = new LoginShowModel()
            {
                Email = "yashpatil@gmail.com",
                Password = "yash"
            };
            var response = accountController.UserLogin(data);
            Assert.IsType<NotFoundObjectResult>(response);
        }

        /// <summary>
        /// given login details password incorrect return not found
        /// </summary>
        [Fact]
        public void Given_Login_Details_Password_Incorrect_Return_NotFound()
        {
            var data = new LoginShowModel()
            {
                Email = "yashmore@gmail.com",
                Password = "yash123"
            };
            var response = accountController.UserLogin(data);
            Assert.IsType<NotFoundObjectResult>(response);
        }

        /// <summary>
        /// check valid friend id & user id return response
        /// </summary>
        [Fact]
        public void Check_ValidFriendIdAndUserId_return_Reponse()
        {
            var friendId = 17;
            var userId = 3;
            var response = accountBL.AddFriend(friendId, userId);
            Assert.IsType<AddFreindModel>(response);
        }

        /// <summary>
        /// check friend id not exist return null
        /// </summary>
        [Fact]
        public void Check_FriendId_NotExist_Return_Null()
        {
            var friendId = 100;
            var userId = 3;
            var response = accountBL.AddFriend(friendId, userId);
            Assert.Null(response);
        }

        /// <summary>
        /// check friend id zero return null
        /// </summary>
        [Fact]
        public void Check_FriendId_Zero_Return_Null()
        {
            var friendId = 0;
            var userId = 3;
            var response = accountBL.AddFriend(friendId, userId);
            Assert.Null(response);
        }

        /// <summary>
        /// check friend request accept by given friend id return response
        /// </summary>
        [Fact]
        public void Check_FriendRequest_Accept_By_Given_FriendId_Return_Response()
        {
            var friendId = 12;
            var userId = 3;
            var requestId = 4;
            var response = accountBL.RequestAccept(friendId, userId, requestId);
            Assert.IsType<AddFreindModel>(response);
        }

        /// <summary>
        /// check friend request delete by given friend id return response
        /// </summary>
        [Fact]
        public void Check_FriendRequest_Delete_By_Given_FriendId_Return_Response()
        {
            var friendId = 12;
            var userId = 3;
            var requestId = 4;
            var response = accountBL.RequestAccept(friendId, userId, requestId);
            Assert.IsType<AddFreindModel>(response);
        }

        /// <summary>
        /// check friend request invalid friend id return null
        /// </summary>
        [Fact]
        public void Check_FriendRequest_InvalidFriendId_Return_Null()
        {
            var friendId = 100;
            var userId = 3;
            var requestId = 4;
            var response = accountBL.RequestAccept(friendId, userId, requestId);
            Assert.Null(response);
        }

        /// <summary>
        /// check count of all friend of user
        /// </summary>
        [Fact]
        public void Check_Count_AllFriends_Of_User()
        {
            int userId = 3;
            var response = accountBL.GetAllFriends(userId);
            var item = Assert.IsType<List<RegistrationResponseModel>>(response);
            Assert.Equal(4, item.Count);
        }

        [Fact]
        public void Check_Count_Of_AllFriends_Of_User_But_UserId_NotFound_Return_Null()
        {
            int userId = 20;
            var response = accountBL.GetAllFriends(userId);
            Assert.Null(response);
        }
    }
}
    
