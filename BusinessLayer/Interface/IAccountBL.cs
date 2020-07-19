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
        RegistrationResponseModel UserSignUp(RegistrationShowModel registrationShowModel);
        RegistrationResponseModel UserLogin(LoginShowModel loginShowModel);

    }
}
