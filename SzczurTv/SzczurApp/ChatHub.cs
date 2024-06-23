
using Microsoft.AspNetCore.SignalR;

namespace SzczurApp;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.Group(user).SendAsync("ReceiveMessage", user, message);
    }

    public async Task JoinRoom(string username)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, username);
    }

    public async Task LeaveRoom(string username)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, username);
    }
}
