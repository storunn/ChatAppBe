using ChatAppBe.Data;
using ChatAppBe.Data.DbContexts;
using ChatAppBe.Data.Entities;
using ChatAppBe.Data.Models;
using ChatAppBe.Handlers;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppBe.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            var time = DateTime.Now.ToString("HH:mm");
            await Clients.All.SendAsync("ReceiveMessage", user, message, time);
        }

        public async Task SendPrivateMessage(string receiverUsername, string message)
        {
            try
            {
                var senderConnId = Context.ConnectionId;
                var senderUsername = ConnectedUserHandler.GetUsername(senderConnId);
                Console.WriteLine($"[DEBUG] senderConnId: {senderConnId}, senderUsername: {senderUsername}");

                var senderConn = ConnectedUserHandler.GetUserByUsername(senderUsername);
                var receiverConn = ConnectedUserHandler.GetUserByUsername(receiverUsername);
                var sender = _context.Users.FirstOrDefault(u => u.Username == senderUsername);
                var receiver = _context.Users.FirstOrDefault(u => u.Username == receiverUsername);
                if (sender == null || receiver == null)
                {
                    Console.WriteLine($"[WARN] Kullanıcı bulunamadı: sender={sender?.Username}, receiver={receiver?.Username}");
                    return;
                }

                var time = DateTime.Now.ToString("HH:mm");

                Console.WriteLine($"[INFO] Özel mesaj: {sender.Username} → {receiver.Username}: {message}");

                _context.Messages.Add(new Message
                {
                    SenderUserId = sender.Id,            
                    ReceiverUserId = receiver.Id,        
                    Msg = message,
                    SentAt = DateTime.Now
                });
                await _context.SaveChangesAsync();
                
                await Clients.Client(receiverConn.ConnectionId)
                    .SendAsync("ReceivePrivateMessage", sender.Username, message, time);

                await Clients.Client(senderConn.ConnectionId)
                    .SendAsync("ReceivePrivateMessage", $"Sen → {receiver.Username}", message, time);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] SendPrivateMessage sırasında hata oluştu: " + ex.Message);
            }
        }

        

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext == null)
            {
                Console.WriteLine("Bağlantı kuruldu ama HttpContext yok.");
                return;
            }

            var username = httpContext.Request.Query["username"];
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Kullanıcı adı boş geldi!");
                return;
            }

            ConnectedUserHandler.AddUser(new ConnectedUser
            {
                ConnectionId = Context.ConnectionId,
                Username = username
            });

            await Clients.All.SendAsync("UserListUpdated", ConnectedUserHandler.GetAllUsers());
            await Clients.Others.SendAsync("UserJoined", username);

            await base.OnConnectedAsync();
        }



        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = ConnectedUserHandler.GetUsername(Context.ConnectionId);
            ConnectedUserHandler.RemoveUser(Context.ConnectionId);

            await Clients.All.SendAsync("UserListUpdated", ConnectedUserHandler.GetAllUsers());
            await Clients.Others.SendAsync("UserLeft", username);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
