using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace NGTest.Hubs 
{
    public class ChatHub : Hub
    {
        private readonly ILogger _logger;
        private static Dictionary<string, string> _connected = new Dictionary<string, string>();

        public ChatHub(ILogger<ChatHub> logger) 
        {
            //_logger = loggerFactory.CreateLogger<ChatHub>();
            _logger = logger;
        }
        public async Task BroadcastMessage(string user, string message) 
        {
            _logger.LogInformation($"Broadcasting [{message}] from user : {user}");
            await Clients.All.SendAsync("ReceiveBroadcast", user, message);
        }

        public async Task BroadcastConnectedUsers() 
        {
            _logger.LogInformation($"Updating connected users to all clients...");
            await Clients.All.SendAsync("UpdateConnectedUsers", GetConnectedUsers());
        }        

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

        public string[] GetConnectedUsers() {
            _logger.LogDebug("Getting connected users...");
            string[] users = new string[_connected.Values.Count];
            _connected.Values.CopyTo(users, 0);
            _logger.LogDebug($"{users.Length} users have connected...");
            return users;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogDebug($"Welcoming connection {Context.ConnectionId}...");
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }   
        
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