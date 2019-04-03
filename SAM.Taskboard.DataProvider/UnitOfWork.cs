using Microsoft.AspNet.Identity.EntityFramework;
using SAM.Taskboard.DataProvider.Identity;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.DataProvider.Repository;

namespace SAM.Taskboard.DataProvider
{
    public class UnitOfWork : IUnitOfWork
    {
        private TaskboardContext context;

        private TaskboardRoleManager roleManager;
        private TaskboardUserManager userManager;
        private TaskboardClientManager clientManager;
        private IRepository<Activity> activityRepository;

        public UnitOfWork()
        {
            context = new TaskboardContext();
            userManager = new TaskboardUserManager(new UserStore<User>(context));
            roleManager = new TaskboardRoleManager(new RoleStore<Role>(context));
            clientManager = new TaskboardClientManager(context);
            activityRepository = new ActivityRepository(context);
        }
        public TaskboardUserManager UserManager { get => userManager; }
        public TaskboardRoleManager RoleManager { get => roleManager; }
        public ITaskboardClientManager ClientManager { get => clientManager; }
        public IRepository<Board> Boards => throw new System.NotImplementedException();
        public void Save()
        {
            context.SaveChanges();
        }
        public void Dispose()
        {
            userManager.Dispose();
            roleManager.Dispose();
            clientManager.Dispose();
        }
    }
}
