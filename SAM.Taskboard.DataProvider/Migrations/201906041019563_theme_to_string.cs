namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class theme_to_string : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserSettings", "Theme", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserSettings", "Theme", c => c.Int(nullable: false));
        }
    }
}
