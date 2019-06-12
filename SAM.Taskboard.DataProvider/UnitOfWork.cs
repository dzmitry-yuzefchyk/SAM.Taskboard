using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using SAM.Taskboard.DataProvider.Identity;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.DataProvider.Repository;
using System;

namespace SAM.Taskboard.DataProvider
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskboardContext context;

        public UnitOfWork()
        {
            context = new TaskboardContext();
            UserManager = new TaskboardUserManager(new UserStore<User>(context));
            ClientManager = new TaskboardClientManager(context);
            Activities = new GenericRepository<Activity>(context);
            Boards = new GenericRepository<Board>(context);
            BoardSettings = new GenericRepository<BoardSettings>(context);
            Columns = new GenericRepository<Column>(context);
            Projects = new GenericRepository<Project>(context);
            ProjectSettings = new GenericRepository<ProjectSettings>(context);
            ProjectUser = new GenericRepository<ProjectUser>(context);
            Tasks = new GenericRepository<Task>(context);
            Users = new GenericRepository<User>(context);
            UserProfiles = new GenericRepository<UserProfile>(context);
            UserSettings = new GenericRepository<UserSettings>(context);
            Attachments = new GenericRepository<Attachment>(context);
            Comments = new GenericRepository<Comment>(context);

            var provider = new DpapiDataProtectionProvider("SAM.Taskboard");
            UserManager.UserTokenProvider = new DataProtectorTokenProvider<User, string>(provider.Create("UserToken"))
                as IUserTokenProvider<User, string>;
        }

        public TaskboardUserManager UserManager { get; }

        public ITaskboardClientManager ClientManager { get; }

        public IRepository<Activity> Activities { get; }

        public IRepository<Board> Boards { get; }

        public IRepository<BoardSettings> BoardSettings { get; }

        public IRepository<Column> Columns { get; }

        public IRepository<Project> Projects { get; }

        public IRepository<ProjectSettings> ProjectSettings { get; }

        public IRepository<ProjectUser> ProjectUser { get; }

        public IRepository<Task> Tasks { get; }

        public IRepository<Attachment> Attachments { get; }

        public IRepository<User> Users { get; }

        public IRepository<UserProfile> UserProfiles { get; }

        public IRepository<UserSettings> UserSettings { get; }

        public IRepository<Comment> Comments { get; }

        public void Dispose()
        {
            UserManager.Dispose();
            ClientManager.Dispose();
            Activities.Dispose();
            Boards.Dispose();
            BoardSettings.Dispose();
            Columns.Dispose();
            Attachments.Dispose();
            Projects.Dispose();
            ProjectSettings.Dispose();
            ProjectUser.Dispose();
            Tasks.Dispose();
            Users.Dispose();
            UserProfiles.Dispose();
            UserSettings.Dispose();
            Comments.Dispose();
        }
    }
}
