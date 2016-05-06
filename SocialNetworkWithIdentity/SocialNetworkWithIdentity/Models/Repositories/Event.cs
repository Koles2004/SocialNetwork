using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace SocialNetworkWithIdentity.Models.Infrastructure
{
    public class Event
    {
        public long Id { set; get; }
        public ApplicationUser Sender { set; get; }
        public string Text { set; get; }
        public DateTime Date { set; get; }
        public string Image { set; get; }
        public EventType EventType { set; get; }
        public ICollection<ApplicationUser> Owners { set; get; }

        public Event()
        {
            Owners = new Collection<ApplicationUser>();
        }
    }
}