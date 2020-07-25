//-----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace SocialSiteApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using BusinessLayer.Interface;
    using CommonLayer.Response;
    using CommonLayer.Show;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAccountBL accountBL;
        IConfiguration configuration;
        public AccountController(IAccountBL accountBL, IConfiguration configuration)
        {
            this.accountBL = accountBL;
            this.configuration = configuration;
        }

        /// <summary>
        /// User signup
        /// </summary>
        /// <param name="registrationShowModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SignUp")]
        public IActionResult UserSignUp(RegistrationShowModel registrationShowModel)
        {
            try
            {
                if (registrationShowModel != null)
                {
                    var data = this.accountBL.UserSignUp(registrationShowModel);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = "Registration Successfully Done", data });
                    }
                    else
                    {
                        return this.Conflict(new { status = "false", message = "Email Id Already Present" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = false, message="Data Not Provided" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        /// <summary>
        /// user login
        /// </summary>
        /// <param name="loginShowModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult UserLogin(LoginShowModel loginShowModel)
        {
            try
            {
                if(loginShowModel != null)
                {
                    var data = this.accountBL.UserLogin(loginShowModel);
                    if (data != null)
                    {
                        var jsonToken = GenerateToken(data);
                        return this.Ok(new { Status = "True", message = "Login Successfully", data, jsonToken });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Email Id Not Present" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Please Provide Login Details" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        /// <summary>
        /// user profile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("Profile")]
        public IActionResult UserProfile(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.accountBL.UserProfile(claim, file);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = "Profile Uploaded Successfully", data, });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "User Not Present" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Please Select Image" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllUsers()
        {
            try
            {
                    var data = this.accountBL.GetAllUsers();
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = "Get All Users", data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Users Not Fonud" });
                    }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpPost]
        [Route("{addUserId}/Friend")]
        public IActionResult AddFriend(int addUserId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                var data = this.accountBL.AddFriend(addUserId, claim);
                if (data != null)
                {
                    return this.Ok(new { Status = "True", message = "Friend Request Sent Successfully",data});
                }
                else
                {
                    return this.NotFound(new { status = "false", message = "User Not Found" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpPost]
        [Route("{userId}/FriendRequest/{requestId}/Accept")]
        public IActionResult RequestAccept(int userId, int requestId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                var data = this.accountBL.RequestAccept(claim, userId, requestId);
                if (data != null)
                {
                    return this.Ok(new { Status = "True", message = "Request Accept Successfully", data });
                }
                else
                {
                    return this.NotFound(new { status = "false", message = "Request Not Found" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpPost]
        [Route("{userId}/FriendRequest/{requestId}/Reject")]
        public IActionResult RequestDelete(int userId, int requestId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                var data = this.accountBL.RequestDelete(claim, userId, requestId);
                if (data != null)
                {
                    return this.Ok(new { Status = "True", message = "Request Deleted Successfully", data });
                }
                else
                {
                    return this.NotFound(new { status = "false", message = "Request Not Found" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpGet]
        [Route("Friends")]
        public IActionResult GetAllFriends()
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                var data = this.accountBL.GetAllFriends(claim);
                var count = data.Count();
                if (data != null)
                {
                    return this.Ok(new { Status = "True", message = "All Friends", count, data });
                }
                else
                {
                    return this.NotFound(new { status = "false", message = "Friends Not Found" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        /// <summary>
        /// generate token
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        private string GenerateToken(RegistrationResponseModel responseData)
        {
            try
            {
                var symmetricSecuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:secretKey"]));

                var signingCreds = new SigningCredentials(symmetricSecuritykey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>()
                                {
                                    new Claim("Id", responseData.Id.ToString()),
                                    new Claim("Email", responseData.Email.ToString())
                                };

                var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                    configuration["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signingCreds);
                return new JwtSecurityTokenHandler().WriteToken(token);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
