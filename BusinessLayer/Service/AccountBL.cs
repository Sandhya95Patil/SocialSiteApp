using BusinessLayer.Interface;
using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
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

        public async Task<RegistrationResponseModel> UserSignUp(RegistrationShowModel registrationShowModel)
        {
            try
            {
                if (registrationShowModel != null)
                {
                    var response = await this.accountRL.UserSignUp(registrationShowModel);
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

        public async Task<RegistrationResponseModel> UserLogin(LoginShowModel loginShowModel)
        {
            try
            {
                if (loginShowModel != null)
                {
                    var response = await this.accountRL.UserLogin(loginShowModel);
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
