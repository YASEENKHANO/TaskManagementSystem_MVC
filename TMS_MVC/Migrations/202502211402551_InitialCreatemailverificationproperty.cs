namespace TMS_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreatemailverificationproperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsEmailVerified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsEmailVerified");
        }
    }
}
