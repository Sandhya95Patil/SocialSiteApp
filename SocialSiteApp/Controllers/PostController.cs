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
        public PostController(IPostBL postBL, IConfiguration configuration)
        {
            this.postBL = postBL;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddPost(IFormFile file, [FromForm]string text, string siteUrl)
        {
            try
            {
                if (file != null || text != null || siteUrl != null)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.postBL.AddPost(file, claim, text, siteUrl);
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
                    return this.BadRequest(new { status="false", message = "" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpDelete]
        [Route("{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = await this.postBL.DeletePost(claim, postId);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = "Post Deleted Successfully" });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Given Credentials Not Found" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Please Select Image To Delete" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllPosts()
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                var data = this.postBL.GetAllPosts(claim);
                if (data != null)
                {
                    return this.Ok(new { Status = "True", message = "All Posts", data });
                }
                else
                {
                    return this.NotFound(new { status = "false", message = "Given Credentials Not Found" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpPost]
        [Route("{postId}/Like")]
        public async Task<IActionResult> Like(LikeShowModel likeShowModel, int postId)
        {
            try
            {
                if (postId > 0 && likeShowModel.UserId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = await this.postBL.Like(likeShowModel, claim, postId);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = data.UserId + " Number User Post Liked by Id: " + data.LikeById, data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "You Already Like This Post" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Please Login With Your Register EmailId" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpGet]
        [Route("{postId}/Likes")]
        public IActionResult LikesForPosts(int postId)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                var data = this.postBL.LikesForPost(claim, postId);
                var Count = data.Count();
                if (data != null)
                {
                    return this.Ok(new { Status = "True", message = "All Likes", Count, data });
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

        [HttpPost]
        [Route("{postId}/Comment")]
        public async Task<IActionResult> AddComment(CommentShowModel commentShowModel, int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = await this.postBL.AddComment(commentShowModel, claim, postId);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = data.Name + " Commented On This Post", data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Please Login With Your Register Email & Password" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Please Take PostId Greater Than 0" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpGet]
        [Route("{postId}/Comments")]
        public IActionResult GetAllComments(int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.postBL.GetAllComments(claim, postId);
                    var count = data.Count();
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = "All Comments For Post Id: " +postId, count, data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "No Any Comments On This Post Yet" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Please Take PostId Greater Than 0" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpPost]
        [Route("{postId}/Share")]
        public async Task<IActionResult> SharePost(int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = await this.postBL.SharePost(claim, postId);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = "Post Share Successfully", data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Please Login With Your Register Email & Password" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Please Take PostId Greater Than 0" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpGet]
        [Route("{postId}/Share")]
        public IActionResult NumberOfShares(int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.postBL.NumberOfShares(claim, postId);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = "Get All Shares", data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Please Login With Your Register Email & Password" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Please Take PostId Greater Than 0" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }
    }
}
