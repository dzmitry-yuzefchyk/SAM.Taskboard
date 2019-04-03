using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SAM.Taskboard.DataProvider.Models;

namespace SAM.Taskboard.DataProvider.Identity
{
    public class TaskboardRoleManager : RoleManager<Role>
    {
        public TaskboardRoleManager(RoleStore<Role> store)
            : base(store)
        { }
    }
}
