using Core.Features.Data.Configs;
using Core.Loader.Features;
using HarmonyLib;
using InventorySystem.Items.Flashlight;

namespace Core.Modules.BetterFlashlight;

public class BetterFlashlightModule : CoreModule<EmptyConfig>
{
    public override string Name { get; } = "BetterFlashlight";

    public static FlashlightManager FlashlightManager;
    
    public override void OnEnabled()
    {
        FlashlightManager = new FlashlightManager();
        
        Exiled.Events.Handlers.Server.RestartingRound += FlashlightManager.OnRestartingRound;
        Exiled.Events.Handlers.Server.RoundStarted += FlashlightManager.OnRoundStarted;

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Exiled.Events.Handlers.Server.RestartingRound -= FlashlightManager.OnRestartingRound;
        Exiled.Events.Handlers.Server.RoundStarted -= FlashlightManager.OnRoundStarted;

        FlashlightManager = null;
        
        base.OnDisabled();
    }
    
    public override void UnPatch()
    {
        Core.Harmony.Unpatch(typeof(FlashlightItem).GetMethod(nameof(FlashlightItem.OnEquipped)), HarmonyPatchType.Prefix, Core.Harmony.Id);
    }
}