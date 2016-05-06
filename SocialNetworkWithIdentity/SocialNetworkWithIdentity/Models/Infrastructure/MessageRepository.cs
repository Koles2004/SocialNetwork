using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Infrastructure
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

        public Message Get(int id)
        {
            return db.Messages.Find(id);
        }

        public void Create(Message messages)
        {
            db.Messages.Add(messages);
        }

        public void Update(Message messages)
        {
            db.Entry(messages).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Message messages = db.Messages.Find(id);
            if (messages != null)
                db.Messages.Remove(messages);
        }
    }
}