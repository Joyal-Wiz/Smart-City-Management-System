using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SmartCity.API.Hubs
{
    public class NotificationHub : Hub
    {
        // 🔥 When client connects
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;

            if (user != null)
            {
                // 👉 Get Role
                var role = user.FindFirst(ClaimTypes.Role)?.Value;

                // 👉 Get UserId
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // 🔥 ADMIN → GROUP
                if (role == "Admin")
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
                }

                // 🔥 USER / WORKER → USER GROUP
                if (!string.IsNullOrEmpty(userId))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                }
            }

            await base.OnConnectedAsync();
        }

        // 🔔 SEND TO SPECIFIC USER
        public async Task SendToUser(string userId, object message)
        {
            await Clients.Group(userId).SendAsync("ReceiveNotification", message);
        }

        // 🔔 SEND TO ADMINS
        public async Task SendToAdmins(object message)
        {
            await Clients.Group("Admins").SendAsync("ReceiveNotification", message);
        }
    }
}