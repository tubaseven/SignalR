using Microsoft.AspNetCore.SignalR;
public class NotificationBackgroundService : BackgroundService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationBackgroundService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Bağlı tüm istemcilere mesaj gönder
            await _hubContext.Clients.All.SendAsync(
                "ReceiveNotification", 
                $"Current Time: {DateTime.UtcNow:HH:mm:ss.fff}");

            await Task.Delay(100, stoppingToken); 
        }
    }
}