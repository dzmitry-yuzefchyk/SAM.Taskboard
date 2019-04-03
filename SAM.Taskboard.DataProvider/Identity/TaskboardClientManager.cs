using SAM.Taskboard.DataProvider.Models;

namespace SAM.Taskboard.DataProvider.Identity
{
    class TaskboardClientManager : ITaskboardClientManager
    {
        private TaskboardContext context;
        public TaskboardClientManager(TaskboardContext context)
        {
            this.context = context;
        }
        public void Create(UserProfile profile, UserSettings settings)
        {
            context.UserProfiles.Add(profile);
            context.UserSettings.Add(settings);
            context.SaveChanges();
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
