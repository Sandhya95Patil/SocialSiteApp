//-----------------------------------------------------------------------
// <copyright file="IAccountBL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace BusinessLayer.Interface
{
    using CommonLayer.Model;
    using CommonLayer.Response;
    using CommonLayer.Show;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAccountBL
    {
        Task<RegistrationResponseModel> UserSignUp(RegistrationShowModel registrationShowModel);
        RegistrationResponseModel UserLogin(LoginShowModel loginShowModel);
        RegistrationResponseModel UserProfile(int userId, IFormFile file);
        IList<RegistrationResponseModel> GetAllUsers();
        Task<AddFreindModel> AddFriend(int friendId, int userId);
        AddFreindModel RequestAccept(int friendId, int userId, int requestId);
        AddFreindModel RequestDelete(int friendId, int userId, int requestId);
        IList<RegistrationResponseModel> GetAllFriends(int userId);
    }
}
