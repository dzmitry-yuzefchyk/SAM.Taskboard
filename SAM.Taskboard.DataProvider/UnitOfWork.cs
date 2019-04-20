using Microsoft.AspNet.Identity.EntityFramework;
using SAM.Taskboard.DataProvider.Identity;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.DataProvider.Repository;

namespace SAM.Taskboard.DataProvider
{
    public class UnitOfWork : IUnitOfWork
    {
        private TaskboardContext context;

        public UnitOfWork()
        {
            context = new TaskboardContext();
            UserManager = new TaskboardUserManager(new UserStore<User>(context));
            ClientManager = new TaskboardClientManager(context);
            Activities = new GenericRepository<Activity>(context);
            Boards = new GenericRepository<Board>(context);
            BoardSettings = new GenericRepository<BoardSettings>(context);
            BoardUser = new GenericRepository<BoardUser>(context);
            Columns = new GenericRepository<Column>(context);
            Projects = new GenericRepository<Project>(context);
            ProjectSettings = new GenericRepository<ProjectSettings>(context);
            ProjectUser = new GenericRepository<ProjectUser>(context);
            Roles = new GenericRepository<Role>(context);
            Tasks = new GenericRepository<Task>(context);
            Teams = new GenericRepository<Team>(context);
            TeamUser = new GenericRepository<TeamUser>(context);
            Users = new GenericRepository<User>(context);
            UserProfiles = new GenericRepository<UserProfile>(context);
            UserSettings = new GenericRepository<UserSettings>(context);
        }

        public TaskboardUserManager UserManager { get; }

        public ITaskboardClientManager ClientManager { get; }

        public IRepository<Activity> Activities { get; }

        public IRepository<Board> Boards { get; }

        public IRepository<BoardSettings> BoardSettings { get; }

        public IRepository<BoardUser> BoardUser { get; }

        public IRepository<Column> Columns { get; }

        public IRepository<Project> Projects { get; }

        public IRepository<ProjectSettings> ProjectSettings { get; }

        public IRepository<ProjectUser> ProjectUser { get; }

        public IRepository<Role> Roles { get; }

        public IRepository<Task> Tasks { get; }

        public IRepository<Team> Teams { get; }

        public IRepository<TeamUser> TeamUser { get; }

        public IRepository<User> Users { get; }

        public IRepository<UserProfile> UserProfiles { get; }

        public IRepository<UserSettings> UserSettings { get; }

        public void Dispose()
        {
            UserManager.Dispose();
            ClientManager.Dispose();
            Activities.Dispose();
            Boards.Dispose();
            BoardSettings.Dispose();
            BoardUser.Dispose();
            Columns.Dispose();
            Projects.Dispose();
            ProjectSettings.Dispose();
            ProjectUser.Dispose();
            Roles.Dispose();
            Tasks.Dispose();
            Teams.Dispose();
            TeamUser.Dispose();
            Users.Dispose();
            UserProfiles.Dispose();
            UserSettings.Dispose();
        }
    }
}
