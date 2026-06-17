using Dapper;
using Npgsql;
using System.Data;
using VenderTest.DTOs;

namespace VenderTest.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly IGenericRepository _genericRepository;

        public ChatRepository(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task MarkAsSeen(int messageId, int receiverId)
        {
            try
            {
                var parameters = new
                {
                    ReceiverId = receiverId   // SP_MarkMessageAsSeen only takes ReceiverId
                };

                await _genericRepository.ExecuteAsync(
                    "_vender.SP_MarkMessageAsSeen",
                    parameters
                );
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in MarkAsSeen: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to mark message as seen: {ex.Message}", ex);
            }
        }

        public async Task<ContactDto> AddContact(int userId, int contactUserId)
        {
            try
            {
                var parameters = new
                {
                    UserId = userId,
                    ContactUserId = contactUserId
                };

                return await _genericRepository.QueryFirstOrDefaultAsync<ContactDto>(
                    "_vender.SP_AddContact",
                    parameters
                );
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in AddContact: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add contact: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<BlockedUserDto>> GetBlockedUsersAsync(int userId)
        {
            try
            {
                var parameters = new { UserId = userId };

                var result = await _genericRepository.QueryAsync<BlockedUserDto>(
                    "_vender.SP_GetBlockedUsers",
                    parameters
                );

                return result ?? Enumerable.Empty<BlockedUserDto>();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in GetBlockedUsersAsync: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get blocked users: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ContactDto>> GetContacts(int userId)
        {
            try
            {
                var parameters = new { UserId = userId };

                var result = await _genericRepository.QueryAsync<ContactDto>(
                    "_vender.SP_GetContacts",
                    parameters
                );

                return result ?? Enumerable.Empty<ContactDto>();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in GetContacts: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get contacts: {ex.Message}", ex);
            }
        }

        public async Task<MessageResultDto> SendMessage(ChatMessageDto model)
        {
            try
            {
                var parameters = new
                {
                    SenderId = model.SenderId,
                    ReceiverId = model.ReceiverId,
                    MessageText = model.MessageText,
                    MessageType = model.MessageType
                };

                return await _genericRepository.QueryFirstOrDefaultAsync<MessageResultDto>(
                    "_vender.SP_SendMessage",
                    parameters
                );
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in SendMessage: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send message: {ex.Message}", ex);
            }
        }

        public async Task MarkMessageDelivered(int messageId, int receiverId)
        {
            try
            {
                // Maps to SP_UpdateDeliveredStatus — no SP_MarkMessageDelivered exists
                var parameters = new { ReceiverId = receiverId };

                await _genericRepository.ExecuteAsync(
                    "_vender.SP_UpdateDeliveredStatus",
                    parameters
                );
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in MarkMessageDelivered: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to mark message as delivered: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<MessageResultDto>> GetMessages(int user1, int user2)
        {
            try
            {
                var parameters = new
                {
                    User1 = user1,
                    User2 = user2
                };

                var result = await _genericRepository.QueryAsync<MessageResultDto>(
                    "_vender.SP_GetChatMessages",
                    parameters
                );

                return result ?? Enumerable.Empty<MessageResultDto>();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in GetMessages: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get messages: {ex.Message}", ex);
            }
        }

        public async Task<bool> BlockUser(int userId, int blockedUserId)
        {
            try
            {
                var parameters = new
                {
                    UserId = userId,
                    BlockedUserId = blockedUserId
                };

                var result = await _genericRepository.QueryFirstOrDefaultAsync<BlockResultDto>(
                    "_vender.SP_BlockUser",
                    parameters
                );

                return result?.IsBlocked ?? false;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in BlockUser: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to block/unblock user: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateUserStatus(int userId, string status)
        {
            try
            {
                // SP_UpdateUserStatus does not exist in the DB — using UserStatus table update approach
                // If you add a dedicated function later, swap the SQL here
                var parameters = new
                {
                    UserId = userId,
                    Status = status
                };

                await _genericRepository.ExecuteAsync(
                    "_vender.SP_UpdateUserStatus",
                    parameters
                );

                return true;
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in UpdateUserStatus: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update user status: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetNotifications(int userId)
        {
            try
            {
                var parameters = new { UserId = userId };

                var result = await _genericRepository.QueryAsync<NotificationDto>(
                    "_vender.SP_GetNotifications",
                    parameters
                );

                return result ?? Enumerable.Empty<NotificationDto>();
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in GetNotifications: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get notifications: {ex.Message}", ex);
            }
        }

        public async Task UpdateDeliveredStatus(int receiverId)
        {
            try
            {
                var parameters = new { ReceiverId = receiverId };

                await _genericRepository.ExecuteAsync(
                    "_vender.SP_UpdateDeliveredStatus",
                    parameters
                );
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in UpdateDeliveredStatus: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update delivered status: {ex.Message}", ex);
            }
        }

        public async Task UpdateSeenStatus(int messageId, int receiverId, int senderId)
        {
            try
            {
                // SP_UpdateSeenStatus takes (p_ReceiverId, p_MessageId) — note order
                var parameters = new
                {
                    ReceiverId = receiverId,
                    MessageId = messageId
                };

                await _genericRepository.ExecuteAsync(
                    "_vender.SP_UpdateSeenStatus",
                    parameters
                );
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in UpdateSeenStatus: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update seen status: {ex.Message}", ex);
            }
        }

        public async Task UpdateLastSeen(int userId)
        {
            try
            {
                var parameters = new { UserId = userId };

                await _genericRepository.ExecuteAsync(
                    "_vender.SP_UpdateUserLastSeen",
                    parameters
                );
            }
            catch (NpgsqlException ex)
            {
                throw new Exception($"Database error in UpdateLastSeen: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update last seen: {ex.Message}", ex);
            }
        }
    }
}
