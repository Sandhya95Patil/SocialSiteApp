using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAccountBL
    {
        Task<RegistrationResponseModel> UserSignUp(RegistrationShowModel registrationShowModel);
        Task<RegistrationResponseModel> UserLogin(LoginShowModel loginShowModel);

    }
}
