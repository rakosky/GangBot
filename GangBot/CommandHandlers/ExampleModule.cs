using NetCord;
using NetCord.Gateway.Voice;
using NetCord.Logging;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace GangBot.CommandHandlers;

public class ExampleModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("pong", "Pong!")]
    public static string Pong() => "Ping!";

    [SlashCommand("square", "Square!")]
    public static string Square(int a) => $"{a}² = {a * a}";

    [UserCommand("ID")]
    public static string Id(User user) => user.Id.ToString();

    [MessageCommand("Timestamp")]
    public static string Timestamp(RestMessage message) => message.CreatedAt.ToString();

    [SlashCommand("echo", "Creates echo", Contexts = [InteractionContextType.Guild])]
    public async Task<string> EchoAsync()
    {
        var guild = Context.Guild!;
        var userId = Context.User.Id;

        if (!guild.VoiceStates.TryGetValue(userId, out var voiceState))
            return "You are not connected to any voice channel!";

        var client = Context.Client;

        var voiceClient = await client.JoinVoiceChannelAsync(
            guild.Id,
            voiceState.ChannelId.GetValueOrDefault(),
            new VoiceClientConfiguration
            {
                ReceiveHandler = new VoiceReceiveHandler(),
                Logger = new ConsoleLogger(),
            });

        await voiceClient.StartAsync();

        await voiceClient.EnterSpeakingStateAsync(new SpeakingProperties(SpeakingFlags.Microphone));

        var outStream = voiceClient.CreateOutputStream(normalizeSpeed: false);

        voiceClient.VoiceReceive += args =>
        {
            if (voiceClient.Cache.Users.TryGetValue(args.Ssrc, out var voiceUserId) && voiceUserId == userId)
                outStream.Write(args.Frame);
            return default;
        };

        return "Echo!";
    }
}
