using System;
using Core.Modules.Logs.Enums;
using Exiled.API.Features;
using HarmonyLib;
using RemoteAdmin;

namespace Core.Modules.Logs.Patches;

[HarmonyPatch(typeof(QueryProcessor), nameof(QueryProcessor.ProcessGameConsoleQuery))]
public static class ConsoleCommandLogging
{
    [HarmonyPrefix]
    public static void Prefix(RemoteAdmin.QueryProcessor __instance, string query) 
    {
        try
        {
            var player = Player.Get(__instance._hub);
            if(player != null)
                WebhookSender.AddMessage($"{player.Nickname.DiscordParse()} ({player.UserId ?? "Srv"}) >> **`{query.DiscordParse()}`**", WebhookType.ConsoleCommandLogs);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
    
}