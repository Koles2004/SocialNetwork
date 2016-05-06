using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetworkWithIdentity.Interfaces;

namespace SocialNetworkWithIdentity.Models.Repositories
{
    public class ApplicationUserRepository : IRepository<ApplicationUser>
    {
        private ApplicationDbContext context;
        private UserStore<ApplicationUser> store;
        private ApplicationUserManager manager;

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            this.context = context;
            store = new UserStore<ApplicationUser>(context);
            manager = new ApplicationUserManager(store);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return manager.Users;
        }

        public ApplicationUser GetById(int id) { return null; }

        public ApplicationUser GetById(string id)
        {
            return manager.Users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<ApplicationUser> Find(Func<ApplicationUser, Boolean> predicate)
        {
            return manager.Users.Where(predicate).ToList();
        }

        public void Create(ApplicationUser user){}

        public void Update(ApplicationUser user)
        {
            context.Set<ApplicationUser>().AddOrUpdate(user);
            context.SaveChanges();
        }

        public void Delete(long id) { }

        public void Delete(string id)
        {
            ApplicationUser user = manager.Users.FirstOrDefault(u => u.Id != null && u.Id == id);

            if (user != null)
                manager.Delete(user);
        }
    }
}