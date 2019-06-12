namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_comments_datetime3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        TaskId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Creator_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Creator_Id)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId)
                .Index(t => t.Creator_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.Comments", "Creator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "Creator_Id" });
            DropIndex("dbo.Comments", new[] { "TaskId" });
            DropTable("dbo.Comments");
        }
    }
}
