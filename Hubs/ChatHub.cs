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
        private static Dictionary<string, string> _connected = new Dictionary<string, string>();

        private readonly StorageHelper _storageHelper;

        public ChatHub(ILogger<ChatHub> logger, StorageHelper storageHelper) 
        {
            //_logger = loggerFactory.CreateLogger<ChatHub>();
            _logger = logger;
            _storageHelper = storageHelper;

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
            await Clients.All.UpdateConnectedUsers(GetConnectedUsers());
        }        

        // Clients call Connect in order to allow the hub to keep track of 
        // connected users internally.
        public void Connect(string user)
        {
            _logger.LogDebug("Calling connect!");
            var id = Context.ConnectionId;
            if (!_connected.ContainsKey(id))
            {
                _logger.LogInformation($"Adding {user} as connected...");
                _connected.Add(id, user);
            }
        }

        private string[] GetConnectedUsers() {
            string[] users = new string[_connected.Values.Count];
            _connected.Values.CopyTo(users, 0);
            _logger.LogDebug($"{users.Length} users have connected...");
            return users;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogDebug($"Welcoming connection {Context.ConnectionId}...");
            await base.OnConnectedAsync();
        }   

        // Update the internal connected user list as users disconnect from the Hub.        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_connected.TryGetValue(Context.ConnectionId, out string user)) 
            {
                _logger.LogDebug($"Removing {user} from connected users...");
                _connected.Remove(Context.ConnectionId);
                await BroadcastConnectedUsers();
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}