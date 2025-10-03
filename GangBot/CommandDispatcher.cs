using Discord.WebSocket;
using GangBot.CommandHandlers;
using GangBot.Settings;
using Microsoft.Extensions.Options;
using System;

namespace GangBot;

public class CommandDispatcher
{
    readonly ILogger<CommandDispatcher> _logger;
    readonly IEnumerable<IDiscordCommandHandler> _handlers;
    readonly DiscordClientSettings _discordClientSettings;

    public CommandDispatcher(
        ILogger<CommandDispatcher> logger,
        IEnumerable<IDiscordCommandHandler> handlers,
        IOptions<DiscordClientSettings> discordClientOptions)
    {
        _logger = logger;
        _handlers = handlers;
        _discordClientSettings = discordClientOptions.Value;
    }

    public async Task DispatchAsync(SocketMessage message)
    {
        if (message.Author.IsBot) return;

        if (!message.Content.StartsWith(_discordClientSettings.Prefix))
            return;

        foreach (var handler in _handlers)
        {
            if(handler.ShouldHandle(message))
                await handler.HandleCommandAsync(message);
        }

        //var handler = _handlers.FirstOrDefault(h => h.ShouldHandle(message));

        
        //if (handler is null)
        //{
        //    _logger.LogInformation("No handler found for message: {Content}", message.Content);
        //    return;
        //}
    }
}
