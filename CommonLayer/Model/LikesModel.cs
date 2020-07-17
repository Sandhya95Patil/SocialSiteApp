using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonLayer.Model
{
    public class LikesModel
    {
        public int Id { get; set; }
        [ForeignKey("Posts")]
        public int PostId { get; set; }
        [ForeignKey("Posts")]
        public int UserId { get; set; }
        [ForeignKey("Registrations")]
        public int LikeById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
