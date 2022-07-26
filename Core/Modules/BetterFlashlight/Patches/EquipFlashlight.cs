using HarmonyLib;
using InventorySystem.Items.Flashlight;

namespace Core.Modules.BetterFlashlight.Patches;

[HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.OnEquipped))]
public class EquipFlashlight
{
    public static bool Prefix(FlashlightItem __instance)
    {
        if (BetterFlashlightModule.FlashlightManager.Batteries.ContainsKey(__instance.ItemSerial) && BetterFlashlightModule.FlashlightManager.Batteries[__instance.ItemSerial] == 0)
            return false;
        
        return true;
    }
}