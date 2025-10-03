using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using GangBot.Settings;
using Microsoft.Extensions.Options;


namespace GangBot
{
    public class DiscordManager
    {
        readonly ILogger<DiscordManager> _logger;
        readonly DiscordSocketClient _socketClient;
        readonly DiscordClientSettings _discordClientSettings;
        readonly CommandDispatcher _commandDispatcher;

        public DiscordManager(ILogger<DiscordManager> logger, DiscordSocketClient socketClient, IOptions<DiscordClientSettings> discordClientOptions, CommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _socketClient = socketClient;
            _discordClientSettings = discordClientOptions.Value;
            _commandDispatcher = commandDispatcher;
        }

        public async Task StartAsync()
        {
            _socketClient.Log += (l) =>
            {
                _logger.LogInformation("Discord client log event triggered: {Message}", l.Message);
                return Task.CompletedTask;
            };

            _socketClient.MessageReceived += async (message) =>
            {
                _logger.LogInformation("Received message from {Author}: {Content}", message.Author.Username, message.Content);

                await _commandDispatcher.DispatchAsync(message);
            };

            await _socketClient.LoginAsync(Discord.TokenType.Bot, _discordClientSettings.Token);
            await _socketClient.StartAsync();
        }   
    }
}
