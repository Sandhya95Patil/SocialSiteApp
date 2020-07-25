using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CommonLayer.Model
{
    public class AddFreindModel
    {
        public int Id { get; set; } 
        [ForeignKey("Registrations")]
        public int UserId { get; set; }

        [ForeignKey("Registrations")]
        public int FriendId { get; set; }
        public bool IsConformed { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
