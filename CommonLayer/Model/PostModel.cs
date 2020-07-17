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
        public string Post { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
