using SAM.Taskboard.Logic.Hubs;
using System.Linq;

namespace SAM.Taskboard.Logic.Utility
{
    public class Notifyer
    {
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
                    NotifyUserOffline();
                }
            }
            catch
            {
                NotifyUserOffline();
            }
        }

        private static void NotifyUserOffline()
        {

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
