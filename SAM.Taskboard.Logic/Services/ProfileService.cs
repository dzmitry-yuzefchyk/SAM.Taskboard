using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using SAM.Taskboard.Model;
using SAM.Taskboard.Model.Profile;
using System.Web.Configuration;

namespace SAM.Taskboard.Logic.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProfileService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public string GetUserName(string userId)
        {
            return unitOfWork.UserProfiles.GetFirstOrDefaultWhere(u => u.Id == userId).Name;
        }

        public string GetUserIcon(string userId)
        {
            return unitOfWork.ClientManager.GetProfile(userId).Icon;
        }

        public OperationResult<ProfileSettingsViewModel> GetUserProfile(string userId)
        {
            try
            {
                UserProfile profile = unitOfWork.ClientManager.GetProfile(userId);
                UserSettings settings = unitOfWork.ClientManager.GetSettings(userId);

                ProfileSettingsViewModel model = new ProfileSettingsViewModel
                {
                    Name = profile.Name,
                    About = profile.About,
                    Icon = profile.Icon,
                    EmailNotifications = settings.EmailNotification,
                    Theme = settings.Theme
                };

                return new OperationResult<ProfileSettingsViewModel> { Model = model, Message = GenericServiceResult.Success };
            }
            catch
            {
                return new OperationResult<ProfileSettingsViewModel> { Model = null, Message = GenericServiceResult.Error };
            }
        }

        public GenericServiceResult UpdateUserProfile(ProfileSettingsViewModel model, string userId)
        {
            try
            {
                UserProfile profile = new UserProfile { Name = model.Name, About = model.About, Icon = model.Icon, Id = userId };
                UserSettings settings = new UserSettings { EmailNotification = model.EmailNotifications, Theme = model.Theme, Id = userId };

                unitOfWork.UserSettings.Update(settings);
                unitOfWork.UserProfiles.Update(profile);

                return GenericServiceResult.Success;
            }
            catch
            {
                return GenericServiceResult.Error;
            }
        }
    }
}
