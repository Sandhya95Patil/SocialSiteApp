//-----------------------------------------------------------------------
// <copyright file="IAccountRL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace RepositoryLayer.Interface
{
    using CommonLayer.Model;
    using CommonLayer.Response;
    using CommonLayer.Show;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    public interface IAccountRL
    {
        RegistrationResponseModel UserSignUp(RegistrationShowModel registrationShowModel);

        RegistrationResponseModel UserLogin(LoginShowModel loginShowModel);

        RegistrationResponseModel UserProfile(int userId, IFormFile file);

        IList<RegistrationResponseModel> GetAllUsers();
    }
}
