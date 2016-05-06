using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetworkWithIdentity.Models;

namespace SocialNetworkWithIdentity.DTO
{
    public class FriendshipDTO
    {
        public long FriendshipId { set; get; }
        public ApplicationUser FirstUser { set; get; }
        public ApplicationUser SecondUser { set; get; }
        public DateTime DateOfCreate { set; get; }
        public DateTime? DateOfDelete { set; get; }
    }
}