using System;
using Core.Loader.Features;
using Exiled.API.Features;
using PlayerEvents = Exiled.Events.Handlers.Player;
using ServerEvents = Exiled.Events.Handlers.Server;


namespace Core.Modules.EndScreen
{
    public class EndScreenModule : CoreModule<EndScreenConfig>
    {
        public override string Name { get; } = "EndScreen";

        private EventHandlers _eventHandlers;
        
        public override void OnEnabled()
        {
            _eventHandlers = new EventHandlers();
            
            PlayerEvents.Verified += _eventHandlers.OnVerified;
            PlayerEvents.Died += _eventHandlers.OnDied;
            PlayerEvents.Hurting += _eventHandlers.OnHurting;
            PlayerEvents.Escaping += _eventHandlers.OnEscaping;

            ServerEvents.RoundEnded += _eventHandlers.OnEndedRound;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerEvents.Verified -= _eventHandlers.OnVerified;
            PlayerEvents.Died -= _eventHandlers.OnDied;
            PlayerEvents.Hurting -= _eventHandlers.OnHurting;
            PlayerEvents.Escaping -= _eventHandlers.OnEscaping;

            ServerEvents.RoundEnded -= _eventHandlers.OnEndedRound;

            _eventHandlers = null;
            
            base.OnDisabled();
        }
    }
}