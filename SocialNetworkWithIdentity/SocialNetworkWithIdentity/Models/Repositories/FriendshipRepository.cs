using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Repositories
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
            return db.Friendships.Include(f => f.FirstUser).Include(f => f.SecondUser).ToList();
        }

        public Friendship GetById(int id)
        {
            return db.Friendships.Find(id);
        }

        public Friendship GetById(string id) { return null; }

        public IEnumerable<Friendship> Find(Func<Friendship, Boolean> predicate)
        {
            return db.Friendships.Where(predicate).ToList();
        }

        public void Create(Friendship friendship)
        {
            db.Friendships.Add(friendship);
        }

        public void Update(Friendship friendship)
        {
            db.Entry(friendship).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            Friendship friendship = db.Friendships.Find(id);
            if (friendship != null)
                db.Friendships.Remove(friendship);
        }
    }
}