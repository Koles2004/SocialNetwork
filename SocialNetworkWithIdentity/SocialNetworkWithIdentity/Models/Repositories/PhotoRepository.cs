using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Repositories
{
    public class PhotoRepository : IRepository<Photo>
    {
                private ApplicationDbContext db;

        public PhotoRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public IEnumerable<Photo> GetAll()
        {
            return db.Photos;
        }

        public Photo GetById(int id)
        {
            return db.Photos.Find(id);
        }

        public Photo GetById(string id) { return null; }

        public IEnumerable<Photo> Find(Func<Photo, Boolean> predicate)
        {
            return db.Photos.Where(predicate).ToList();
        }

        public void Create(Photo Photo)
        {
            db.Photos.Add(Photo);
        }

        public void Update(Photo Photo)
        {
            db.Entry(Photo).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            Photo Photo = db.Photos.Find(id);
            if (Photo != null)
                db.Photos.Remove(Photo);
        }
    }
}