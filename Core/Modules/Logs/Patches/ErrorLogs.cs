using System.Reflection;
using Core.Modules.Logs.Enums;
using Exiled.API.Features;
using HarmonyLib;

namespace Core.Modules.Logs.Patches
{
    [HarmonyPatch(typeof(Log), nameof(Log.Error), typeof(object))]
    internal static class ErrorLogs
    {
        private static void Postfix(object message)
        {
            WebhookSender.AddMessage($"**{Assembly.GetCallingAssembly().GetName().Name}**\n```{message}```", WebhookType.ErrorLogs);
        }
    }
    
    [HarmonyPatch(typeof(Log), nameof(Log.Error), typeof(string))]
    internal static class ErrorLogs2
    {
        private static void Postfix(string message)
        {
            WebhookSender.AddMessage($"**{Assembly.GetCallingAssembly().GetName().Name}**\n```{message}```", WebhookType.ErrorLogs);
        }
    }
}