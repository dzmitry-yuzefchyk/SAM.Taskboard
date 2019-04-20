using Microsoft.Owin.Security;
using SAM.Taskboard.Logic.Utility;
using System;
using System.Threading.Tasks;

namespace SAM.Taskboard.Logic.Services
{
    public interface IUserService : IDisposable
    {
        Task<Result> Register(string userName, string email, string password);
        void LogIn(UserData userData, IAuthenticationManager authenticationManager, string providerKey = null, bool isPersistent = false);
        void LogOut(IAuthenticationManager authenticationManager);
        string GetUserId(string email);
        Result SendConfirmationEmail(string userName, string email, string confirmationLink);
    }
}
