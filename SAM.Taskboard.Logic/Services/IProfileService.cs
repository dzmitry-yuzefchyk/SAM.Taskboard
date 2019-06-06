using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model.Profile;

namespace SAM.Taskboard.Logic.Services
{
    public interface IProfileService
    {
        string GetUserName(string userId);
        string GetUserIcon(string userId);
        OperationResult<ProfileSettingsViewModel> GetUserProfile(string userId);
        GenericServiceResult UpdateUserProfile(ProfileSettingsViewModel model, string userId);
    }
}
