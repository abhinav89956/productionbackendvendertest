using VenderTest.DTOs;

namespace VenderTest.Repository
{
    public interface IChatRepository
    {
        Task UpdateDeliveredStatus(int receiverId);

        Task UpdateSeenStatus(int receiverId, int senderId, int messageId);

        Task MarkMessageDelivered(int messageId, int receiverId);

        Task UpdateLastSeen(int userId);

        Task MarkAsSeen(int messageId, int receiverId);

        Task<IEnumerable<BlockedUserDto>> GetBlockedUsersAsync(int userId);

        Task<IEnumerable<ContactDto>> GetContacts(int userId);

        Task<MessageResultDto> SendMessage(ChatMessageDto model);

        Task<IEnumerable<MessageResultDto>> GetMessages(int user1, int user2);

        Task<bool> BlockUser(int userId, int blockedUserId);

        Task<IEnumerable<NotificationDto>> GetNotifications(int userId);
    }
}