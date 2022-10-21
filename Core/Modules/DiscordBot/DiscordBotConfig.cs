using Core.Features.Data.Configs;

namespace Core.Modules.DiscordBot;

public class DiscordBotConfig : EmptyConfig
{
    public string Path { get; set; } = "/home/container/Core.Bot/Core.Bot";
    public string Ip { get; set; } = "127.0.0.1";
    public string Token { get; set; } = "ODYzMDEzNjA0MTI1OTY2MzU2.GQ0khu.b9sFDkM7QAq8ewwmiNwEdIvIZ8f-HTH3RsLgIg";
}