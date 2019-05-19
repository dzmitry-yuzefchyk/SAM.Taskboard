namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedUserId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BoardUsers", new[] { "User_Id" });
            DropColumn("dbo.BoardUsers", "UserId");
            RenameColumn(table: "dbo.BoardUsers", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.BoardUsers", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.BoardUsers", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BoardUsers", new[] { "UserId" });
            AlterColumn("dbo.BoardUsers", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.BoardUsers", name: "UserId", newName: "User_Id");
            AddColumn("dbo.BoardUsers", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.BoardUsers", "User_Id");
        }
    }
}
