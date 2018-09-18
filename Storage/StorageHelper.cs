using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NGTest.Storage 
{
    public class StorageHelper    
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;    
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly CloudTable _messageTable;

        // Initialize access to the cloud storage account, and create a Message table, if needed.
        public StorageHelper(ILogger<StorageHelper> logger, IConfiguration configuration) 
        {
            _logger = logger;
            _configuration = configuration;  

            _cloudStorageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("ChatStorage"));
            CloudTableClient tableClient = _cloudStorageAccount.CreateCloudTableClient();
            _messageTable = tableClient.GetTableReference(_configuration["Chat:StorageTable"]);
            
            _messageTable.CreateIfNotExistsAsync().Wait(); // is this a good idea?
        }

        // Package message attributes into MessageEntity and insert into our
        // Azure Table
        public async Task StorageChatMessageAsync(string id, string user, string message, string timestamp) 
        {
            MessageEntity messageEntity = new MessageEntity(id, user, timestamp);
            messageEntity.Message = message;

            _logger.LogInformation($"Inserting message from {user} @ {timestamp}");
            TableOperation insert = TableOperation.Insert(messageEntity);
            await _messageTable.ExecuteAsync(insert);
        }
    }
}