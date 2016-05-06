using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Infrastructure
{
    public class EventRepository : IRepository<Event>
    {
        private ApplicationDbContext db;

        public EventRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Event> GetAll()
        {
            return db.Events;
        }

        public Event Get(int id)
        {
            return db.Events.Find(id);
        }

        public void Create(Event Event)
        {
            db.Events.Add(Event);
        }

        public void Update(Event Event)
        {
            db.Entry(Event).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Event Event = db.Events.Find(id);
            if (Event != null)
                db.Events.Remove(Event);
        }
    }
}