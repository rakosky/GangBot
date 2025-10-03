using Discord.WebSocket;
using GangBot.Settings;
using Microsoft.Extensions.Options;

namespace GangBot.CommandHandlers;

public abstract class BaseCommandHandler : IDiscordCommandHandler
{
    readonly DiscordClientSettings _discordClientSettings;

    protected BaseCommandHandler(IOptions<DiscordClientSettings> discordClientOptions)
    {
        _discordClientSettings = discordClientOptions.Value;
    }

    public virtual Predicate<SocketMessage> ShouldHandle => (m) =>
    {
        return m.Content.StartsWith(_discordClientSettings.Prefix, StringComparison.OrdinalIgnoreCase);
    };

    public async virtual Task HandleCommandAsync(SocketMessage message)
    {
        await Task.CompletedTask;
    }
}
