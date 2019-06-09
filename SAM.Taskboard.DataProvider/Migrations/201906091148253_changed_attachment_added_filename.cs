namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changed_attachment_added_filename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attachments", "Name");
        }
    }
}
