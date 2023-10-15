using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TentamenSlackBot.Models.Github
{
    public class Commit
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}
