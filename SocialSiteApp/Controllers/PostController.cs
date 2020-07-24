//-----------------------------------------------------------------------
// <copyright file="PostController.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace SocialSiteApp.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using CommonLayer.Show;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Post controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        IPostBL postBL;
        IConfiguration configuration;
        public PostController(IPostBL postBL, IConfiguration configuration)
        {
            this.postBL = postBL;
            this.configuration = configuration;
        }
        
        /// <summary>
        /// Add post method
        /// </summary>
        /// <param name="file"></param>
        /// <param name="text"></param>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IActionResult AddPost(IFormFile file, [FromForm]string text, [FromForm]string siteUrl)
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
                         return this.NotFound(new { status = "false", message = "User Not Found" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status="false", message = "Please Select Image or Write Your Thoughts or Site Url" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        
        }

        /// <summary>
        /// Delete post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{postId}")]
        public IActionResult DeletePost(int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.postBL.DeletePost(claim, postId);
                    if (data == true)
                    {
                        return this.Ok(new { Status = "True", message = "Post Deleted Successfully" });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "User Id & Post Id Not Found" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Please Select Post id Greater Than 0" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        /// <summary>
        /// Get all posts
        /// </summary>
        /// <returns></returns>
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
                    return this.NotFound(new { status = "false", message = "Posts Not Found" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        /// <summary>
        /// Like on post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{postId}/LikeDisLike")]
        public IActionResult Like(int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.postBL.Like(claim, postId);
                    if (data != null)
                    {
                        if (data.Like == true)
                        {
                            return this.Ok(new { Status = "True", message = data.UserId + " Number User Post Liked by Id: " + data.LikeById, data });
                        }
                        else
                        {
                            return this.Ok(new { status = "True", message = data.UserId + " Number User Post Unliked by Id: " + data.LikeById, data });
                        }
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Post Id Not Found" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Enter Post Id Must Be Greater Than 0" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        /// <summary>
        /// likes on post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{postId}/Likes")]
        public IActionResult LikesForPosts(int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.postBL.LikesForPost(claim, postId);
                    var Count = data.Count();
                    if (data.Count > 0)
                    {
                        return this.Ok(new { Status = "True", message = "All Likes", Count, data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Post Id Not Found" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Post Id Must Be Greater Than zero" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        /// <summary>
        /// add comment on post
        /// </summary>
        /// <param name="commentShowModel"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{postId}/Comment")]
        public IActionResult AddComment(CommentShowModel commentShowModel, int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.postBL.AddComment(commentShowModel, claim, postId);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = data.Name + " Commented On This Post Id: "+postId, data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Post id Not Found" });
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

        /// <summary>
        /// delete comment 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{postId}/Comment/{commentId}")]
        public IActionResult DeleteComment(int postId, int commentId)
        {
            try
            {
                if (postId > 0 && commentId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.postBL.DeleteComment(claim, postId, commentId);
                    if (data == true)
                    {
                        return this.Ok(new { Status = "True", message = "Comment Deleted Successfully" });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Comment Id Not Found" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "PostId & CommentId Must Be Greater Than 0" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        /// <summary>
        /// get all comments
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// share post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{postId}/Share")]
        public IActionResult SharePost(int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.postBL.SharePost(claim, postId);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = "Post Share Successfully", data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Post Id Not Found" });
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

        /// <summary>
        /// get all shared post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
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
                        return this.NotFound(new { status = "false", message = "Post Id Not Found" });
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

        [HttpDelete]
        [Route("Share/{shareId}")]
        public IActionResult DeleteSharePost(int shareId)
        {
            try
            {
                if (shareId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.postBL.DeleteSharePost(claim, shareId);
                    if (data == true)
                    {
                        return this.Ok(new { Status = "True", message = "Shared Post Deleted Successfully" });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Share Id Not Found" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Please Take Share Id Greater Than 0" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }
    }
}
