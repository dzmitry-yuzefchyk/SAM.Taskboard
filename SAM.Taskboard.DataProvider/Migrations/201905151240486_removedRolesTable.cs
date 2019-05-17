namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedRolesTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BoardUsers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ProjectUsers", "RoleId", "dbo.Roles");
            DropIndex("dbo.BoardUsers", new[] { "RoleId" });
            DropIndex("dbo.Roles", new[] { "Id" });
            DropIndex("dbo.ProjectUsers", new[] { "RoleId" });
            AddColumn("dbo.BoardUsers", "Role", c => c.String());
            AddColumn("dbo.ProjectUsers", "Role", c => c.String());
            DropColumn("dbo.BoardUsers", "RoleId");
            DropColumn("dbo.ProjectUsers", "RoleId");
            DropTable("dbo.Roles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ProjectUsers", "RoleId", c => c.Int(nullable: false));
            AddColumn("dbo.BoardUsers", "RoleId", c => c.Int(nullable: false));
            DropColumn("dbo.ProjectUsers", "Role");
            DropColumn("dbo.BoardUsers", "Role");
            CreateIndex("dbo.ProjectUsers", "RoleId");
            CreateIndex("dbo.Roles", "Id", unique: true);
            CreateIndex("dbo.BoardUsers", "RoleId");
            AddForeignKey("dbo.ProjectUsers", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BoardUsers", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
        }
    }
}
