using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer.Response;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;

namespace SocialSiteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IAccountBL accountBL;
        AppDBContext appDBContext;
        IConfiguration configuration;
        public AccountController(IAccountBL accountBL, AppDBContext appDBContext, IConfiguration configuration)
        {
            this.accountBL = accountBL;
            this.appDBContext = appDBContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> UserSignUp(RegistrationShowModel registrationShowModel)
        {
            try
            {
                var data = await this.accountBL.UserSignUp(registrationShowModel);
                if (data != null)
                {
                    return this.Ok(new { Status = "True", message = "Registration Successfully Done", data });
                }
                else
                {
                    return this.Conflict(new { status = "false", message = "Email Id Already Present" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> UserLogin(LoginShowModel loginShowModel)
        {
            try
            {
                var data = await this.accountBL.UserLogin(loginShowModel);
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
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

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
