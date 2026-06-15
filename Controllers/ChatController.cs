using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using VenderTest.DTOs;
using VenderTest.Service;

namespace VenderTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _service;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IChatService service, IHubContext<ChatHub> hubContext)
        {
            _service = service;
            _hubContext = hubContext;
        }

        [HttpGet("GetContacts")]
        public async Task<IActionResult> GetContacts(int userId)
        {
            try
            {
                var data = await _service.GetContacts(userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(ChatMessageDto model)
        {
            
            var savedMessage = await _service.SendMessage(model);

         
            if (ChatHub.OnlineUsers.TryGetValue(model.ReceiverId, out var receiverConns))
            {
                var tasks = receiverConns.Select(conn =>
                    _hubContext.Clients.Client(conn).SendAsync("ReceiveMessage", new
                    {
                        messageId = savedMessage.MessageId,
                        senderId = model.SenderId,
                        receiverId = model.ReceiverId,
                        messageText = model.MessageText,
                        createdAt = savedMessage.SentAt,
                        status = "Sent"
                    })
                );
                await Task.WhenAll(tasks);
            }

          
            if (ChatHub.OnlineUsers.TryGetValue(model.SenderId, out var senderConns))
            {
                var sentTasks = senderConns.Select(conn =>
                    _hubContext.Clients.Client(conn).SendAsync("MessageStatusUpdated", new
                    {
                        messageId = savedMessage.MessageId,
                        status = "Sent"
                    })
                );
                await Task.WhenAll(sentTasks);
            }

            return Ok(savedMessage);
        }
        [HttpGet("GetBlockedUsers")]
        public async Task<IActionResult> GetBlockedUsers(int userId)
        {
            try
            {
                var blockedUsers = await _service.GetBlockedUsersAsync(userId);
                return Ok(blockedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("GetMessages")]
        public async Task<IActionResult> GetMessages(int user1, int user2)
        {
            try
            {
                var messages = await _service.GetMessages(user1, user2);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPost("MarkAsSeen")]
        public async Task<IActionResult> MarkAsSeen(int messageId, int receiverId, int senderId)
        {
            await _service.UpdateSeenStatus(messageId, receiverId, senderId);

            await _hubContext.Clients.User(senderId.ToString())
                .SendAsync("MessageSeen", new
                {
                    messageId,
                    senderId,
                    receiverId,
                    seenAt = DateTime.UtcNow
                });

            return Ok();
        }

        [HttpGet("GetNotifications")]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            try
            {
                var notifications = await _service.GetNotifications(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpPost("BlockUser")]
        public async Task<IActionResult> BlockUser([FromBody] BlockUserRequest request)
        {
            try
            {
                bool isBlocked = await _service.BlockUser(request.UserId, request.BlockedUserId);

                string message = isBlocked
                    ? "User blocked successfully."
                    : "User unblocked successfully.";

                await _hubContext.Clients.All.SendAsync("BlockEvent", new
                {
                    userId = request.UserId,
                    blockedUserId = request.BlockedUserId,
                    isBlocked
                });
                // Handle status updates
                if (isBlocked)
                {
                    await _hubContext.Clients.User(request.BlockedUserId.ToString())
                        .SendAsync("UserStatusChanged", new { userId = request.UserId, status = "Offline", lastSeen = (DateTime?)null });

                    await _hubContext.Clients.User(request.UserId.ToString())
                        .SendAsync("UserStatusChanged", new { userId = request.BlockedUserId, status = "Offline", lastSeen = (DateTime?)null });
                }
                else
                {
                    bool userOnline = ChatHub.OnlineUsers.ContainsKey(request.UserId);
                    bool blockedUserOnline = ChatHub.OnlineUsers.ContainsKey(request.BlockedUserId);

                    DateTime? lastSeenUser = null;
                    DateTime? lastSeenBlockedUser = null;

                    await _hubContext.Clients.User(request.BlockedUserId.ToString())
                        .SendAsync("UserStatusChanged", new
                        {
                            userId = request.UserId,
                            status = userOnline ? "Online" : "Offline",
                            lastSeen = lastSeenUser
                        });

                    await _hubContext.Clients.User(request.UserId.ToString())
                        .SendAsync("UserStatusChanged", new
                        {
                            userId = request.BlockedUserId,
                            status = blockedUserOnline ? "Online" : "Offline",
                            lastSeen = lastSeenBlockedUser
                        });
                }

                return Ok(new { success = true, isBlocked, message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}