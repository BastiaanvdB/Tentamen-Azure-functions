using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TentamenSlackBot.Services.Interface;

namespace TentamenSlackBot.Controllers
{
    public class LogOverview
    {
        private readonly ILogger _logger;
        private readonly ICommitLoggerService _commitLoggerService;

        public LogOverview(ILoggerFactory loggerFactory, ICommitLoggerService commitLoggerService)
        {
            _logger = loggerFactory.CreateLogger<LogOverview>();
            _commitLoggerService = commitLoggerService;
        }

        [Function("LogOverview")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("HTTP trigger get all logs function processed a request.");

            try
            {
                var logs = await _commitLoggerService.GetAllLogs();
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");

                response.WriteString(JsonConvert.SerializeObject(logs));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the logs.");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                errorResponse.WriteString("An error occurred while retrieving the logs.");
                return errorResponse;
            }
        }
    }
}
