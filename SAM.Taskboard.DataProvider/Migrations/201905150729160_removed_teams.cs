namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removed_teams : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamUsers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.TeamUsers", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "TeamId" });
            DropIndex("dbo.TeamUsers", new[] { "Team_Id" });
            DropIndex("dbo.TeamUsers", new[] { "User_Id" });
            DropColumn("dbo.Projects", "TeamId");
            DropTable("dbo.Teams");
            DropTable("dbo.TeamUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TeamUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Team_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Projects", "TeamId", c => c.Int(nullable: false));
            CreateIndex("dbo.TeamUsers", "User_Id");
            CreateIndex("dbo.TeamUsers", "Team_Id");
            CreateIndex("dbo.Projects", "TeamId");
            AddForeignKey("dbo.TeamUsers", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.TeamUsers", "Team_Id", "dbo.Teams", "Id");
            AddForeignKey("dbo.Projects", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
        }
    }
}
