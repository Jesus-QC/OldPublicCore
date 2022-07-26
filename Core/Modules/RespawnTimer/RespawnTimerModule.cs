using System;
using Core.Loader.Features;
using Exiled.API.Features;
using Server = Exiled.Events.Handlers.Server;

namespace Core.Modules.RespawnTimer
{
    public class RespawnTimerModule : CoreModule<RespawnTimerConfig>
    {
        public override string Name { get; } = "Core.RespawnTimer";
        public override string Prefix { get; } = "core_respawntimer";

        private EventHandler _eventHandler;
        
        public override void OnEnabled()
        {
            _eventHandler = new EventHandler();

            EventHandler.Tips = Config.Tips;
            
            Server.RoundEnded += _eventHandler.OnEndedRound;
            Server.RoundStarted += _eventHandler.OnRoundStarted;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.RoundEnded -= _eventHandler.OnEndedRound;
            Server.RoundStarted -= _eventHandler.OnRoundStarted;
            
            _eventHandler = null;
            
            base.OnDisabled();
        }
    }
}