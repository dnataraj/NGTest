using System.Threading.Tasks;

namespace NGTest.Hubs
{
    public interface IChatClient
    {
        Task ReceiveBroadcast(string user, string message, string timestamp);
        Task UpdateConnectedUsers(string[] users);
    }
}