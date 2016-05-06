using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialNetworkWithIdentity.Models.Infrastructure
{
    //[Table("OfferFriendships")]
    public class OfferFriendship
    {
        //[Key]
        public long Id { set; get; }
        public ApplicationUser Offer { set; get; }
        public ApplicationUser Received { set; get; }
        public bool Status { set; get; }
        public DateTime Date { set; get; }
    }
}