using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonLayer.Model
{
    public class ShareModel
    {
        public int Id { get; set; }
        [ForeignKey("Registrations")]
        public int PostById { get; set; }
        [ForeignKey("Posts")]
        public int PostId { get; set; }
        public string Post { get; set; }
        [ForeignKey("Registrations")]
        public int ShareById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
