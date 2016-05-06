namespace SocialNetworkWithIdentity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        Image = c.String(),
                        EventType_Id = c.Long(nullable: false),
                        Sender_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventTypes", t => t.EventType_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.Sender_Id)
                .Index(t => t.EventType_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.EventTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Surname = c.String(),
                        DateOfBirth = c.DateTime(),
                        Avatar = c.String(),
                        Gender = c.Boolean(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        Room_Id = c.Long(nullable: false),
                        Sender_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rooms", t => t.Room_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.Sender_Id)
                .Index(t => t.Room_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.Friendships",
                c => new
                    {
                        FriendshipId = c.Long(nullable: false, identity: true),
                        DateOfCreate = c.DateTime(nullable: false),
                        DateOfDelete = c.DateTime(),
                        FirstUser_Id = c.String(nullable: false, maxLength: 128),
                        SecondUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.FriendshipId)
                .ForeignKey("dbo.ApplicationUsers", t => t.FirstUser_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.SecondUser_Id)
                .Index(t => t.FirstUser_Id)
                .Index(t => t.SecondUser_Id);
            
            CreateTable(
                "dbo.OfferFriendships",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Status = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Offer_Id = c.String(nullable: false, maxLength: 128),
                        Received_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.Offer_Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.Received_Id)
                .Index(t => t.Offer_Id)
                .Index(t => t.Received_Id);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserEvents",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Event_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Event_Id })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Event_Id);
            
            CreateTable(
                "dbo.ApplicationUserRooms",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Room_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Room_Id })
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Rooms", t => t.Room_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Room_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.OfferFriendships", "Received_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.OfferFriendships", "Offer_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Friendships", "SecondUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Friendships", "FirstUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Events", "Sender_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserRooms", "Room_Id", "dbo.Rooms");
            DropForeignKey("dbo.ApplicationUserRooms", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Messages", "Sender_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Messages", "Room_Id", "dbo.Rooms");
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUserEvents", "Event_Id", "dbo.Events");
            DropForeignKey("dbo.ApplicationUserEvents", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.Events", "EventType_Id", "dbo.EventTypes");
            DropIndex("dbo.ApplicationUserRooms", new[] { "Room_Id" });
            DropIndex("dbo.ApplicationUserRooms", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserEvents", new[] { "Event_Id" });
            DropIndex("dbo.ApplicationUserEvents", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OfferFriendships", new[] { "Received_Id" });
            DropIndex("dbo.OfferFriendships", new[] { "Offer_Id" });
            DropIndex("dbo.Friendships", new[] { "SecondUser_Id" });
            DropIndex("dbo.Friendships", new[] { "FirstUser_Id" });
            DropIndex("dbo.Messages", new[] { "Sender_Id" });
            DropIndex("dbo.Messages", new[] { "Room_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Events", new[] { "Sender_Id" });
            DropIndex("dbo.Events", new[] { "EventType_Id" });
            DropTable("dbo.ApplicationUserRooms");
            DropTable("dbo.ApplicationUserEvents");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.OfferFriendships");
            DropTable("dbo.Friendships");
            DropTable("dbo.Messages");
            DropTable("dbo.Rooms");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.EventTypes");
            DropTable("dbo.Events");
        }
    }
}
