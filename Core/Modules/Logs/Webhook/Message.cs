namespace Core.Modules.Logs.Webhook;

public class Message
{
    // ReSharper disable InconsistentNaming
    public string username { get; set; } = $"{Exiled.API.Features.Server.Port} | Game Logs";
    public string avatar_url { get; set; } = "https://cdn.discordapp.com/attachments/935841786948096000/1013154497570279424/Oh_yes.jpg";
    public string content { get; set; }

    public Message(string _content)
    {
        content = _content;
    }
}