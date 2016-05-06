namespace SocialNetworkWithIdentity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "City", c => c.String());
            AddColumn("dbo.ApplicationUsers", "DateOfActivity", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "DateOfActivity");
            DropColumn("dbo.ApplicationUsers", "City");
        }
    }
}
