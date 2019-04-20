namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedColorSettings : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserSettings", "PrimaryColor");
            DropColumn("dbo.UserSettings", "SecondaryColor");
            DropColumn("dbo.BoardSettings", "PrimaryColor");
            DropColumn("dbo.BoardSettings", "SecondaryColor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BoardSettings", "SecondaryColor", c => c.String(nullable: false));
            AddColumn("dbo.BoardSettings", "PrimaryColor", c => c.String(nullable: false));
            AddColumn("dbo.UserSettings", "SecondaryColor", c => c.String(nullable: false));
            AddColumn("dbo.UserSettings", "PrimaryColor", c => c.String(nullable: false));
        }
    }
}
