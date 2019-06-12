using Microsoft.AspNet.Identity;
using SAM.Taskboard.DataProvider.Models;

namespace SAM.Taskboard.DataProvider.Identity
{
    public class TaskboardUserManager : UserManager<User>
    {
        public TaskboardUserManager(IUserStore<User> store)
            : base(store)
        {
            this.UserTokenProvider = new TotpSecurityStampBasedTokenProvider<User, string>();
        }
    }
}
