//-----------------------------------------------------------------------
// <copyright file="AccountBL.cs" company="BridgeLabz">
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

    public class AccountBL : IAccountBL
    {
        IAccountRL accountRL;
        public AccountBL(IAccountRL accountRL)
        {
            this.accountRL = accountRL;
        }

        /// <summary>
        /// user signup
        /// </summary>
        /// <param name="registrationShowModel"></param>
        /// <returns></returns>
        public async Task<RegistrationResponseModel> UserSignUp(RegistrationShowModel registrationShowModel)
        {
            try
            {
                if (registrationShowModel != null)
                {
                    var response = await this.accountRL.UserSignUp(registrationShowModel);
                    if (response != null)
                    {
                        return response;
                    }
                    return null;
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
        /// user login
        /// </summary>
        /// <param name="loginShowModel"></param>
        /// <returns></returns>
        public RegistrationResponseModel UserLogin(LoginShowModel loginShowModel)
        {
            try
            {
                if (loginShowModel != null)
                {
                    var response = this.accountRL.UserLogin(loginShowModel);
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
        /// user profile
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public RegistrationResponseModel UserProfile(int userId, IFormFile file)
        {
            try
            {
                if (userId > 0 && file != null)
                {
                    var response = this.accountRL.UserProfile(userId, file);
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
        /// get all users
        /// </summary>
        /// <returns></returns>
        public IList<RegistrationResponseModel> GetAllUsers()
        {
            try
            {
                return this.accountRL.GetAllUsers();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// request send 
        /// </summary>
        /// <param name="friendId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AddFreindModel> AddFriend(int friendId, int userId)
        {
            try
            {
                if (friendId > 0 && userId > 0)
                {
                    return await this.accountRL.AddFriend(friendId, userId);
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
        /// request accept
        /// </summary>
        /// <param name="friendId"></param>
        /// <param name="userId"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public AddFreindModel RequestAccept(int friendId, int userId, int requestId)
        {
            try
            {
                if (friendId > 0 && userId > 0 && requestId > 0)
                {
                    return this.accountRL.RequestAccept(friendId, userId, requestId);
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
        /// request delete
        /// </summary>
        /// <param name="friendId"></param>
        /// <param name="userId"></param>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public AddFreindModel RequestDelete(int friendId, int userId, int requestId)
        {
            try
            {
                return this.accountRL.RequestDelete(friendId, userId, requestId);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// get all friends
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<RegistrationResponseModel> GetAllFriends(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    return this.accountRL.GetAllFriends(userId);
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
    }
}
