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

                // 1. database'e yaz
                _context.Messages.Add(new Message
                {
                    SenderUserId = sender.Id,
                    ReceiverUserId = receiver.Id,
                    Msg = message,
                    SentAt = DateTime.Now
                });
                await _context.SaveChangesAsync();

                // 2. kullanıcı online mı
                if (receiverConn != null)
                {
                    await Clients.Client(receiverConn.ConnectionId)
                        .SendAsync("ReceivePrivateMessage", sender.Username, message, time);
                }
                // 3. GÖNDERİCİYE DE MESAJ GÖNDER (kendine attığında tek, farklıya attığında iki tarafta da gözüksün)
                if (senderConn != null && (receiverConn == null || receiverConn.ConnectionId != senderConn.ConnectionId))
                {
                    await Clients.Client(senderConn.ConnectionId)
                        .SendAsync("ReceivePrivateMessage", sender.Username, message, time);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR] SendPrivateMessage sırasında hata oluştu: " + ex.Message);
            }
        }

        public async Task SendGroupMessage(int groupId, int senderUserId, string message)
        {
            var sender = _context.Users.FirstOrDefault(u => u.Id == senderUserId);
            var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);

            if (sender == null || group == null)
                return;

            _context.GroupMessages.Add(new GroupMessage
            {
                SenderUserId = sender.Id,
                GroupId = group.Id,
                Msg = message,
                SentAt = DateTime.Now
            });
            await _context.SaveChangesAsync();

            var time = DateTime.Now.ToString("HH:mm");

            var members = _context.GroupMembers
                .Where(ug => ug.GroupId == group.Id)
                .Select(ug => ug.User.Username)
                .ToList();

            foreach (var memberUsername in members)
            {
                var connected = ConnectedUserHandler.GetUserByUsername(memberUsername);
                if (connected != null)
                {
                    await Clients.Client(connected.ConnectionId)
                        .SendAsync("ReceiveGroupMessage", sender.Username, message, group.Id, time);
                }
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
