using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace NGTest.Storage
{
    public class ConnectedUsers : IConnectedUsers
    {
        private readonly ILogger _logger;
        private readonly Dictionary<string, string> _connected = new Dictionary<string, string>();

        public ConnectedUsers(ILogger<ConnectedUsers> logger)
        {
            _logger = logger;
        }

        public void AddUser(string connectionId, string user) 
        {
            if (!_connected.ContainsKey(connectionId))
            {
                _logger.LogInformation($"Adding {user} as connected...");
                _connected.Add(connectionId, user);
            }
        }

        public string[] GetConnectedUsers() 
        {
            string[] users = new string[_connected.Values.Count];
            _connected.Values.CopyTo(users, 0);
            _logger.LogDebug($"{users.Length} users have connected...");
            return users;
        }

        public (bool removed, string user) DisconnectUser(string connectionId)
        {
            if (_connected.TryGetValue(connectionId, out string user)) 
            {
                _connected.Remove(connectionId);
                return (removed: true, user: user);
            }

            return (removed: false, user: null);
        }
    }
}