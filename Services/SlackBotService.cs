using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TentamenSlackBot.Controllers;
using TentamenSlackBot.Models.Github;
using TentamenSlackBot.Services.Interface;

namespace TentamenSlackBot.Services
{
    public class SlackBotService : ISlackBotService
    {
        private readonly ILogger _logger;
        private readonly string _webhookUrl;

        public SlackBotService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SlackBotService>();
            _webhookUrl = Environment.GetEnvironmentVariable("SlackWebhookUrl");
        }

        public async Task SendMessage(GithubPushEvent pushEvent)
        {
            string? branchName = pushEvent.Ref?.Split('/').LastOrDefault();

            foreach (Commit commit in pushEvent.Commits)
            {
                var message = new
                {
                    text = $"New commit by {pushEvent.Pusher.Name} ({pushEvent.Pusher.Email}) on branch {branchName} at {commit.Timestamp:yyyy-MM-dd HH:mm:ss} - {commit.Message}"
            };

                try
                {
                    var jsonPayload = JsonConvert.SerializeObject(message);

                    using (HttpClient client = new HttpClient())
                    {
                        var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(_webhookUrl, httpContent);

                        if (!response.IsSuccessStatusCode)
                        {
                            _logger.LogError($"Failed to send message to Slack: {response.StatusCode}");
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending message to Slack.");
                }
            }
        }

    }
}
