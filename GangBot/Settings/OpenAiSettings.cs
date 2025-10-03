namespace GangBot.Settings;

internal class OpenAiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;

    //public int MaxTokens { get; set; } = 2048;
    //public double Temperature { get; set; } = 0.7;
    //public double TopP { get; set; } = 1.0;
    //public int FrequencyPenalty { get; set; } = 0;
    //public int PresencePenalty { get; set; } = 0;
}
