using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace SocialNetworkWithIdentity.Models.Infrastructure
{
    public class Room
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public ICollection<ApplicationUser> Users { set; get; }
        public ICollection<Message> Messages { set; get; }

        public Room()
        {
            Users = new Collection<ApplicationUser>();
            Messages = new Collection<Message>();
        }
    }
}