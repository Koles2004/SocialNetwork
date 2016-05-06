using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Repositories
{
    public class RoomRepository : IRepository<Room>
    {
        private ApplicationDbContext db;

        public RoomRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Room> GetAll()
        {
            return db.Rooms.Include(r => r.Users).Include(r => r.Messages);
        }

        public Room GetById(int id)
        {
            return db.Rooms.Find(id);
        }

        public Room GetById(string id) { return null; }

        public IEnumerable<Room> Find(Func<Room, Boolean> predicate)
        {
            return db.Rooms.Where(predicate).ToList();
        }

        public void Create(Room room)
        {
            db.Rooms.Add(room);
        }

        public void Update(Room room)
        {
            db.Entry(room).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            Room room = db.Rooms.Find(id);
            if (room != null)
                db.Rooms.Remove(room);
        }
    }
}