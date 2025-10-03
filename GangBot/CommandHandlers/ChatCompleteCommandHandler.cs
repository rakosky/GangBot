using Discord.WebSocket;
using GangBot.Settings;
using Microsoft.Extensions.Options;
using OpenAI.Responses;

namespace GangBot.CommandHandlers;

public class ChatCompleteCommandHandler : BaseCommandHandler
{
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    readonly OpenAIResponseClient _openAIResponseClient;

    public ChatCompleteCommandHandler(
        OpenAIResponseClient openAIResponseClient,
        IOptions<DiscordClientSettings> options) : base(options)
    {
        _openAIResponseClient = openAIResponseClient;

        ShouldHandle = (m) =>
        {
            if (!base.ShouldHandle(m))
                return false;

            return m.Content.Split(' ', 1)[0].Contains("chat", StringComparison.OrdinalIgnoreCase);
        };
    }

    public override Predicate<SocketMessage> ShouldHandle { get; }

    public async override Task HandleCommandAsync(SocketMessage message)
    {
        var content = message.Content;
        var splitIdx = content.IndexOf(' ');
        if (splitIdx <= 0)
            return;

        OpenAIResponse response = await _openAIResponseClient.CreateResponseAsync(content.Substring(splitIdx));

        await message.Channel.SendMessageAsync(response.GetOutputText());
    }
}
