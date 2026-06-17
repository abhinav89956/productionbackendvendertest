using Dapper;
using Npgsql;
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
                    @"SELECT * FROM ""_vender"".sp_markmessageasseen(@ReceiverId)",
                    new { ReceiverId = receiverId });
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to mark message as seen: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<BlockedUserDto>> GetBlockedUsersAsync(int userId)
        {
            return await _genericRepository.QueryAsync<BlockedUserDto>(
                @"SELECT * FROM ""_vender"".sp_getblockedusers(@UserId)",
                new { UserId = userId });
        }

        public async Task<IEnumerable<ContactDto>> GetContacts(int userId)
        {
            return await _genericRepository.QueryAsync<ContactDto>(
                @"SELECT * FROM ""_vender"".sp_getcontacts(@UserId)",
                new { UserId = userId });
        }

        public async Task<MessageResultDto> SendMessage(ChatMessageDto model)
        {
            return await _genericRepository.QueryFirstOrDefaultAsync<MessageResultDto>(
                @"SELECT * FROM ""_vender"".sp_sendmessage(
                    @SenderId,
                    @ReceiverId,
                    @MessageText,
                    @MessageType)",
                new
                {
                    model.SenderId,
                    model.ReceiverId,
                    model.MessageText,
                    model.MessageType
                });
        }

        public async Task MarkMessageDelivered(int messageId, int receiverId)
        {
            await _genericRepository.ExecuteAsync(
                @"SELECT * FROM ""_vender"".sp_updatedeliveredstatus(@ReceiverId)",
                new { ReceiverId = receiverId });
        }

        public async Task<IEnumerable<MessageResultDto>> GetMessages(int user1, int user2)
        {
            return await _genericRepository.QueryAsync<MessageResultDto>(
                @"SELECT * FROM ""_vender"".sp_getchatmessages(@User1,@User2)",
                new
                {
                    User1 = user1,
                    User2 = user2
                });
        }

        public async Task<bool> BlockUser(int userId, int blockedUserId)
        {
            var result = await _genericRepository.QueryFirstOrDefaultAsync<BlockResultDto>(
                @"SELECT * FROM ""_vender"".sp_toggleblockuser(
                    @UserId,
                    @BlockedUserId)",
                new
                {
                    UserId = userId,
                    BlockedUserId = blockedUserId
                });

            return result?.IsBlocked ?? false;
        }

        public async Task<IEnumerable<NotificationDto>> GetNotifications(int userId)
        {
            return await _genericRepository.QueryAsync<NotificationDto>(
                @"SELECT * FROM ""_vender"".sp_getnotifications(@UserId)",
                new { UserId = userId });
        }

        public async Task UpdateDeliveredStatus(int receiverId)
        {
            await _genericRepository.ExecuteAsync(
                @"SELECT * FROM ""_vender"".sp_updatedeliveredstatus(@ReceiverId)",
                new { ReceiverId = receiverId });
        }

        public async Task UpdateSeenStatus(int messageId, int receiverId, int senderId)
        {
            await _genericRepository.ExecuteAsync(
                @"SELECT * FROM ""_vender"".sp_updateseenstatus(
                    @ReceiverId,
                    @MessageId)",
                new
                {
                    ReceiverId = receiverId,
                    MessageId = messageId
                });
        }

        public async Task UpdateLastSeen(int userId)
        {
            await _genericRepository.ExecuteAsync(
                @"SELECT * FROM ""_vender"".sp_updateuserlastseen(@UserId)",
                new { UserId = userId });
        }
    }
}