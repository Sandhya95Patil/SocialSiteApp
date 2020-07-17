using CommonLayer.Model;
using CommonLayer.Response;
using CommonLayer.Show;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IAccountRL
    {
        Task<RegistrationResponseModel> UserSignUp(RegistrationShowModel registrationShowModel);
        Task<RegistrationResponseModel> UserLogin(LoginShowModel loginShowModel);
    }
}
