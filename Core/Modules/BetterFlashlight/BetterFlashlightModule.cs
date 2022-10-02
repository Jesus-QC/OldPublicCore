using Core.Features.Data.Configs;
using Core.Loader.Features;
using Exiled.API.Features;

namespace Core.Modules.BetterFlashlight;

public class BetterFlashlightModule : CoreModule<EmptyConfig>
{
    public override string Name { get; } = "BetterFlashlight";

    public static FlashlightManager FlashlightManager;
    
    public override void OnEnabled()
    {
        Log.Info(1);
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
}