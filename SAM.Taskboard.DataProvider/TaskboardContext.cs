﻿using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SAM.Taskboard.DataProvider.Models;

namespace SAM.Taskboard.DataProvider
{
    class TaskboardContext : IdentityDbContext<User>
    {
        public TaskboardContext()
            : base("Taskboard")
        { }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<BoardSettings> BoardSettings { get; set; }
        public DbSet<BoardUser> BoardsUsers { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectSettings> ProjectSettings { get; set; }
        public DbSet<ProjectUser> ProjectsUsers { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamUser> TeamsUsers { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
    }
}
