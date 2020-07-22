using BusinessLayer.Interface;
using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AccountBL : IAccountBL
    {
        IAccountRL accountRL;
        public AccountBL(IAccountRL accountRL)
        {
            this.accountRL = accountRL;
        }

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

    }
}
