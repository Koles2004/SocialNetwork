using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetworkWithIdentity.Models;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.DTO
{
    public class EventTypeDTO
    {
        public long Id { set; get; }
        public ApplicationUser Sender { set; get; }
        public string Text { set; get; }
        public DateTime Date { set; get; }
        public string Image { set; get; }
        public EventType EventType { set; get; }
    }
}