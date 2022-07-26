using HarmonyLib;

namespace Core.Modules.Essentials.Patches
{
    [HarmonyPatch(typeof(Lift), nameof(Lift.UseLift))]
    public class LiftDurationPatch
    {
        private static void Prefix(Lift __instance)
        {
            __instance.movingSpeed = 7;
        }
    }
}