using SAM.Taskboard.Logic.Utility;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAM.Taskboard.Logic.Services
{
    public interface IUserService : IDisposable
    {
        Task<UserServiceResult> Register(string userName, string email, string password);
        ClaimsIdentity PasswordEmailSignIn(string email, string password);
        UserServiceResult IsUserEmailConfirmed(string email);
        string GetUserIdByEmail(string email);
        string GetUserNameByEmail(string email);
        UserServiceResult SendConfirmationEmail(string userName, string email, string confirmationLink);
        UserServiceResult ConfirmEmail(string userId, string token);
        string GenerateEmailConfirmationToken(string userId);
    }
}
