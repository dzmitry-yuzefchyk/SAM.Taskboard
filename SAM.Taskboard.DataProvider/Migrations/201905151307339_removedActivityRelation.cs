namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedActivityRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectUsers", "ActivityId", "dbo.Activities");
            DropIndex("dbo.ProjectUsers", new[] { "ActivityId" });
            DropColumn("dbo.ProjectUsers", "ActivityId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectUsers", "ActivityId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectUsers", "ActivityId");
            AddForeignKey("dbo.ProjectUsers", "ActivityId", "dbo.Activities", "Id", cascadeDelete: true);
        }
    }
}
