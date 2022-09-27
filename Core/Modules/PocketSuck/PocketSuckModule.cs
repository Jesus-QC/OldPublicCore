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

        Player.StayingOnEnvironmentalHazard += _events.OnStayingOnEnvironmentalHazard;

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Player.StayingOnEnvironmentalHazard -= _events.OnStayingOnEnvironmentalHazard;

        _events = null;
        
        base.OnDisabled();
    }
}