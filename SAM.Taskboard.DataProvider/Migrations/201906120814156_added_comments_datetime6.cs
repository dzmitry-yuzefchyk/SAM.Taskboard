namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_comments_datetime6 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Comments", new[] { "Creator_Id" });
            DropColumn("dbo.Comments", "CreatorId");
            RenameColumn(table: "dbo.Comments", name: "Creator_Id", newName: "CreatorId");
            AlterColumn("dbo.Comments", "CreatorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Comments", "CreatorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comments", new[] { "CreatorId" });
            AlterColumn("dbo.Comments", "CreatorId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Comments", name: "CreatorId", newName: "Creator_Id");
            AddColumn("dbo.Comments", "CreatorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "Creator_Id");
        }
    }
}
