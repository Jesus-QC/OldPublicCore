using System.Collections.Generic;
using Core.Modules.Logs.Enums;
using Exiled.API.Interfaces;

namespace Core.Modules.Logs;

public class LogsConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;

    public Dictionary<WebhookType, string> Webhooks { get; set; } = new()
    {
        [WebhookType.CommandLogs] = "https://discord.com/api/webhooks/951559473422548992/SBr5EHUrp8NZzfKbQKJi1bcuiV628uDg5j2FkJ0R3mkVSgCxO397RjtczMuTKg9hTELI",
        [WebhookType.GameLogs] = "https://discord.com/api/webhooks/951559361451397181/4JGB0gsm3Q78_xGdRuKtoRwnO47KYUtj9_Xqz8BwfcSsFxFVWqQbfL7wGEpG91on-rMs",
        [WebhookType.KillLogs] = "https://discord.com/api/webhooks/953777572175573013/9emAR7Ra8lMawSc2zFv9QMPbhSwzmDYSFONO3-M18ymLuLHr7owAAL-DwlAJv8hAD2BN",
        [WebhookType.ErrorLogs] = "https://discord.com/api/webhooks/953778381252591626/dPdg0tm1RmpFpxgHarVIS9L8hhF4ySjP4Zy5snP0GOxRJ7SXb92yUE8jpmuVe7fvzEKW"
    };
}