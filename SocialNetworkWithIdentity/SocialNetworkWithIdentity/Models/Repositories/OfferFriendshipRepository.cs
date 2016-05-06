using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Repositories
{
    public class OfferFriendshipRepository : IRepository<OfferFriendship>
    {
        private ApplicationDbContext db;

        public OfferFriendshipRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<OfferFriendship> GetAll()
        {
            return db.OfferFriendships;
        }

        public OfferFriendship GetById(int id)
        {
            return db.OfferFriendships.Find(id);
        }

        public OfferFriendship GetById(string id) { return null; }

        public IEnumerable<OfferFriendship> Find(Func<OfferFriendship, Boolean> predicate)
        {
            return db.OfferFriendships.Where(predicate).ToList();
        }

        public void Create(OfferFriendship offerFriendship)
        {
            db.OfferFriendships.Add(offerFriendship);
        }

        public void Update(OfferFriendship offerFriendship)
        {
            db.Entry(offerFriendship).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            OfferFriendship offerFriendship = db.OfferFriendships.Find(id);
            if (offerFriendship != null)
                db.OfferFriendships.Remove(offerFriendship);
        }
    }
}