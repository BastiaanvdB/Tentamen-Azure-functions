using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TentamenSlackBot.DAL.Interface;
using TentamenSlackBot.Models.Github;
using TentamenSlackBot.Services.Interface;

namespace TentamenSlackBot.Services
{
    public class CommitLoggerService : ICommitLoggerService
    {
        private readonly ICommitLoggerRepository _CommitLoggerRepository;

        public CommitLoggerService(ICommitLoggerRepository commitLoggerService)
        {
            _CommitLoggerRepository = commitLoggerService;
        }

        public async Task<IEnumerable<CommitEntity>> GetAllLogs()
        {
            return await _CommitLoggerRepository.GetAllLogs();
        }

        public async Task Add(GithubPushEvent pushEvent)
        {
            await _CommitLoggerRepository.CreateLog(pushEvent);
        }

    }
}
