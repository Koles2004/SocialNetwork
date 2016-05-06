using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SocialNetworkWithIdentity.Models.DomainModels
{
    public class EventType
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public ICollection<Event> Events { set; get; }

        //public EventType()
        //{
        //    Events = new Collection<Event>();
        //}
    }
}