using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonLayer.Model
{
    public class PostModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Registrations")]
        public int UserId { get; set; }
        public string Text { get; set; }

        public string ImageUrl { get; set; }
        public string SiteUrl { get; set; }

        public bool IsRemoved { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
