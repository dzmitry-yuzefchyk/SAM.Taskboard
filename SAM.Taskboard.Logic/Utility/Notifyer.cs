using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAM.Taskboard.DataProvider;
using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Logic.Hubs;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SAM.Taskboard.Logic.Utility
{
    public static class Notifyer
    {
        static readonly string serviceEmail;
        static readonly string servicePassword;

        public static void Notify(NotificationMessage message)
        {
            try
            {
                var user = NotificationHub.Users.FirstOrDefault(u => u.IdentityUserId == message.SendTo);

                if (user != null)
                {
                    NotifyUserOnline(message);
                }

                else
                {
                    NotifyUserOffline(message);
                }
            }
            catch
            {
                NotifyUserOffline(message);
            }
        }

        private static void NotifyUserOffline(NotificationMessage message)
        {
            IUnitOfWork unitOfWork = new UnitOfWork();

            string serviceEmail = Environment.GetEnvironmentVariable("email");
            string servicePassword = Environment.GetEnvironmentVariable("password");

            if (serviceEmail == null && servicePassword == null)
            {
                using (StreamReader stream = File.OpenText($"{HttpRuntime.AppDomainAppPath}\\..\\SAM.Taskboard.Logic\\settings.json"))
                {
                    JObject settings = (JObject)JToken.ReadFrom(new JsonTextReader(stream));
                    serviceEmail = (string)settings["email"];
                    servicePassword = (string)settings["password"];
                }
            }

            User user = unitOfWork.Users.GetFirstOrDefaultWhere(u => u.Id == message.SendTo);
            if (!user.EmailConfirmed)
            {
                return;
            }

            UserSettings userSettings = unitOfWork.ClientManager.GetSettings(message.SendTo);
            if (!userSettings.EmailNotification)
            {
                return;
            }

            UserProfile userProfile = unitOfWork.ClientManager.GetProfile(message.SendTo);

            try
            {
                MailMessage m = new MailMessage(
                new MailAddress(serviceEmail, "DTaskboard notification"),
                new MailAddress(user.Email));
                m.Subject = message.Title;
                m.Body = $"Dear {userProfile.Name}." +
                        $"<br/ >{message.Message}";
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Credentials = new System.Net.NetworkCredential(serviceEmail, servicePassword);
                smtp.EnableSsl = true;
                smtp.Send(m);
            }

            catch
            {
                return;
            }
        }

        private static void NotifyUserOnline(NotificationMessage message)
        {
            var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            var user = NotificationHub.Users.FirstOrDefault(u => u.IdentityUserId == message.SendTo);
            string userConnectionId = user.ConnectionId;

            if (message.SendTo == message.Initiator)
            {
                return;
            }

            if (message.AdditionalSendTo != null)
            {
                Notify(message.AdditionalSendToMessage);
            }

            context.Clients.Client(userConnectionId).notify(message);
        }
    }
}
