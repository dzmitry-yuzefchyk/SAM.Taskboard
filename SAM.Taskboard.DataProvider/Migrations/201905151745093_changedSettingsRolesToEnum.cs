namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedSettingsRolesToEnum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectSettings", "AccessToDeleteBoard", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectSettings", "AccessToChangeProject", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectSettings", "AccessToCreateBoard", c => c.Int(nullable: false));
            AlterColumn("dbo.BoardSettings", "AccessToDeleteTask", c => c.Int(nullable: false));
            AlterColumn("dbo.BoardSettings", "AccessToChangeTask", c => c.Int(nullable: false));
            AlterColumn("dbo.BoardSettings", "AccessToCreateTask", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BoardSettings", "AccessToCreateTask", c => c.String(nullable: false));
            AlterColumn("dbo.BoardSettings", "AccessToChangeTask", c => c.String(nullable: false));
            AlterColumn("dbo.BoardSettings", "AccessToDeleteTask", c => c.String(nullable: false));
            AlterColumn("dbo.ProjectSettings", "AccessToCreateBoard", c => c.String(nullable: false));
            AlterColumn("dbo.ProjectSettings", "AccessToChangeProject", c => c.String(nullable: false));
            AlterColumn("dbo.ProjectSettings", "AccessToDeleteBoard", c => c.String(nullable: false));
        }
    }
}
