using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SAM.Taskboard.Logic.Hubs
{
    public class NotificationHub : Hub
    {
        public static List<SignalrUser> Users = new List<SignalrUser>();

        public override Task OnConnected()
        {
            var id = Context.ConnectionId;

            if (!Users.Any(x => x.ConnectionId == id))
            {
                Users.Add(new SignalrUser { ConnectionId = id, IdentityUserId = Context.User.Identity.GetUserId() });
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var id = Context.ConnectionId;

            SignalrUser user = Users.Find(u => u.IdentityUserId == Context.User.Identity.GetUserId());
            Users.Remove(user);

            return base.OnDisconnected(stopCalled);
        }
    }

    public class SignalrUser
    {
        public string ConnectionId { get; set; }
        public string IdentityUserId { get; set; }
    }
}
