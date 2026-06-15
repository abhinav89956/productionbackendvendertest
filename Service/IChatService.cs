using VenderTest.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VenderTest.Service
{
    public interface IChatService
    {

        Task<ContactDto> AddContact(int userId, int contactUserId);
        Task UpdateDeliveredStatus(int receiverId);
        Task UpdateSeenStatus(int receiverId, int senderId, int messageId);
        Task MarkMessageAsSeen(int messageId, int receiverId);
        Task MarkMessageDelivered(int messageId, int receiverId);
        Task<IEnumerable<BlockedUserDto>> GetBlockedUsersAsync(int userId);

        Task<IEnumerable<ContactDto>> GetContacts(int userId);
        Task<MessageResultDto> SendMessage(ChatMessageDto model);

        Task<IEnumerable<MessageResultDto>> GetMessages(int user1, int user2);
 
        Task<bool> BlockUser(int userId, int blockedUserId);

       
        Task<bool> UpdateUserStatus(int userId, string status);
        Task<IEnumerable<NotificationDto>> GetNotifications(int userId);

        Task UpdateLastSeen(int userId);
    }
}