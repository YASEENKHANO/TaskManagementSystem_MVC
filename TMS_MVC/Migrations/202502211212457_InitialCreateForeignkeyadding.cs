namespace TMS_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreateForeignkeyadding : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "AssignedUser_Id", "dbo.Users");
            DropIndex("dbo.Tasks", new[] { "AssignedUser_Id" });
            DropColumn("dbo.Tasks", "AssignedTo");
            RenameColumn(table: "dbo.Tasks", name: "AssignedUser_Id", newName: "AssignedTo");
            AlterColumn("dbo.Tasks", "AssignedTo", c => c.Int(nullable: false));
            CreateIndex("dbo.Tasks", "AssignedTo");
            AddForeignKey("dbo.Tasks", "AssignedTo", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "AssignedTo", "dbo.Users");
            DropIndex("dbo.Tasks", new[] { "AssignedTo" });
            AlterColumn("dbo.Tasks", "AssignedTo", c => c.Int());
            RenameColumn(table: "dbo.Tasks", name: "AssignedTo", newName: "AssignedUser_Id");
            AddColumn("dbo.Tasks", "AssignedTo", c => c.Int(nullable: false));
            CreateIndex("dbo.Tasks", "AssignedUser_Id");
            AddForeignKey("dbo.Tasks", "AssignedUser_Id", "dbo.Users", "Id");
        }
    }
}
