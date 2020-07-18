using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonLayer.Model
{
    public class ShareModel
    {
        public int Id { get; set; }

        [ForeignKey("Posts")]
        public int PostId { get; set; }

        [ForeignKey("Registrations")]
        public int SharedUserId { get; set; }

        public bool IsRemoved { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
