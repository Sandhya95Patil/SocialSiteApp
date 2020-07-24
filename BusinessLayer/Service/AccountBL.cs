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
        public RegistrationResponseModel UserSignUp(RegistrationShowModel registrationShowModel)
        {
            try
            {
                if (registrationShowModel != null)
                {
                    var response = this.accountRL.UserSignUp(registrationShowModel);
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
    }
}
