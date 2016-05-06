using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkWithIdentity.Models.Infrastructure
{
    public class Message
    {
        public long Id { set; get; }
        public ApplicationUser Sender { set; get; }
        public Room Room { set; get; }
        public string Text { set; get; }
        public DateTime Date { set; get; }
    }
}