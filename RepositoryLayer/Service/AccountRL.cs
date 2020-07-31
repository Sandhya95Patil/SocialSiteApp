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
    using RepositoryLayer.Migrations;
    using RepositoryLayer.PostImage;
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

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
        public async Task<RegistrationResponseModel> UserSignUp(RegistrationShowModel registrationShowModel)
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
                    var result = await this.appDBContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        var data = appDBContext.Registrations.Where(u => u.Email == registrationDetails.Email).Select(u => new RegistrationResponseModel()
                        {
                            Id = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            MobileNumber = u.MobileNumber,
                            Profile = u.Profile,
                            CreatedDate = u.CreatedDate,
                            ModifiedDate = u.ModifiedDate
                        });
                        return data.FirstOrDefault();
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

        public async Task<AddFreindModel> AddFriend(int friendId, int userId)
        {
            try
            {
                var friendIdExist = this.appDBContext.Registrations.FirstOrDefault(g => g.Id == friendId);
                var friendAlreadyExist = this.appDBContext.AddFriends.FirstOrDefault(g => g.UserId == userId && g.FriendId == friendId);
                if (friendIdExist != null && friendAlreadyExist == null)
                {
                    var addFriendData = new AddFreindModel()
                    {
                        UserId = userId,
                        FriendId = friendId,
                        IsConformed = false,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now
                    };
                    this.appDBContext.AddFriends.Add(addFriendData);
                    await this.appDBContext.SaveChangesAsync();

                    var data = appDBContext.AddFriends.Where(u => u.UserId == addFriendData.UserId && u.FriendId == addFriendData.FriendId).Select(u => new AddFreindModel()
                    {
                        Id = u.Id,
                        UserId=u.UserId,
                        FriendId=u.FriendId,
                        IsConformed=u.IsConformed,
                        IsDeleted=u.IsDeleted,
                        CreatedDate = u.CreatedDate,
                        ModifiedDate = u.ModifiedDate
                    });
                    return data.FirstOrDefault();
                    /* var addFriend = this.appDBContext.AddFriends.FirstOrDefault(g => g.UserId == addFriendData.UserId && g.FriendId == addFriendData.FriendId);
                     var response = new AddFreindModel()
                     {
                         Id=addFriend.Id,
                         UserId = addFriend.UserId,
                         FriendId = addFriend.FriendId,
                         IsConformed = addFriend.IsConformed,
                         IsDeleted = addFriend.IsDeleted,
                         CreatedDate = DateTime.Now,
                     };
                     return response;*/
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

        public AddFreindModel RequestAccept(int friendId, int userId, int requestId)
        {
            try
            {
                var requestIdExist = this.appDBContext.AddFriends.FirstOrDefault(g => g.FriendId == friendId && g.UserId == userId && g.Id == requestId);
                if (requestIdExist != null)
                {
                    requestIdExist.IsConformed = true;
                    requestIdExist.IsDeleted = false;
                    this.appDBContext.SaveChanges();

                    var response = new AddFreindModel()
                    {
                        Id = requestIdExist.Id,
                        UserId = requestIdExist.UserId,
                        FriendId = requestIdExist.FriendId,
                        IsConformed = requestIdExist.IsConformed,
                        IsDeleted = requestIdExist.IsDeleted,
                        CreatedDate = requestIdExist.CreatedDate
                    };
                    return response;
                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public AddFreindModel RequestDelete(int friendId, int userId, int requestId)
        {
            try
            {
                var requestIdExist = this.appDBContext.AddFriends.FirstOrDefault(g => g.FriendId == friendId && g.UserId == userId && g.Id == requestId);
                if (requestIdExist != null)
                {
                    requestIdExist.IsDeleted = true;
                    requestIdExist.IsConformed = false;
                    this.appDBContext.SaveChangesAsync();
                    var response = new AddFreindModel()
                    {
                        Id = requestIdExist.Id,
                        UserId = requestIdExist.UserId,
                        FriendId = requestIdExist.FriendId,
                        IsConformed = requestIdExist.IsConformed,
                        IsDeleted = requestIdExist.IsDeleted,
                        CreatedDate = requestIdExist.CreatedDate
                    };
                    return response;
                }
                return null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public IList<RegistrationResponseModel> GetAllFriends(int userId)
        {
            try
            {
                IList<RegistrationResponseModel> friendsList = new List<RegistrationResponseModel>();
                var friendsExists = from addFriend in this.appDBContext.AddFriends where addFriend.UserId == userId 
                                    join reg in this.appDBContext.Registrations on addFriend.FriendId equals reg.Id
                                    where addFriend.IsConformed == true
                                    select new RegistrationResponseModel()
                                    {
                                        Id=reg.Id,
                                        FirstName=reg.FirstName,
                                        LastName=reg.LastName,
                                        Email=reg.Email,
                                        Profile=reg.Profile,
                                        MobileNumber=reg.MobileNumber,
                                        CreatedDate=reg.CreatedDate
                                    };
                foreach (var friend in friendsExists)
                {
                    friendsList.Add(friend);
                }
                if (friendsList.Count > 0)
                {
                    return friendsList;
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
