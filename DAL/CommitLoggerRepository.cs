using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TentamenSlackBot.DAL.Interface;
using TentamenSlackBot.Models.Github;

namespace TentamenSlackBot.DAL
{
    public class CommitLoggerRepository : ICommitLoggerRepository
    {
        private readonly CloudStorageAccount _storageAccount;
        protected CloudTableClient _tableClient;
        protected CloudTable _table;

        public CommitLoggerRepository()
        {
            _storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("CONNSTRING"));
            _tableClient = _storageAccount.CreateCloudTableClient();

            _table = _tableClient.GetTableReference("CommitTable");
            _table.CreateIfNotExistsAsync().GetAwaiter().GetResult();
        }

        public Task<IEnumerable<CommitEntity>> GetAllLogs()
        {
            List<CommitEntity> entities = new List<CommitEntity>();
            TableQuery<CommitEntity> query = new TableQuery<CommitEntity>();

            foreach (CommitEntity entity in _table.ExecuteQuerySegmentedAsync(query, null).Result)
            {
                entities.Add(entity);
            }

            return Task.FromResult<IEnumerable<CommitEntity>>(entities);
        }

        public async Task CreateLog(GithubPushEvent pushEvent)
        {
            string? branchName = pushEvent.Ref?.Split('/').LastOrDefault();

            foreach (Commit commit in pushEvent.Commits)
            {
                var commitEntity = new CommitEntity
                {
                    PartitionKey = "Commits",
                    RowKey = Guid.NewGuid().ToString(),
                    CommitMessage = commit.Message,
                    CommitTimestamp = commit.Timestamp,
                    PusherName = pushEvent.Pusher.Name,
                    PusherEmail = pushEvent.Pusher.Email,
                    BranchName = branchName
                };

                TableOperation insertOperation = TableOperation.Insert(commitEntity);
                await _table.ExecuteAsync(insertOperation);
            }
        }
    }
}
