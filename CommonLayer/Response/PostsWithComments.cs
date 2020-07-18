using CommonLayer.Model;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Response
{
    public class PostsWithComments
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Post { get; set; }
        public List<CommentsModel> Comments { get; set; }
    }
}
