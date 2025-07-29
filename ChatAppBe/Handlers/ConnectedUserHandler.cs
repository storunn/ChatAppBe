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
                return _connectedUsers.ToList();
            }
        }

        public static string? GetUsername(string connectionId)
        {
            var user = _connectedUsers.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (user != null)
                return user.Username;
            else
                return null;
        }

    }
}
