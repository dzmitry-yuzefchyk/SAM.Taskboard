using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAM.Taskboard.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Register(string userName, string email, string password)
        {
            try
            {
                User user = await unitOfWork.UserManager.FindByEmailAsync(email);

                if (user == null)
                {
                    user = new User { UserName = userName, Email = email, EmailConfirmed = false };
                    IdentityResult result = await unitOfWork.UserManager.CreateAsync(user, password);

                    UserProfile profile = new UserProfile { Id = user.Id, Name = userName };
                    UserSettings settings = new UserSettings { Id = user.Id, EmailNotification = false };
                    unitOfWork.ClientManager.Create(profile, settings);

                    if (result.Succeeded)
                    {
                        return Result.success;
                    }

                    else
                    {
                        return Result.error;
                    }
                }
            }

            catch
            {
                return Result.error;
            }

            return Result.userAlreadyExists;
        }

        public Result SendConfirmationEmail(string userName, string email, string confirmationLink)
        {
            try
            {
                MailMessage m = new MailMessage(
                new MailAddress("DTaskboard@gmail.com", "Registration"),
                new MailAddress(email));
                m.Subject = "Email confirmation";
                m.Body = $"Dear {userName}." +
                         $"<br/ > Thank you for your registration, please click on below link to complete your registration: " +
                         $"<a href =\"{confirmationLink}\"";
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Credentials = new System.Net.NetworkCredential("DTaskboard@gmail.com", "greener0511");
                smtp.EnableSsl = true;
                smtp.Send(m);
            }

            catch
            {
                return Result.emailNotSent;
            }

            return Result.success;
        }

        public string GetUserId(string email)
        {
            return unitOfWork.UserManager.FindByEmail(email).Id;
        }

        public void LogIn(UserData userData, IAuthenticationManager authenticationManager, string providerKey = null, bool isPersistent = false)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, userData.UserId));
            claims.Add(new Claim(ClaimTypes.Name, userData.Name));

            claims.Add(new Claim("userState", userData.ToString()));

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            authenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, identity);
        }

        public void LogOut(IAuthenticationManager authenticationManager)
        {
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
