using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Infrastructure
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

        public OfferFriendship Get(int id)
        {
            return db.OfferFriendships.Find(id);
        }

        public void Create(OfferFriendship offerFriendship)
        {
            db.OfferFriendships.Add(offerFriendship);
        }

        public void Update(OfferFriendship offerFriendship)
        {
            db.Entry(offerFriendship).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            OfferFriendship offerFriendship = db.OfferFriendships.Find(id);
            if (offerFriendship != null)
                db.OfferFriendships.Remove(offerFriendship);
        }
    }
}