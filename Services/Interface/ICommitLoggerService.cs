using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TentamenSlackBot.Models.Github;

namespace TentamenSlackBot.Services.Interface
{
    public interface ICommitLoggerService
    {
        Task Add(GithubPushEvent pushEvent);
        Task<IEnumerable<CommitEntity>> GetAllLogs();
    }
}
