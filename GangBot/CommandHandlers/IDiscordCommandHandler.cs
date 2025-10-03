using Discord.WebSocket;

namespace GangBot.CommandHandlers;

public interface IDiscordCommandHandler
{

    public Predicate<SocketMessage> ShouldHandle { get; }

    Task HandleCommandAsync(SocketMessage message);
}
