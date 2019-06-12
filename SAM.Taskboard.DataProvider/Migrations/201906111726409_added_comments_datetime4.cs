namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_comments_datetime4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Comments", "CreatorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "CreatorId", c => c.Int(nullable: false));
        }
    }
}
