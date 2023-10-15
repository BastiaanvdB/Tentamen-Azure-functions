using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TentamenSlackBot.DAL;
using TentamenSlackBot.DAL.Interface;
using TentamenSlackBot.Services;
using TentamenSlackBot.Services.Interface;

var host = new HostBuilder()
    .ConfigureAppConfiguration(configurationBuilder => { })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddTransient<ICommitLoggerRepository, CommitLoggerRepository>();
        services.AddTransient<ICommitLoggerService, CommitLoggerService>();
        services.AddTransient<ISlackBotService, SlackBotService>();
    })
    .Build();

host.Run();
