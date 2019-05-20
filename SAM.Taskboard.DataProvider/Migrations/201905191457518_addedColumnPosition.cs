namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedColumnPosition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Columns", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Columns", "Position");
        }
    }
}
