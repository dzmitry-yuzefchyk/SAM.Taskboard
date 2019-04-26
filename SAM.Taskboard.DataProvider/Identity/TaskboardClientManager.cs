using SAM.Taskboard.DataProvider.Models;

namespace SAM.Taskboard.DataProvider.Identity
{
    public class TaskboardClientManager : ITaskboardClientManager
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
        public UserProfile GetProfile(string id)
        {
            return context.UserProfiles.Find(id);
        }
        public UserSettings GetSettings(string id)
        {
            return context.UserSettings.Find(id);
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
