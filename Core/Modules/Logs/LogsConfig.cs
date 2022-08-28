using System.Collections.Generic;
using Core.Modules.Logs.Enums;
using Exiled.API.Interfaces;

namespace Core.Modules.Logs;

public class LogsConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;

    public Dictionary<WebhookType, string> Webhooks { get; set; } = new()
    {
        [WebhookType.CommandLogs] = "https://discord.com/api/webhooks/1013504100060647504/gqHwm_G0X82GshDGO4hUZWSdexpHIkQgkgP461msUD6iS1nfjyXkV_7ptCjsetERMep5",
        [WebhookType.GameLogs] = "https://discord.com/api/webhooks/1013504011946696845/QP8xKwevpogqXrHipDcWCxzOFXSbrN2cQzNO7MvFc-Rf7fhSv-jOZi1vMmGEc_myWUmA",
        [WebhookType.KillLogs] = "https://discord.com/api/webhooks/1013504011946696845/QP8xKwevpogqXrHipDcWCxzOFXSbrN2cQzNO7MvFc-Rf7fhSv-jOZi1vMmGEc_myWUmA",
        [WebhookType.ErrorLogs] = "https://discord.com/api/webhooks/953778381252591626/dPdg0tm1RmpFpxgHarVIS9L8hhF4ySjP4Zy5snP0GOxRJ7SXb92yUE8jpmuVe7fvzEKW"
    };
}