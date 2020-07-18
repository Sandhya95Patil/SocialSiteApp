using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using CommonLayer.Show;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SocialSiteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        ICommentBL commentBL;
         
        public CommentController (ICommentBL commentBL)
        {
            this.commentBL = commentBL;
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddComment(CommentShowModel commentShowModel)
        {
            try
            {
                if (commentShowModel.PostId > 0 && commentShowModel.UserId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = await this.commentBL.AddComment(commentShowModel, claim);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = data.Name+ " Commented On Your Post", data });
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Please Login With Your Register Email & Password" });
                    }
                }
                else
                {
                    return this.BadRequest(new { status = "false", message = "Please Take PostId & PostId Greater Than 0" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { status = "false", message = exception.Message });
            }
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllComments([FromQuery] int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = this.commentBL.GetAllComments(claim, postId);
                    if (data != null)
                    {
                        return this.Ok(new { Status = "True", message = "All Comments", data});
                    }
                    else
                    {
                        return this.NotFound(new { status = "false", message = "Please Login With Your Register EmailId" });
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
