namespace SAM.Taskboard.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 256),
                        Date = c.DateTime(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 64),
                        About = c.String(maxLength: 256),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.Boards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 64),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.BoardUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        Board_Id = c.Int(),
                        Role_Id = c.String(maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boards", t => t.Board_Id)
                .ForeignKey("dbo.AspNetRoles", t => t.Role_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Board_Id)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        About = c.String(maxLength: 256),
                        Icon = c.Binary(maxLength: 2048),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ProjectUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        Project_Id = c.Int(),
                        Rolde_Id = c.String(maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .ForeignKey("dbo.AspNetRoles", t => t.Rolde_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.ActivityId)
                .Index(t => t.Project_Id)
                .Index(t => t.Rolde_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserSettings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PrimaryColor = c.String(nullable: false, maxLength: 64),
                        SecondaryColor = c.String(nullable: false, maxLength: 64),
                        EmailNotification = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.TeamUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Team_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Team_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Columns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 64),
                        BoardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boards", t => t.BoardId, cascadeDelete: true)
                .Index(t => t.BoardId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 64),
                        Content = c.String(maxLength: 1024),
                        Attachments = c.Binary(maxLength: 3096),
                        Type = c.String(nullable: false, maxLength: 11),
                        Priority = c.String(nullable: false, maxLength: 6),
                        Severity = c.String(maxLength: 8),
                        Asignee = c.String(maxLength: 64),
                        TimeToComplete = c.Time(nullable: false, precision: 7),
                        StartTime = c.DateTime(nullable: false),
                        ColumnId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Columns", t => t.ColumnId, cascadeDelete: true)
                .Index(t => t.ColumnId);
            
            CreateTable(
                "dbo.BoardSettings",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Background = c.Binary(maxLength: 2048),
                        PrimaryColor = c.String(nullable: false, maxLength: 64),
                        SecondaryColor = c.String(nullable: false, maxLength: 64),
                        AccessToDeleteTask = c.String(nullable: false),
                        AccessToChangeTask = c.String(nullable: false),
                        AccessToCreateTask = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Boards", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ProjectSettings",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Background = c.Binary(maxLength: 2048),
                        AccessToDeleteBoard = c.String(nullable: false),
                        AccessToChangeProject = c.String(nullable: false),
                        AccessToCreateTask = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProjectSettings", "Id", "dbo.Projects");
            DropForeignKey("dbo.BoardSettings", "Id", "dbo.Boards");
            DropForeignKey("dbo.Boards", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Tasks", "ColumnId", "dbo.Columns");
            DropForeignKey("dbo.Columns", "BoardId", "dbo.Boards");
            DropForeignKey("dbo.TeamUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TeamUsers", "Team_Id", "dbo.Teams");
            DropForeignKey("dbo.Projects", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.UserSettings", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectUsers", "Rolde_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProjectUsers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectUsers", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.UserProfiles", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BoardUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.BoardUsers", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.BoardUsers", "Board_Id", "dbo.Boards");
            DropForeignKey("dbo.Activities", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectSettings", new[] { "Id" });
            DropIndex("dbo.BoardSettings", new[] { "Id" });
            DropIndex("dbo.Tasks", new[] { "ColumnId" });
            DropIndex("dbo.Columns", new[] { "BoardId" });
            DropIndex("dbo.TeamUsers", new[] { "User_Id" });
            DropIndex("dbo.TeamUsers", new[] { "Team_Id" });
            DropIndex("dbo.UserSettings", new[] { "Id" });
            DropIndex("dbo.ProjectUsers", new[] { "User_Id" });
            DropIndex("dbo.ProjectUsers", new[] { "Rolde_Id" });
            DropIndex("dbo.ProjectUsers", new[] { "Project_Id" });
            DropIndex("dbo.ProjectUsers", new[] { "ActivityId" });
            DropIndex("dbo.UserProfiles", new[] { "Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.BoardUsers", new[] { "User_Id" });
            DropIndex("dbo.BoardUsers", new[] { "Role_Id" });
            DropIndex("dbo.BoardUsers", new[] { "Board_Id" });
            DropIndex("dbo.Boards", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "TeamId" });
            DropIndex("dbo.Activities", new[] { "ProjectId" });
            DropTable("dbo.ProjectSettings");
            DropTable("dbo.BoardSettings");
            DropTable("dbo.Tasks");
            DropTable("dbo.Columns");
            DropTable("dbo.Teams");
            DropTable("dbo.TeamUsers");
            DropTable("dbo.UserSettings");
            DropTable("dbo.ProjectUsers");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.BoardUsers");
            DropTable("dbo.Boards");
            DropTable("dbo.Projects");
            DropTable("dbo.Activities");
        }
    }
}
