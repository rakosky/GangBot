using Discord;
using Discord.WebSocket;
using GangBot.CommandHandlers;
using GangBot.Settings;
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

            builder.Services.AddSingleton(sp =>
                new DiscordSocketClient(new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
                }));

            builder.Services.AddSingleton<DiscordManager>();
            builder.Services.AddSingleton<CommandDispatcher>();
            builder.Services.AddSingleton<IDiscordCommandHandler, PingCommandHandler>();
            builder.Services.AddSingleton<IDiscordCommandHandler, DingCommandHandler>();
            builder.Services.AddSingleton<IDiscordCommandHandler, ChatCompleteCommandHandler>();

            builder.Services.AddSingleton(sp =>
            {
                var settings = builder.Configuration
                    .GetSection("OpenAi")
                    .Get<OpenAiSettings>();
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

            var discordHandler = host.Services.GetRequiredService<DiscordManager>();
            await discordHandler.StartAsync();
            host.Run();
        }
    }
}