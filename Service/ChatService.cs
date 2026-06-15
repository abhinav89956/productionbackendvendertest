using VenderTest.DTOs;
using VenderTest.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VenderTest.Service
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _repository;

        public ChatService(IChatRepository repository)
        {
            _repository = repository;
        }

        public async Task UpdateDeliveredStatus(int receiverId)
        {
            await _repository.UpdateDeliveredStatus(receiverId);
        }

        public async Task UpdateSeenStatus(int receiverId, int senderId, int messageId)
        {
            await _repository.UpdateSeenStatus(receiverId, senderId, messageId);
        }
        public async Task MarkMessageDelivered(int messageId, int receiverId)
        {
            await _repository.MarkMessageDelivered(messageId, receiverId);
        }

        public async Task MarkMessageAsSeen(int messageId, int receiverId)
        {
            try
            {
                await _repository.MarkAsSeen(messageId, receiverId);
            }
            catch (Exception ex)
            {
                
                throw new Exception($"Error marking message {messageId} as seen for user {receiverId}.", ex);
            }
        }

        public async Task<ContactDto> AddContact(int userId, int contactUserId)
        {
            try
            {
                return await _repository.AddContact(userId, contactUserId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding contact {contactUserId} for user {userId}.", ex);
            }
        }

        public async Task<IEnumerable<ContactDto>> GetContacts(int userId)
        {
            try
            {
                return await _repository.GetContacts(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving contacts for user {userId}.", ex);
            }
        }

        public async Task<MessageResultDto> SendMessage(ChatMessageDto model)
        {
            try
            {
                return await _repository.SendMessage(model);
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending message.", ex);
            }
        }

        public async Task<IEnumerable<BlockedUserDto>> GetBlockedUsersAsync(int userId)
        {
            try
            {
                return await _repository.GetBlockedUsersAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving blocked users for user {userId}.", ex);
            }
        }

        public async Task<IEnumerable<MessageResultDto>> GetMessages(int user1, int user2)
        {
            try
            {
                return await _repository.GetMessages(user1, user2);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving messages between user {user1} and user {user2}.", ex);
            }
        }

        public async Task<bool> BlockUser(int userId, int blockedUserId)
        {
            try
            {
                return await _repository.BlockUser(userId, blockedUserId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error blocking user {blockedUserId} for user {userId}.", ex);
            }
        }

        public async Task<bool> UpdateUserStatus(int userId, string status)
        {
            try
            {
                return await _repository.UpdateUserStatus(userId, status);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating status for user {userId}.", ex);
            }
        }

        public async Task UpdateLastSeen(int userId)
        {
            try
            {
                await _repository.UpdateLastSeen(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating last seen for user {userId}.", ex);
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetNotifications(int userId)
        {
            try
            {
                return await _repository.GetNotifications(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving notifications for user {userId}.", ex);
            }
        }
    }
}