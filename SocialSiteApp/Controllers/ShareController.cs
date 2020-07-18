using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SocialSiteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        IShareBL shareBL;
      
        public ShareController (IShareBL shareBL)
        {
            this.shareBL = shareBL;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SharePost([FromQuery] int postId)
        {
            try
            {
                if (postId > 0)
                {
                    var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(g => g.Type == "Id").Value);
                    var data = await this.shareBL.SharePost(claim, postId);
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
    }
}
