using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Response
{
    public class AddFriendResponseModel
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public bool IsConformed { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
