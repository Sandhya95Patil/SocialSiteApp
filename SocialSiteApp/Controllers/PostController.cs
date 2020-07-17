using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer.Show;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;

namespace SocialSiteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PostController : ControllerBase
    {
        IPostBL postBL;
        AppDBContext appDBContext;
        IConfiguration configuration;
        public PostController(IPostBL postBL, AppDBContext appDBContext, IConfiguration configuration)
        {
            this.postBL = postBL;
            this.appDBContext = appDBContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddPost(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = await this.postBL.AddPost(file, claim);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = "Post Added Successfully", data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Please Login With Correct Credentials" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status="false", message = "Please Select Image" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }
    }
}
