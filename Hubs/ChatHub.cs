using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NGTest.Storage;

namespace NGTest.Hubs 
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly ILogger _logger;

        private readonly StorageHelper _storageHelper;
        private readonly IConnectedUsers _connectedUsers;

        public ChatHub(ILogger<ChatHub> logger, StorageHelper storageHelper, IConnectedUsers connectedUsers) 
        {
            //_logger = loggerFactory.CreateLogger<ChatHub>();
            _logger = logger;
            _storageHelper = storageHelper;
            _connectedUsers = connectedUsers;

        }
        
        // Broadcase message from one client to all, so that that there is a global view of messages.
        // Messages are persisted to Azure Table Storage - and will save the user, the message itself 
        // and a timestamp.
        public async Task BroadcastMessage(string user, string message, string timestamp) 
        {
            _logger.LogInformation($"Broadcasting [{message}] from user : {user}");

            // storage message
            await _storageHelper.StorageChatMessageAsync(Context.ConnectionId, user, message, timestamp);

            await Clients.All.ReceiveBroadcast(user, message, timestamp);
        }

        // Broadcast all connected users, so each user can see who is online.
        public async Task BroadcastConnectedUsers() 
        {
            _logger.LogInformation($"Updating connected users to all clients...");
            await Clients.All.UpdateConnectedUsers(_connectedUsers.GetConnectedUsers());
        }        

        // Clients call Connect in order to allow the hub to keep track of 
        // connected users internally.
        public void Connect(string user)
        {
            _logger.LogDebug("Calling connect!");
            _connectedUsers.AddUser(Context.ConnectionId, user);
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogDebug($"Welcoming connection {Context.ConnectionId}...");
            await base.OnConnectedAsync();
        }   

        // Update the internal connected user list as users disconnect from the Hub.        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var returned = _connectedUsers.DisconnectUser(Context.ConnectionId);

            if (returned.removed) 
            {
                _logger.LogDebug($"Removing {returned.user} from connected users...");
                await BroadcastConnectedUsers();
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}