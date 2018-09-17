using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace NGTest.Storage
{
    public class MessageEntity : TableEntity
    {
        public MessageEntity(string id, string user, string timestamp)
        {
            this.PartitionKey = $"{id}_{user}";
            this.RowKey = timestamp;
        }

        public MessageEntity() {}

        public string Message { get; set; }

    }
}