namespace Core.Modules.Logs.Webhook
{
    public class Message
    {
        // ReSharper disable InconsistentNaming
        public string username { get; set; } = $"{Exiled.API.Features.Server.Port} | Game Logs";
        public string avatar_url { get; set; } = "https://imgur.com/aSSkRAX.png";
        public string content { get; set; }

        public Message(string _content)
        {
            content = _content;
        }
    }
}