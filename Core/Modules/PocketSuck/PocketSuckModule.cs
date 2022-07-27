﻿using Core.Features.Data.Configs;
using Core.Loader.Features;
using Exiled.Events.Handlers;

namespace Core.Modules.PocketSuck;

public class PocketSuckModule : CoreModule<EmptyConfig>
{
    public override string Name { get; } = "PocketSuck";

    private EventsHandler _events;
    
    public override void OnEnabled()
    {
        _events = new EventsHandler();

        Scp106.CreatingPortal += _events.OnCreatingPortal;
        Player.StayingOnEnvironmentalHazard += _events.OnStayingOnEnvironmentalHazard;

        Server.RestartingRound += _events.OnRestartingRound;
        Server.WaitingForPlayers += _events.OnWaitingForPlayers;
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Server.RestartingRound -= _events.OnRestartingRound;
        Server.WaitingForPlayers -= _events.OnWaitingForPlayers;
        
        Player.StayingOnEnvironmentalHazard -= _events.OnStayingOnEnvironmentalHazard;
        Scp106.CreatingPortal -= _events.OnCreatingPortal;
        
        _events = null;
        
        base.OnDisabled();
    }
}