using Core.Loader.Features;
using ServerEvents = Exiled.Events.Handlers.Server;
using Warhead = Exiled.Events.Handlers.Warhead;

namespace Core.Modules.AutoNuke;

public class AutoNukeModule : CoreModule<AutoNukeConfig>
{
    public override string Name { get; } = "AutoNuke";

    private AlphaManager _utilities;

    public override void OnEnabled()
    {
        _utilities = new AlphaManager(this);

        ServerEvents.RestartingRound += _utilities.OnRestartingRound;
        ServerEvents.RoundStarted += _utilities.OnRoundStarted;
        Warhead.Stopping += _utilities.OnStopping;
        Warhead.Detonated += _utilities.OnDetonated;
            
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        ServerEvents.RestartingRound -= _utilities.OnRestartingRound;
        ServerEvents.RoundStarted -= _utilities.OnRoundStarted;
        Warhead.Stopping -= _utilities.OnStopping;
        Warhead.Detonated -= _utilities.OnDetonated;
            
        _utilities = null;

        base.OnDisabled();
    }
}