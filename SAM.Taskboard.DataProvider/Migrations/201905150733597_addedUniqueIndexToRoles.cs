namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedUniqueIndexToRoles : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Roles", "Id", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Roles", new[] { "Id" });
        }
    }
}
