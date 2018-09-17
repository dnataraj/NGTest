using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace NGTest.Hubs 
{
    public class ChatHub : Hub
    {
        private readonly ILogger _logger;
        private Dictionary<string, string> _connected = new Dictionary<string, string>();

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
            string[] users = {};
            _connected.Values.CopyTo(users, 0);
            return users;
        }

        public override async Task OnConnectedAsync()
        {
            //await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }   
        
    }
}