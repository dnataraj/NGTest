
namespace NGTest.Storage 
{
    public interface IConnectedUsers
    {
        string[] GetConnectedUsers();
        void AddUser(string connectionId, string user);
        (bool removed, string user) DisconnectUser(string connectionId);
    }
}