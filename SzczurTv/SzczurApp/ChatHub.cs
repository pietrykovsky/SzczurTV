
using Microsoft.AspNetCore.SignalR;

namespace SzczurApp;

public class ChatHub : Hub
{
    public async Task SendMessage(string room, string user, string message)
    {
        await Clients.Group(room).SendAsync("ReceiveMessage", user, message);
    }

    public async Task JoinRoom(string room)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
    }

    public async Task LeaveRoom(string room)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
    }
}
