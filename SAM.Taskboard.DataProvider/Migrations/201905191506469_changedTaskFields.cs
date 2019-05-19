namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedTaskFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Tasks", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.Tasks", "Priority", c => c.Int(nullable: false));
            AlterColumn("dbo.Tasks", "Severity", c => c.Int(nullable: false));
            CreateIndex("dbo.Tasks", "UserId");
            AddForeignKey("dbo.Tasks", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Tasks", "Asignee");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "Asignee", c => c.String());
            DropForeignKey("dbo.Tasks", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Tasks", new[] { "UserId" });
            AlterColumn("dbo.Tasks", "Severity", c => c.String());
            AlterColumn("dbo.Tasks", "Priority", c => c.String(nullable: false));
            AlterColumn("dbo.Tasks", "Type", c => c.String(nullable: false));
            DropColumn("dbo.Tasks", "UserId");
        }
    }
}
