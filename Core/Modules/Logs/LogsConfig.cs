using System.Collections.Generic;
using Core.Modules.Logs.Enums;
using Exiled.API.Interfaces;

namespace Core.Modules.Logs;

public class LogsConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;

    public Dictionary<WebhookType, string> Webhooks { get; set; } = new()
    {
        [WebhookType.CommandLogs] = "",
        [WebhookType.GameLogs] = "",
        [WebhookType.KillLogs] = "",
        [WebhookType.ErrorLogs] = "",
        [WebhookType.ConsoleCommandLogs] = ""
    };
}
