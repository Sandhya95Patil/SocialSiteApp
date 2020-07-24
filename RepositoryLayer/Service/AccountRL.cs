//-----------------------------------------------------------------------
// <copyright file="AccountRL.cs" company="BridgeLabz">
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
    using Remotion.Linq.Clauses;
    using RepositoryLayer.Context;
    using RepositoryLayer.Encrypt;
    using RepositoryLayer.Interface;
    using RepositoryLayer.PostImage;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Account class
    /// </summary>
    public class AccountRL : IAccountRL
    {
        private readonly AppDBContext appDBContext;

        IConfiguration configuration;
        public AccountRL(AppDBContext appDBContext, IConfiguration configuration)
        {
            this.appDBContext = appDBContext;
            this.configuration = configuration;
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
                var checkEmailId = this.appDBContext.Registrations.FirstOrDefault(g => g.Email == registrationShowModel.Email);
                if (checkEmailId == null)
                {
                    var password = PasswordEncrypt.Encryptdata(registrationShowModel.Password);
                    var registrationDetails = new RegistrationModel()
                    {
                        FirstName = registrationShowModel.FirstName,
                        LastName = registrationShowModel.LastName,
                        Email = registrationShowModel.Email,
                        Password = password,
                        MobileNumber = registrationShowModel.MobileNumber,
                        CreatedDate = DateTime.Now,
                    };
                    this.appDBContext.Registrations.Add(registrationDetails);
                    var result = this.appDBContext.SaveChangesAsync();
                    if (result != null)
                    {
                        var response = new RegistrationResponseModel()
                        {
                            Id = registrationDetails.Id,
                            FirstName = registrationDetails.FirstName,
                            LastName = registrationDetails.LastName,
                            Email = registrationDetails.Email,
                            MobileNumber = registrationDetails.MobileNumber,
                            CreatedDate = registrationDetails.CreatedDate
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

        /// <summary>
        /// user login
        /// </summary>
        /// <param name="loginShowModel"></param>
        /// <returns></returns>
        public RegistrationResponseModel UserLogin(LoginShowModel loginShowModel)
        {
            try
            {
                var checkUserExist = this.appDBContext.Registrations.FirstOrDefault(g => g.Email == loginShowModel.Email);
                if (checkUserExist != null)
                {
                    var password = PasswordEncrypt.Encryptdata(loginShowModel.Password);
                    if (password.Equals(checkUserExist.Password))
                    {
                        var loginResponse = new RegistrationResponseModel()
                        {
                            Id = checkUserExist.Id,
                            FirstName = checkUserExist.FirstName,
                            LastName = checkUserExist.LastName,
                            Email = checkUserExist.Email,
                            MobileNumber = checkUserExist.MobileNumber,
                            CreatedDate = checkUserExist.CreatedDate
                        };
                        return loginResponse;
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
                string url = null;
                if (file != null)
                {
                    ImageUpload imageUpload = new ImageUpload(this.configuration, file);
                     url = imageUpload.Upload(file);
                }
                var userExists = this.appDBContext.Registrations.FirstOrDefault(g => g.Id == userId);
                if (userExists != null)
                {
                    userExists.Profile = url;
                    this.appDBContext.SaveChangesAsync();

                    var data = new RegistrationResponseModel()
                    {
                        Id=userExists.Id,
                        FirstName = userExists.FirstName,
                        LastName = userExists.LastName,
                        Email = userExists.Email,
                        MobileNumber = userExists.MobileNumber,
                        Profile = url,
                        CreatedDate = DateTime.Now
                    };
                    return data;
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
        /// Get all users 
        /// </summary>
        /// <returns></returns>
        public IList<RegistrationResponseModel> GetAllUsers()
        {
            try
            {
                IList<RegistrationResponseModel> userLists = new List<RegistrationResponseModel>();
                var users = from table in this.appDBContext.Registrations select table;
                foreach (var user in users)
                {
                    var data = new RegistrationResponseModel()
                    {
                        Id=user.Id,
                        FirstName=user.FirstName,
                        LastName=user.LastName,
                        Email=user.Email,
                        MobileNumber=user.MobileNumber,
                        Profile=user.Profile,
                        CreatedDate=user.CreatedDate,
                        ModifiedDate=user.ModifiedDate
                    };
                    userLists.Add(data);
                }
                return userLists;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
