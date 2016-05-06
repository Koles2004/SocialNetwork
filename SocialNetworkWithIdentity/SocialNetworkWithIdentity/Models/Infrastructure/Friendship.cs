using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialNetworkWithIdentity.Models.Infrastructure
{
    //[Table("Friendships")]
    public class Friendship
    {
        //[Key]
        public long FriendshipId { set; get; }
        public ApplicationUser FirstUser { set; get; }
        public ApplicationUser SecondUser { set; get; }
        public DateTime DateOfCreate { set; get; }
        public DateTime? DateOfDelete { set; get; }
    }
}