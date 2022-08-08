using HarmonyLib;

namespace Core.Modules.Essentials.Patches;

[HarmonyPatch(typeof(Radio), nameof(Radio.UserCode_CmdSyncTransmissionStatus))]
public class ScpVoiceChatPatch
{
    public static bool Prefix(Radio __instance, bool b)
    {
        if (EssentialsModule.PluginConfig.ScpsAbleToTalk.Contains(__instance._hub.characterClassManager.NetworkCurClass))
            __instance._dissonanceSetup.MimicAs939 = b;
        return true;
    }
}