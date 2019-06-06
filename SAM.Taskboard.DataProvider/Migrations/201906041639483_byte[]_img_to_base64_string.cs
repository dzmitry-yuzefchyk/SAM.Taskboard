namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class byte_img_to_base64_string : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserProfiles", "Icon", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserProfiles", "Icon", c => c.Binary());
        }
    }
}
