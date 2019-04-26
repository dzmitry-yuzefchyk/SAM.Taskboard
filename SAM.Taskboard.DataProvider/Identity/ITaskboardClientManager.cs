using SAM.Taskboard.DataProvider.Models;
using System;

namespace SAM.Taskboard.DataProvider.Identity
{
    public interface ITaskboardClientManager : IDisposable
    {
        void Create(UserProfile profile, UserSettings settings);
        UserProfile GetProfile(string id);
        UserSettings GetSettings(string id);
    }
}
