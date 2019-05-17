using SAM.Taskboard.DataProvider.Identity;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.DataProvider.Repository;
using System;

namespace SAM.Taskboard.DataProvider
{
    public interface IUnitOfWork : IDisposable
    {
        TaskboardUserManager UserManager { get; }
        ITaskboardClientManager ClientManager { get; }
        IRepository<Activity> Activities { get; }
        IRepository<Board> Boards { get; }
        IRepository<BoardSettings> BoardSettings { get; }
        IRepository<BoardUser> BoardUser { get; }
        IRepository<Column> Columns { get; }
        IRepository<Project> Projects { get; }
        IRepository<ProjectSettings> ProjectSettings { get; }
        IRepository<ProjectUser> ProjectUser { get; }
        IRepository<Task> Tasks { get; }
        IRepository<User> Users { get; }
        IRepository<UserProfile> UserProfiles { get; }
        IRepository<UserSettings> UserSettings { get; }
    }
}
