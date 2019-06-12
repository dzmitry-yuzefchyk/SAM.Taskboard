namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_comments_datetime5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "CreatorId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "CreatorId");
        }
    }
}
