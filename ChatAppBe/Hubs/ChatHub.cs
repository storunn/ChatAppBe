using ChatAppBe.Data.Models;
using ChatAppBe.Handlers;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppBe.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Tüm client'lara mesajı gönder
            await Clients.Others.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext().Request.Query["username"]; // örnek kullanım

            ConnectedUserHandler.AddUser(new ConnectedUser
            {
                ConnectionId = Context.ConnectionId,
                Username = username
            });

            await Clients.All.SendAsync("UserListUpdated", ConnectedUserHandler.GetAllUsers());

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUserHandler.RemoveUser(Context.ConnectionId);

            await Clients.All.SendAsync("UserListUpdated", ConnectedUserHandler.GetAllUsers());

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<List<ConnectedUser>> GetConnectedUsers()
        {
            return ConnectedUserHandler.GetAllUsers();
        }
    }
}
