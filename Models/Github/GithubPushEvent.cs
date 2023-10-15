using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace TentamenSlackBot.Models.Github
{
    public class GithubPushEvent
    {
        [JsonProperty("ref")]
        public string? Ref {  get; set; }

        [JsonProperty("pusher")]
        public Pusher? Pusher { get; set; }


        [JsonProperty("commits")]
        public Commit[]? Commits { get; set; }
    }
}
