using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TentamenSlackBot.Models.Github;
using TentamenSlackBot.Services.Interface;

namespace TentamenSlackBot.Controllers
{
    public class GithubWebHook
    {
        private readonly ILogger _logger;
        private readonly ICommitLoggerService _commitLoggerService;
        private readonly ISlackBotService _slackBotService;

        public GithubWebHook(ILoggerFactory loggerFactory, ICommitLoggerService commitLoggerService, ISlackBotService slackBotService)
        {
            _logger = loggerFactory.CreateLogger<GithubWebHook>();
            _commitLoggerService = commitLoggerService;
            _slackBotService = slackBotService;
        }

        [Function("GithubWebHook")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("GitHub Webhook Received");

            GithubPushEvent pushEvent;
            try
            {
                var json = req.ReadAsStringAsync().Result;
                pushEvent = JsonConvert.DeserializeObject<GithubPushEvent>(json);

                if (pushEvent == null)
                {
                    _logger.LogWarning("Received null payload");
                    var nullPayloadResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await nullPayloadResponse.WriteStringAsync("Null payload received");
                    return nullPayloadResponse;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize GitHub Webhook json");
                var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                errorResponse.WriteString("Invalid JSON payload");
                return errorResponse;
            }

            try
            {
                await _slackBotService.SendMessage(pushEvent);
                await _commitLoggerService.Add(pushEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add commit log or send message to slack");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Failed to process payload");
                return errorResponse;
            }


            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}
