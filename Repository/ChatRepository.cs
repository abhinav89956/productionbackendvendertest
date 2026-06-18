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
                await _genericRepository.ExecuteAsync(
                    @"SELECT * FROM ""_vender"".""sp_markmessageasseen""(@p_receiverid)",
                    new { p_receiverid = receiverId }
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
                // ⚠️ sp_addcontact DB mein nahi hai — direct insert karo
                throw new NotImplementedException("sp_addcontact function DB mein nahi hai.");
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
                var result = await _genericRepository.QueryAsync<BlockedUserDto>(
                    @"SELECT * FROM ""_vender"".""sp_getblockedusers""(@p_userid)",
                    new { p_userid = userId }
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
                var result = await _genericRepository.QueryAsync<ContactDto>(
                    @"SELECT * FROM ""_vender"".""sp_getcontacts""(@p_userid)",
                    new { p_userid = userId }
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
                return await _genericRepository.QueryFirstOrDefaultAsync<MessageResultDto>(
                    @"SELECT * FROM ""_vender"".""sp_sendmessage""(
                    @p_senderid,
                    @p_receiverid,
                    @p_messagetext,
                    @p_messagetype)",
                    new
                    {
                        p_senderid = model.SenderId,
                        p_receiverid = model.ReceiverId,
                        p_messagetext = model.MessageText,
                        p_messagetype = model.MessageType
                    }
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
                await _genericRepository.ExecuteAsync(
                    @"SELECT * FROM ""_vender"".""sp_updatedeliveredstatus""(@p_receiverid)",
                    new { p_receiverid = receiverId }
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
                var result = await _genericRepository.QueryAsync<MessageResultDto>(
                    @"SELECT * FROM ""_vender"".""sp_getchatmessages""(
                    @p_user1,
                    @p_user2)",
                    new
                    {
                        p_user1 = user1,
                        p_user2 = user2
                    }
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
                var result = await _genericRepository.QueryFirstOrDefaultAsync<BlockResultDto>(
                    @"SELECT * FROM ""_vender"".""sp_blockuser""(
                    @p_userid,
                    @p_blockeduserid)",
                    new
                    {
                        p_userid = userId,
                        p_blockeduserid = blockedUserId
                    }
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
                // ⚠️ sp_updateuserstatus DB mein nahi hai — LastSeen update use karo
                await _genericRepository.ExecuteAsync(
                    @"SELECT * FROM ""_vender"".""sp_updateuserlastseen""(@p_userid)",
                    new { p_userid = userId }
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
                var result = await _genericRepository.QueryAsync<NotificationDto>(
                    @"SELECT * FROM ""_vender"".""sp_getnotifications""(@p_userid)",
                    new { p_userid = userId }
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
                await _genericRepository.ExecuteAsync(
                    @"SELECT * FROM ""_vender"".""sp_updatedeliveredstatus""(@p_receiverid)",
                    new { p_receiverid = receiverId }
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
                await _genericRepository.ExecuteAsync(
                    @"SELECT * FROM ""_vender"".""sp_updateseenstatus""(
                    @p_receiverid,
                    @p_messageid)",
                    new
                    {
                        p_receiverid = receiverId,
                        p_messageid = messageId
                    }
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
                await _genericRepository.ExecuteAsync(
                    @"SELECT * FROM ""_vender"".""sp_updateuserlastseen""(@p_userid)",
                    new { p_userid = userId }
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