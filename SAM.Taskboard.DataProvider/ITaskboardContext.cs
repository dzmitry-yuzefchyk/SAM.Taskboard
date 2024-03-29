﻿using SAM.Taskboard.DataProvider.Models;
using System.Data.Entity;

namespace SAM.Taskboard.DataProvider
{
    public interface ITaskboardContext
    {
        DbSet<Activity> Activities { get; set; }
        DbSet<Board> Boards { get; set; }
        DbSet<BoardSettings> BoardSettings { get; set; }
        DbSet<Column> Columns { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<Attachment> Attachments { get; set; }
        DbSet<ProjectSettings> ProjectSettings { get; set; }
        DbSet<ProjectUser> ProjectsUsers { get; set; }
        DbSet<Task> Tasks { get; set; }
        DbSet<UserProfile> UserProfiles { get; set; }
        DbSet<UserSettings> UserSettings { get; set; }
    }
}
