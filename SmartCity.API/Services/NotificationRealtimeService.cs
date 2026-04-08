using Microsoft.AspNetCore.SignalR;
using SmartCity.API.Hubs;

namespace SmartCity.API.Services
{
    public class NotificationRealtimeService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationRealtimeService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendAsync(Guid? userId, object notification)
        {
            Console.WriteLine("🔥 REALTIME TRIGGERED");

            if (userId == null)
            {
                Console.WriteLine("👉 Sending to Admins");

                await _hubContext.Clients.Group("Admins")
                    .SendAsync("ReceiveNotification", notification);
            }
            else
            {
                Console.WriteLine($"👉 Sending to User: {userId}");

                await _hubContext.Clients.Group(userId.ToString())
                    .SendAsync("ReceiveNotification", notification);
            }
        }
    }
}