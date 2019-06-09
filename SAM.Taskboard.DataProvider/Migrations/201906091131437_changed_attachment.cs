namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed_attachment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "Type", c => c.String());
            AddColumn("dbo.Attachments", "Extension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attachments", "Extension");
            DropColumn("dbo.Attachments", "Type");
        }
    }
}
