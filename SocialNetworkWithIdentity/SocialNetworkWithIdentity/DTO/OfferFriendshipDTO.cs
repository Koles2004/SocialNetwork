using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetworkWithIdentity.Models;

namespace SocialNetworkWithIdentity.DTO
{
    public class OfferFriendshipDTO
    {
        public long Id { set; get; }
        public ApplicationUser Offer { set; get; }
        public ApplicationUser Received { set; get; }
        public bool Status { set; get; }
        public DateTime Date { set; get; }
    }
}