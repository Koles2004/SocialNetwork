using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetworkWithIdentity.Models.DomainModels;
using SocialNetworkWithIdentity.Migrations;

namespace SocialNetworkWithIdentity.Models
{
    
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { set; get; }
        public string Surname { set; get; }
        public DateTime? DateOfBirth { set; get; }
        public string Avatar { set; get; }
        public string City { set; get; }
        public bool? Gender { set; get; }
        public DateTime? DateOfActivity { set; get; }

        public string Status { set; get; }
        
        public ICollection<Event> Events { set; get; }
        public ICollection<Room> Rooms { set; get; }
        public ICollection<OfferFriendship> OfferFriendships { set; get; }
        public ICollection<Friendship> Friendships { set; get; }
        public ICollection<Message> Messages { set; get; }
        public ICollection<Photo> Photos { set; get; }

        public ApplicationUser()
        {
            DateOfActivity = DateTime.Now;
            Events = new Collection<Event>();
            Rooms = new Collection<Room>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    // ГДЕ-ТО ТУТ косяки с ролями. Не дает присваивать одну и ту же роль многим пользователям.
    // должно помочь для работы OnModelCreating
    public class IdentityUserLoginConfiguration : EntityTypeConfiguration<IdentityUserLogin>
    {
        public IdentityUserLoginConfiguration()
        {
            HasKey(iul => iul.UserId);
        }
    }

    public class IdentityUserRoleConfiguration : EntityTypeConfiguration<IdentityUserRole>
    {
        public IdentityUserRoleConfiguration()
        {
            HasKey(iur => iur.RoleId);
        }
    }
    //--------------------------------------

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<OfferFriendship> OfferFriendships { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            Database.SetInitializer<ApplicationDbContext>(new MyContextInitializer());
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // должно помочь для работы OnModelCreating
            modelBuilder.Configurations.Add(new IdentityUserLoginConfiguration());
            modelBuilder.Configurations.Add(new IdentityUserRoleConfiguration());
            //--------------------------------------

            //configure model with fluent API

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Events)
                .WithMany(e => e.Owners);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Rooms)
                .WithMany(r => r.Users);

            modelBuilder.Entity<Friendship>()
                .HasRequired(f => f.FirstUser)
                .WithMany(u => u.Friendships);

            modelBuilder.Entity<Friendship>()
               .HasRequired(f => f.SecondUser);

            modelBuilder.Entity<OfferFriendship>()
                .HasRequired(f => f.Offer)
                .WithMany(u => u.OfferFriendships);

            modelBuilder.Entity<OfferFriendship>()
                .HasRequired(f => f.Received);

            modelBuilder.Entity<Event>()
                .HasRequired(e => e.Sender);

            modelBuilder.Entity<Message>()
                .HasRequired(m => m.Sender)
                .WithMany(u => u.Messages);

            modelBuilder.Entity<Message>()
                .HasRequired(m => m.Room)
                .WithMany(r => r.Messages);

			modelBuilder.Entity<Room>().
				HasMany(r => r.Users).
				WithRequired();

            modelBuilder.Entity<Photo>().
                HasRequired(p => p.Sender).
                WithMany(u => u.Photos);
        }
    }

       // ВНИМАНИЕ!
    // if you delete Database, then you must uncomment: class MyContextInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // After the Database is created, cooment that string and uncomment : public class MyContextInitializer : MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>
    // And then comment previous string and uncomment: class MyContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    
    // чтобы создалась новая база. После создания нужно снова ЗАКОМЕНТИТЬ эту строку и РАСКОМЕНТИТЬ
    // class MyContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>

    // Для Миграции 
    // public class MyContextInitializer : MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>

    // Если понадобилось заюзать эти классы, то комментим в миграции ткла методов Up() и Down()
    // class MyContextInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    class MyContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected virtual void Seed(ApplicationDbContext db)
        {
            if (!db.Roles.Any(r => r.Name == "AppAdmin"))
            {
                var manager1 = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                var role = new IdentityRole { Name = "AppAdmin" };
                manager1.Create(role);
            }

            if (!db.Users.Any(u => u.UserName == "Vova@gmail.com"))
            {
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                var userVova = new ApplicationUser { UserName = "Vova@gmail.com", Email = "Vova@gmail.com", Name = "Admin", Surname = "Adminovich", Status = "Не беспокоить!", Avatar = "gif_for_admin.gif" };
                manager.Create(userVova, "111111");
                manager.AddToRole(userVova.Id, "AppAdmin");
            }
        }
    }
}