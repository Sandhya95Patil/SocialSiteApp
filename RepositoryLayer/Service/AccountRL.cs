using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Remotion.Linq.Parsing;
using RepositoryLayer.Context;
using RepositoryLayer.Encrypt;
using RepositoryLayer.Interface;
using RepositoryLayer.PostImage;
using System;
using System.Linq;

namespace RepositoryLayer.Service
{
    public class AccountRL : IAccountRL
    {
        private readonly AppDBContext appDBContext;

        IConfiguration configuration;
        public AccountRL(AppDBContext appDBContext, IConfiguration configuration)
        {
            this.appDBContext = appDBContext;
            this.configuration = configuration;
        }

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
    }
}
