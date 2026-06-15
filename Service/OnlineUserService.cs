using System.Collections.Concurrent;

namespace VenderTest.Service
{
    using System.Collections.Concurrent;

    namespace VenderTest.Service
    {
        public class OnlineUserService : IOnlineUserService
        {
            private readonly ConcurrentDictionary<int, ConcurrentDictionary<string, byte>> _onlineUsers
                = new();

            public void AddConnection(int userId, string connectionId)
            {
                var connections = _onlineUsers.GetOrAdd(userId, _ => new ConcurrentDictionary<string, byte>());

                connections.TryAdd(connectionId, 0);
            }

            public void RemoveConnection(int userId, string connectionId)
            {
                if (_onlineUsers.TryGetValue(userId, out var connections))
                {
                    connections.TryRemove(connectionId, out _);

                    if (connections.IsEmpty)
                    {
                        _onlineUsers.TryRemove(userId, out _);
                    }
                }
            }

            public bool IsOnline(int userId)
            {
                return _onlineUsers.ContainsKey(userId);
            }

            public ConcurrentDictionary<int, ConcurrentDictionary<string, byte>> GetAll()
            {
                return _onlineUsers;
            }
        }
    }
}