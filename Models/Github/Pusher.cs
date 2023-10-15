using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TentamenSlackBot.Models.Github
{
    public class Pusher
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }
    }
}
