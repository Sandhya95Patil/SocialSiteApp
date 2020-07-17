using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Response
{
    public class CommentResponseModel
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public int UserId { get; set; }

        public int CommentById { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
