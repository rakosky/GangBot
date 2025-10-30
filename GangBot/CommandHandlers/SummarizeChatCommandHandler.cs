//using Discord;
//using Discord.WebSocket;
//using GangBot.Settings;
//using Microsoft.Extensions.Options;
//using OpenAI.Responses;
//using System.Text;

//namespace GangBot.CommandHandlers;

//public class SummarizeChatCommandHandler : BaseCommandHandler
//{
//#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
//    readonly OpenAIResponseClient _openAIResponseClient;

//    const string _promptTemplate = "Summarize the following conversation between a group of people in a Discord chat. The summary should be concise and capture the main points discussed. Use bullet points for clarity.";

//    public SummarizeChatCommandHandler(
//        OpenAIResponseClient openAIResponseClient,
//        IOptions<DiscordClientSettings> options) : base(options)
//    {
//        _openAIResponseClient = openAIResponseClient;

//        ShouldHandle = (m) =>
//        {
//            if (!base.ShouldHandle(m))
//                return false;

//            return m.Content.Split(' ', 1)[0].Contains("summarize", StringComparison.OrdinalIgnoreCase);
//        };
//    }

//    public override Predicate<SocketMessage> ShouldHandle { get; }

//    public async override Task HandleCommandAsync(SocketMessage message)
//    {
//        var content = message.Content;
//        var splitIdx = content.IndexOf(' ');
//        if (splitIdx <= 0)
//            return;

//        if (!int.TryParse(content.Substring(splitIdx), out int count))
//            return;

//        count = Math.Clamp(count, 1, 25);
//        var messages = await message.Channel.GetMessagesAsync(count + 1).FlattenAsync();
//        var sb = new StringBuilder();
//        int c = 1;
//        foreach (var m in messages.Skip(1).Select(m => m.Content).ToList())
//        {
//            sb.Append($"Message {c}: {m}");
//        }


//        OpenAIResponse response = await _openAIResponseClient.CreateResponseAsync($"{_promptTemplate}: {sb.ToString()}");

//        await message.Channel.SendMessageAsync(response.GetOutputText());
//    }
//}
