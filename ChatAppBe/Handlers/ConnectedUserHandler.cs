using ChatAppBe.Data.Models;

namespace ChatAppBe.Handlers
{
    public static class ConnectedUserHandler
    {
        private static readonly List<ConnectedUser> _connectedUsers = new();
        private static readonly object _lock = new();

        public static void AddUser(ConnectedUser user)
        {
            lock (_lock)
            {
                // Aynı kullanıcıdan varsa eski bağlantıyı sil
                var existing = _connectedUsers.FirstOrDefault(u => u.Username == user.Username);
                if (existing != null)
                    _connectedUsers.Remove(existing);

                _connectedUsers.Add(user);
            }
        }

        public static void RemoveUser(string connectionId)
        {
            lock (_lock)
            {
                var user = _connectedUsers.FirstOrDefault(u => u.ConnectionId == connectionId);
                if (user != null)
                    _connectedUsers.Remove(user);
            }
        }

        public static List<ConnectedUser> GetAllUsers()
        {
            lock (_lock)
            {
                return _connectedUsers.ToList(); // Kopyasını döndürür.
            }
        }

        public static string? GetUsername(string connectionId)
        {
            lock (_lock)
            {
                return _connectedUsers.FirstOrDefault(x => x.ConnectionId == connectionId)?.Username;
            }
        }

        public static ConnectedUser? GetUserByUsername(string username)
        {
            lock (_lock)
            {
                return _connectedUsers.FirstOrDefault(x => x.Username == username);
            }
        }
    }
}
