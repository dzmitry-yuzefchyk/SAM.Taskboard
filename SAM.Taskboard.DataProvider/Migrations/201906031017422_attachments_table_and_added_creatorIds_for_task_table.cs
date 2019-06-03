namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attachments_table_and_added_creatorIds_for_task_table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BoardUsers", "BoardId", "dbo.Boards");
            DropForeignKey("dbo.BoardUsers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.BoardUsers", new[] { "UserId" });
            DropIndex("dbo.BoardUsers", new[] { "BoardId" });
            RenameColumn(table: "dbo.Tasks", name: "UserId", newName: "User_Id");
            RenameIndex(table: "dbo.Tasks", name: "IX_UserId", newName: "IX_User_Id");
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Document = c.Binary(),
                        TaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId);
            
            AddColumn("dbo.Boards", "CreatorId", c => c.String(maxLength: 128));
            AddColumn("dbo.Tasks", "AssigneeId", c => c.String(maxLength: 128));
            AddColumn("dbo.Tasks", "CreatorId", c => c.String(maxLength: 128));
            AddColumn("dbo.BoardSettings", "AccessToChangeBoard", c => c.Int(nullable: false));
            CreateIndex("dbo.Tasks", "AssigneeId");
            CreateIndex("dbo.Tasks", "CreatorId");
            CreateIndex("dbo.Boards", "CreatorId");
            AddForeignKey("dbo.Boards", "CreatorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Tasks", "AssigneeId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Tasks", "CreatorId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Tasks", "Attachments");
            DropTable("dbo.BoardUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BoardUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Role = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        BoardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tasks", "Attachments", c => c.Binary());
            DropForeignKey("dbo.Tasks", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Attachments", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "AssigneeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Boards", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Boards", new[] { "CreatorId" });
            DropIndex("dbo.Tasks", new[] { "CreatorId" });
            DropIndex("dbo.Tasks", new[] { "AssigneeId" });
            DropIndex("dbo.Attachments", new[] { "TaskId" });
            DropColumn("dbo.BoardSettings", "AccessToChangeBoard");
            DropColumn("dbo.Tasks", "CreatorId");
            DropColumn("dbo.Tasks", "AssigneeId");
            DropColumn("dbo.Boards", "CreatorId");
            DropTable("dbo.Attachments");
            RenameIndex(table: "dbo.Tasks", name: "IX_User_Id", newName: "IX_UserId");
            RenameColumn(table: "dbo.Tasks", name: "User_Id", newName: "UserId");
            CreateIndex("dbo.BoardUsers", "BoardId");
            CreateIndex("dbo.BoardUsers", "UserId");
            AddForeignKey("dbo.BoardUsers", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.BoardUsers", "BoardId", "dbo.Boards", "Id", cascadeDelete: true);
        }
    }
}
