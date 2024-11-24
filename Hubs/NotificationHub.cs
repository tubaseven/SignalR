using Microsoft.AspNetCore.SignalR;
public class NotificationHub : Hub
{
    // İstemciden mesaj alır ve tüm istemcilere iletir
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    // Bir istemciden başka bir istemciye özel mesaj gönderebilirsiniz
    public async Task SendToClient(string connectionId, string message)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
    }

    // Sunucuya bağlanan istemcinin kimliğini döndürebilirsiniz
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
}