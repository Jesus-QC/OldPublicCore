using Core.Features.Data.Configs;
using Core.Loader.Features;
using Exiled.Events.Handlers;

namespace Core.Modules.PocketSuck;

public class PocketSuckModule : CoreModule<PocketSuckConfig>
{
    public override string Name { get; } = "PocketSuck";

    private EventsHandler _events;
    
    public override void OnEnabled()
    {
        _events = new EventsHandler();

        if (Config.SinkholesEnabled)
        {
            Player.StayingOnEnvironmentalHazard += _events.OnStayingOnEnvironmentalHazard;
        }

        if (Config.PortalEnabled)
        {
            Scp106.CreatingPortal += _events.OnCreatingPortal;

            Server.RestartingRound += _events.OnRestartingRound;
            Server.WaitingForPlayers += _events.OnWaitingForPlayers;
        }
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        if (Config.SinkholesEnabled)
        {
            Player.StayingOnEnvironmentalHazard -= _events.OnStayingOnEnvironmentalHazard;
        }

        if (Config.PortalEnabled)
        {
            Scp106.CreatingPortal -= _events.OnCreatingPortal;

            Server.RestartingRound -= _events.OnRestartingRound;
            Server.WaitingForPlayers -= _events.OnWaitingForPlayers;
        }
        
        _events = null;
        
        base.OnDisabled();
    }
}