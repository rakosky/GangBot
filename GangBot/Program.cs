using GangBot.Settings;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using OpenAI.Responses;

namespace GangBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Services.Configure<DiscordClientSettings>(
                builder.Configuration.GetSection("DiscordClient"));
            builder.Services.Configure<OpenAiSettings>(
                builder.Configuration.GetSection("OpenAi"));

            builder.Services
                .AddDiscordGateway(options =>
                {
                    var settings = builder.Configuration
                       .GetSection("DiscordClient")
                       .Get<DiscordClientSettings>();

                    ArgumentException.ThrowIfNullOrWhiteSpace(settings?.Token);

                    options.Intents = GatewayIntents.GuildMessages
                                      | GatewayIntents.DirectMessages
                                      | GatewayIntents.MessageContent
                                      | GatewayIntents.DirectMessageReactions
                                      | GatewayIntents.GuildMessageReactions
                                      | GatewayIntents.AllNonPrivileged
                                      | GatewayIntents.Guilds;
                    options.Token = settings.Token;
                })
                .AddGatewayHandlers(typeof(Program).Assembly)
                .AddApplicationCommands();

            builder.Services.AddSingleton(sp =>
            {
                var settings = builder.Configuration
                    .GetSection("OpenAi")
                    .Get<OpenAiSettings>();

                ArgumentException.ThrowIfNullOrWhiteSpace(settings?.Model);
                ArgumentException.ThrowIfNullOrWhiteSpace(settings?.ApiKey);

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                return new OpenAIResponseClient(settings.Model, settings.ApiKey);
#pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            });

            builder.Services.AddLogging(builder =>
            {
                builder
                    .AddDebug()
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information);
            });

            var host = builder.Build();

            // Add discord application command modules in assembly
            host.AddModules(typeof(Program).Assembly);

            host.Run();
        }
    }
}