using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;
using WebGrease.Css.Extensions;

namespace SocialNetworkWithIdentity.Models.Infrastructure
{
    public class ApplicationUserRepository : IRepository<ApplicationUser>
    {
        private ApplicationDbContext db;
        private ApplicationUserManager manager;

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            this.db = context;
            manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return manager.Users;
        }

        public ApplicationUser Get(int id) { return null; }

        public ApplicationUser Get(string id)
        {
            return manager.Users.FirstOrDefault(u => u.Id != null && u.Id == id);
        }

        public void Create(ApplicationUser user){}

        public void Update(ApplicationUser user)
        {
            manager.Update(user);
        }

        public void Delete(int id) { }

        public void Delete(string id)
        {
            ApplicationUser user = manager.Users.FirstOrDefault(u => u.Id != null && u.Id == id);
            if (user != null)
                manager.Delete(user);
        }
    }
}