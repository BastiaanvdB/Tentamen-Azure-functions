using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TentamenSlackBot.Models.Github;

namespace TentamenSlackBot.DAL.Interface
{
    public interface ICommitLoggerRepository
    {
        Task CreateLog(GithubPushEvent pushEvent);

        Task<IEnumerable<CommitEntity>> GetAllLogs();

    }
}
