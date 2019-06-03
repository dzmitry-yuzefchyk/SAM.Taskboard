using SAM.Taskboard.DataProvider;

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
    }
}
