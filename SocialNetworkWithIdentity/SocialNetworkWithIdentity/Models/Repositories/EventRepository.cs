using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Repositories
{
    public class EventRepository : IRepository<Event>
    {
        private ApplicationDbContext db;

        public EventRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public IEnumerable<Event> GetAll()
        {
            return db.Events.Include(u => u.Owners);
        }

        public Event GetById(int id)
        {
            return db.Events.Find(id);
        }

        public Event GetById(string id) { return null; }

        public IEnumerable<Event> Find(Func<Event, Boolean> predicate)
        {
            return db.Events.Where(predicate).ToList();
        }

        public void Create(Event Event)
        {
            db.Events.Add(Event);
        }

        public void Update(Event Event)
        {
            db.Entry(Event).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            Event Event = db.Events.Find(id);

            if (Event != null)
                db.Events.Remove(Event);
        }
    }
}