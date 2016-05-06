using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Repositories
{
    public class EventTypeRepository : IRepository<EventType>
    {
        private ApplicationDbContext db;

        public EventTypeRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<EventType> GetAll()
        {
            return db.EventTypes;
        }

        public EventType GetById(int id)
        {
            return db.EventTypes.Find(id);
        }

        public EventType GetById(string id) { return null; }

        public void Create(EventType eventType)
        {
            db.EventTypes.Add(eventType);
        }

        public void Update(EventType eventType)
        {
            db.Entry(eventType).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            EventType eventType = db.EventTypes.Find(id);
            if (eventType != null)
                db.EventTypes.Remove(eventType);
        }
    }
}