using Microsoft.AspNetCore.SignalR;
using SmartCity.API.Hubs;
using SmartCity.Application.Interfaces;

namespace SmartCity.API.Services
{
    public class NotificationRealtimeService : INotificationRealtimeService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationRealtimeService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendAsync(Guid? userId, string message)
        {
            Console.WriteLine("REALTIME TRIGGERED");

            if (userId == null)
            {
                Console.WriteLine("Sending to Admins");

                await _hubContext.Clients.Group("Admins")
                    .SendAsync("ReceiveNotification", message);
            }
            else
            {
                Console.WriteLine($"Sending to User: {userId}");

                await _hubContext.Clients.Group(userId.ToString())
                    .SendAsync("ReceiveNotification", message);
            }
        }
    }
}