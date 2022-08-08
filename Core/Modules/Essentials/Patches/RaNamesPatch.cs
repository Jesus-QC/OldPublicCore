using HarmonyLib;

namespace Core.Modules.Essentials.Patches;

[HarmonyPatch(typeof(NicknameSync), nameof(NicknameSync.CombinedName), MethodType.Getter)]
public class RaNamesPatch
{
    static bool Prefix(ref string __result, NicknameSync __instance)
    {
        __result = __instance.MyNick;
        return false;
    }
}