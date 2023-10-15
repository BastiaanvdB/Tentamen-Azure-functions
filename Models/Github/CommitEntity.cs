using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TentamenSlackBot.Models.Github
{
    public class CommitEntity : TableEntity
    {
        public string? CommitMessage { get; set; }
        public string? CommitTimestamp { get; set; }
        public string? PusherName { get; set; }
        public string? PusherEmail { get; set; }
        public string? BranchName { get; set; }
    }
}
