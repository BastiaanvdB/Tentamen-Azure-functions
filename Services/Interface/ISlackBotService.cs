using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TentamenSlackBot.Models.Github;

namespace TentamenSlackBot.Services.Interface
{
    public interface ISlackBotService
    {
        Task SendMessage(GithubPushEvent pushEvent);
    }
}
