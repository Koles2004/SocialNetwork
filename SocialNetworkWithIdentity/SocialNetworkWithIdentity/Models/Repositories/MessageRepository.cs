using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Repositories
{
    public class MessageRepository : IRepository<Message>
    {
        private ApplicationDbContext db;

        public MessageRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Message> GetAll()
        {
            return db.Messages;
        }

        public Message GetById(int id)
        {
            return db.Messages.Find(id);
        }

        public Message GetById(string id) { return null; }

        public IEnumerable<Message> Find(Func<Message, Boolean> predicate)
        {
            return db.Messages.Where(predicate).ToList();
        }

        public void Create(Message messages)
        {
            db.Messages.Add(messages);
        }

        public void Update(Message messages)
        {
            db.Entry(messages).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            Message messages = db.Messages.Find(id);
            if (messages != null)
                db.Messages.Remove(messages);
        }
    }
}