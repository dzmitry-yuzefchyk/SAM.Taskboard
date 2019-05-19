namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedRolesToEnum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BoardUsers", "Role", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectUsers", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectUsers", "Role", c => c.String());
            AlterColumn("dbo.BoardUsers", "Role", c => c.String());
        }
    }
}
