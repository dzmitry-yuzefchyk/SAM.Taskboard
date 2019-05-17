namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedBoardId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BoardUsers", "Board_Id", "dbo.Boards");
            DropIndex("dbo.BoardUsers", new[] { "Board_Id" });
            RenameColumn(table: "dbo.BoardUsers", name: "Board_Id", newName: "BoardId");
            AddColumn("dbo.BoardUsers", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.BoardUsers", "BoardId", c => c.Int(nullable: false));
            CreateIndex("dbo.BoardUsers", "BoardId");
            AddForeignKey("dbo.BoardUsers", "BoardId", "dbo.Boards", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoardUsers", "BoardId", "dbo.Boards");
            DropIndex("dbo.BoardUsers", new[] { "BoardId" });
            AlterColumn("dbo.BoardUsers", "BoardId", c => c.Int());
            DropColumn("dbo.BoardUsers", "UserId");
            RenameColumn(table: "dbo.BoardUsers", name: "BoardId", newName: "Board_Id");
            CreateIndex("dbo.BoardUsers", "Board_Id");
            AddForeignKey("dbo.BoardUsers", "Board_Id", "dbo.Boards", "Id");
        }
    }
}
