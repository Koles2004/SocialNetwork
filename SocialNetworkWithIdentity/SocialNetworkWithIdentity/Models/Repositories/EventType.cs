using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace SocialNetworkWithIdentity.Models.Infrastructure
{
    public class EventType
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public ICollection<Event> Events { set; get; }

        public EventType()
        {
            Events = new Collection<Event>();
        }
    }
}