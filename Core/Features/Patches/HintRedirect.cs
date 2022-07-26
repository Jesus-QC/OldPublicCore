using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using HarmonyLib;
using Hints;

namespace Core.Features.Patches
{
    [HarmonyPatch(typeof(HintDisplay), nameof(HintDisplay.Show))]
    public static class HintRedirect
    {
        public static bool Prefix(HintDisplay __instance, Hint hint)
        {
            var type = hint.GetType();
            
            if (type == typeof(TranslationHint))
                return false;
            
            if (type == typeof(TextHint))
            {
                var t = hint as TextHint;
                Player.Get(__instance.gameObject).SendHint(ScreenZone.CenterBottom ,t.Text, t.DurationScalar);
                return false;
            }

            return true;
        }
    }
}