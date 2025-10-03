using Discord.WebSocket;
using GangBot.Settings;
using Microsoft.Extensions.Options;

namespace GangBot.CommandHandlers;

public class DingCommandHandler : BaseCommandHandler
{
    public DingCommandHandler(IOptions<DiscordClientSettings> options) : base(options)
    {
        ShouldHandle = (m) =>
        {
            if (!base.ShouldHandle(m))
                return false;

            return m.Content.Split(' ', 1)[0].Contains("ding", StringComparison.OrdinalIgnoreCase);
        };
    }

    public override Predicate<SocketMessage> ShouldHandle { get; }

    public async override Task HandleCommandAsync(SocketMessage message)
    {
        await message.Channel.SendMessageAsync("Dong!");
    }
}
