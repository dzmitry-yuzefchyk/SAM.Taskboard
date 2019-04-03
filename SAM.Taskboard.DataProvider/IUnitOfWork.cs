using SAM.Taskboard.DataProvider.Identity;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.DataProvider.Repository;
using System;

namespace SAM.Taskboard.DataProvider
{
    public interface IUnitOfWork : IDisposable
    {
        TaskboardUserManager UserManager { get; }
        TaskboardRoleManager RoleManager { get; }
        ITaskboardClientManager ClientManager { get; }
        IRepository<Board> Boards { get; }
        void Save();
    }
}
