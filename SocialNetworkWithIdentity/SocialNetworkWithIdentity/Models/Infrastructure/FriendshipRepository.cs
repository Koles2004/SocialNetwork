using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Infrastructure
{
    public class FriendshipRepository : IRepository<Friendship>
    {
        private ApplicationDbContext db;

        public FriendshipRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Friendship> GetAll()
        {
            return db.Friendships;
        }

        public Friendship Get(int id)
        {
            return db.Friendships.Find(id);
        }

        public void Create(Friendship friendship)
        {
            db.Friendships.Add(friendship);
        }

        public void Update(Friendship friendship)
        {
            db.Entry(friendship).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Friendship friendship = db.Friendships.Find(id);
            if (friendship != null)
                db.Friendships.Remove(friendship);
        }
    }
}