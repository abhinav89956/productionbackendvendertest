using System.Collections.Concurrent;

namespace VenderTest.Service
{
    public interface IOnlineUserService
    {
        void AddConnection(int userId, string connectionId);

        void RemoveConnection(int userId, string connectionId);

        bool IsOnline(int userId);

        ConcurrentDictionary<int, ConcurrentDictionary<string, byte>> GetAll();
    }
}