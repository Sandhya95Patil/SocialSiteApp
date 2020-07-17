using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer.Show;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;

namespace SocialSiteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeController : ControllerBase
    {
        ILikeBL likeBL;
        AppDBContext appDBContext;
        IConfiguration configuration;
        public LikeController(ILikeBL likeBL, AppDBContext appDBContext, IConfiguration configuration)
        {
            this.likeBL = likeBL;
            this.appDBContext = appDBContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Like(LikeShowModel likeShowModel)
        {
            try
            {
                if (likeShowModel.PostId > 0 && likeShowModel.UserId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = await this.likeBL.Like(likeShowModel, claim);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = data.UserId+ " Number User Post Liked by Id: "+data.LikeById, data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Please Login First" });
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
        public IActionResult LikesForPosts([FromQuery]int postId)
        {
            try
            {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.likeBL.LikesForPost(claim, postId);
                    var NumberOfLikes = data.Count();
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = "All Likes", NumberOfLikes, data});
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Please Login First" });
                    }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }
    }
}
