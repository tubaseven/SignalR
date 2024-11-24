using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR(); 
builder.Services.AddHostedService<NotificationBackgroundService>(); 

var app = builder.Build();
app.UseStaticFiles();
app.MapHub<NotificationHub>("/notificationHub"); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Herkese aynı anda mesaj göndermek için bir API oluşturuyoruz.
app.MapPost("/api/send-message", async (IHubContext<NotificationHub> hubContext, string message) =>
{
    await hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    return Results.Ok(new { Status = "Message Sent", Message = message });
});

// Bir API oluşturup bağlanan istemcilerin ConnectionId değerlerini döndürüyoruz.

app.MapGet("/api/get-connection-id", (HttpContext context) =>
{
    var connectionId = context.Request.Headers["ConnectionId"].ToString();
    if (string.IsNullOrEmpty(connectionId))
    {
        return Results.BadRequest(new { Error = "ConnectionId header is missing" });
    }

    return Results.Ok(new { ConnectionId = connectionId });
});


app.UseHttpsRedirection();

app.Run();

