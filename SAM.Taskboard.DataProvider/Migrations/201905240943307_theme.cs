namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class theme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserSettings", "Theme", c => c.Int(nullable: false));
            DropColumn("dbo.BoardSettings", "Background");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BoardSettings", "Background", c => c.Binary());
            DropColumn("dbo.UserSettings", "Theme");
        }
    }
}
