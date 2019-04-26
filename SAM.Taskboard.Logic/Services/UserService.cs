using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Utility;
using System.IO;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SAM.Taskboard.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly string serviceEmail;
        private readonly string servicePassword;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            using (StreamReader stream = File.OpenText($"{HttpRuntime.AppDomainAppPath}\\..\\SAM.Taskboard.Logic\\settings.json"))
            {
                JObject settings = (JObject)JToken.ReadFrom(new JsonTextReader(stream));
                this.serviceEmail = (string)settings["email"];
                this.servicePassword = (string)settings["password"];
            }
        }

        public async Task<UserServiceResult> Register(string userName, string email, string password)
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
                        return UserServiceResult.success;
                    }

                    else
                    {
                        return UserServiceResult.error;
                    }
                }
            }

            catch
            {
                return UserServiceResult.error;
            }

            return UserServiceResult.userAlreadyExists;
        }

        public UserServiceResult SendConfirmationEmail(string userName, string email, string confirmationLink)
        {
            User user = unitOfWork.UserManager.FindByEmail(email);
            if (user.EmailConfirmed)
            {
                return UserServiceResult.emailAlreadyConfirmed;
            }

            try
            {
                MailMessage m = new MailMessage(
                new MailAddress(serviceEmail, "Registration"),
                new MailAddress(email));
                m.Subject = "Email confirmation";
                m.Body = $"Dear {userName}." +
                        $"<br/ >Thank you for your registration, please click on below link to complete your registration: " +
                        $"<a href=\"{confirmationLink}\">click me</a>";
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Credentials = new System.Net.NetworkCredential(serviceEmail, servicePassword);
                smtp.EnableSsl = true;
                smtp.Send(m);
            }

            catch
            {
                return UserServiceResult.emailNotSent;
            }

            return UserServiceResult.success;
        }

        public string GenerateEmailConfirmationToken(string userId)
        {
            return unitOfWork.UserManager.GenerateEmailConfirmationToken(userId);
        }

        public UserServiceResult ConfirmEmail(string userId, string token)
        {
            User user = unitOfWork.UserManager.FindById(userId);

            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    return UserServiceResult.emailAlreadyConfirmed;
                }

                unitOfWork.UserManager.ConfirmEmail(userId, token);
                return UserServiceResult.success;
            }

            return UserServiceResult.userNotExist;
        }

        public string GetUserIdByEmail(string email)
        {
            string id;

            try
            {
                id = unitOfWork.UserManager.FindByEmail(email).Id;
            }

            catch
            {
                id = null;
            }

            return id;
        }

        public string GetUserNameByEmail(string email)
        {
            string id = GetUserIdByEmail(email);

            if (id == null)
            {
                return null;
            }

            return unitOfWork.ClientManager.GetProfile(id).Name;
        }

        public ClaimsIdentity LogIn(string userName, string password)
        {
            ClaimsIdentity claim = null;

            User user = unitOfWork.UserManager.Find(userName, password);
            if (user != null)
            {
                claim = unitOfWork.UserManager.CreateIdentity(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }

        public UserServiceResult IsUserEmailConfirmed(string email)
        {
            User user = unitOfWork.UserManager.FindByEmail(email);

            if (!user.EmailConfirmed)
            {
                return UserServiceResult.emailNotConfirmed;
            }

            return UserServiceResult.emailAlreadyConfirmed;
        }

        public ClaimsIdentity PasswordEmailSignIn(string email, string password)
        {
            User user = unitOfWork.UserManager.FindByEmail(email);
            return LogIn(user.UserName, password);
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
