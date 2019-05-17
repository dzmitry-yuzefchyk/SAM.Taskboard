namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectUsers", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ProjectUsers", new[] { "Project_Id" });
            RenameColumn(table: "dbo.ProjectUsers", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.ProjectUsers", name: "Project_Id", newName: "ProjectId");
            RenameIndex(table: "dbo.ProjectUsers", name: "IX_User_Id", newName: "IX_UserId");
            AlterColumn("dbo.ProjectUsers", "ProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectUsers", "ProjectId");
            AddForeignKey("dbo.ProjectUsers", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectUsers", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectUsers", new[] { "ProjectId" });
            AlterColumn("dbo.ProjectUsers", "ProjectId", c => c.Int());
            RenameIndex(table: "dbo.ProjectUsers", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.ProjectUsers", name: "ProjectId", newName: "Project_Id");
            RenameColumn(table: "dbo.ProjectUsers", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.ProjectUsers", "Project_Id");
            AddForeignKey("dbo.ProjectUsers", "Project_Id", "dbo.Projects", "Id");
        }
    }
}
