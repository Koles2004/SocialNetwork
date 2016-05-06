using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetworkWithIdentity.Models;

namespace SocialNetworkWithIdentity.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<SocialNetworkWithIdentity.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            // для миграции на Хостинге AutomaticMigrationsEnabled = true;
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SocialNetworkWithIdentity.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (!context.Roles.Any(r => r.Name == "AppAdmin"))
            {
                var manager1 = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var role = new IdentityRole { Name = "AppAdmin" };
                manager1.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "Vova1@gmail.com"))
            {
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                var userVova = new ApplicationUser { UserName = "Vova1@gmail.com", Email = "Vova1@gmail.com", Name = "Admin", Surname = "Adminovich", Status = "Не беспокоить!", Avatar = "gif_for_admin.gif" };
                manager.Create(userVova, "111111");
                var managerRole = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var role1 = managerRole.FindByName("AppAdmin");
                // не ассинхронно не хотело работать.
                // оказывается асснхронно тоже не работает, но сайт не отваливается)
                manager.AddToRoleAsync(userVova.Id, role1.Name);
            }
        }
    }
}
