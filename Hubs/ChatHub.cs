using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using VenderTest.Service;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public static readonly ConcurrentDictionary<int, HashSet<string>> OnlineUsers =
        new ConcurrentDictionary<int, HashSet<string>>();


    public override async Task OnConnectedAsync()
    {
       
        var userIdStr = Context.UserIdentifier
            ?? Context.GetHttpContext()?.Request.Query["userId"].ToString();

        if (!int.TryParse(userIdStr, out var userId))
            return;

      
        OnlineUsers.AddOrUpdate(
            userId,
            new HashSet<string> { Context.ConnectionId },
            (key, existingSet) =>
            {
                existingSet.Add(Context.ConnectionId);
                return existingSet;
            });

      
        await _chatService.UpdateDeliveredStatus(userId);
     
        await Clients.All.SendAsync("OnlineUsers", OnlineUsers.Keys.ToList());

        await Clients.All.SendAsync("UserStatusChanged", new
        {
            userId,
            status = "Online",
            lastSeen = (DateTime?)null
        });
        await _chatService.GetNotifications(userId);


        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userIdStr = Context.UserIdentifier
            ?? Context.GetHttpContext()?.Request.Query["userId"].ToString();

        if (!int.TryParse(userIdStr, out var userId))
            return;

        if (OnlineUsers.TryGetValue(userId, out var connections))
        {
            connections.Remove(Context.ConnectionId);

            if (!connections.Any())
            {
                OnlineUsers.TryRemove(userId, out _);


                await _chatService.UpdateLastSeen(userId);

                var lastSeenTime = DateTime.UtcNow;

                await Clients.All.SendAsync("UserStatusChanged", new
                {
                    userId,
                    status = "Offline",
                    lastSeen = lastSeenTime
                });
            }
        }

        await Clients.All.SendAsync("OnlineUsers", OnlineUsers.Keys.ToList());

        await base.OnDisconnectedAsync(exception);
    }
    public async Task Typing(int senderId, int receiverId)
    {
        if (OnlineUsers.TryGetValue(receiverId, out var connections))
        {
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId)
                    .SendAsync("UserTyping", senderId);
            }
        }
    }


    public async Task Delivered(int messageId, int senderId, int receiverId)
    {
        
        await _chatService.MarkMessageDelivered(messageId, receiverId);

      
        if (OnlineUsers.TryGetValue(senderId, out var senderConnections))
        {
            foreach (var conn in senderConnections)
            {
                await Clients.Client(conn).SendAsync("MessageStatusUpdated", new
                {
                    messageId,
                    status = "Delivered"
                });
            }
        }
    }
    //// Hub method: called when receiver actually receives the message
    //public async Task MessageReceivedByClient(int messageId, int senderId, int receiverId)
    //{
    //    // Mark message as Delivered in DB
    //    await _chatService.MarkMessageDelivered(messageId, receiverId);

    //    // Notify sender about Delivered (double tick)
    //    if (OnlineUsers.TryGetValue(senderId, out var senderConnections))
    //    {
    //        foreach (var conn in senderConnections)
    //        {
    //            await Clients.Client(conn).SendAsync("MessageStatusUpdated", new
    //            {
    //                messageId,
    //                status = "Delivered" // ✅ double tick
    //            });
    //        }
    //    }
    //}



    public async Task MessageSeen(int messageId, int receiverId, int senderId)
    {
        if (OnlineUsers.TryGetValue(senderId, out var senderConnections))
        {
            foreach (var connectionId in senderConnections)
            {
                await Clients.Client(connectionId)
                    .SendAsync("MessageSeen", new
                    {
                        messageId,
                        receiverId,
                        seenAt = DateTime.UtcNow
                    });
            }
        }
    }
}