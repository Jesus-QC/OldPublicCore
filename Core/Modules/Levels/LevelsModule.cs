using System;
using Core.Loader.Features;
using Core.Modules.Levels.Events;
using Exiled.API.Enums;
using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;
using Warhead = Exiled.Events.Handlers.Warhead;

namespace Core.Modules.Levels
{
    public class LevelsModule : CoreModule<LevelsConfig>
    {
        public override string Name { get; } = "Core.Levels";
        public override string Prefix { get; } = "core_levels";
        public override byte Priority { get; } = 5;

        private PlayerHandler _playerHandler;
        private ServerHandler _serverHandler;
        
        public override void OnEnabled()
        {
            _playerHandler = new PlayerHandler();

            Player.Verified += _playerHandler.OnVerified;
            Player.PickingUpItem += _playerHandler.OnPickingUpItem;
            Player.CancellingItemUse += _playerHandler.OnCancellingItemUse;
            Player.InteractingDoor += _playerHandler.OnInteractingDoor;
            Player.Escaping += _playerHandler.OnEscaping;
            Player.EscapingPocketDimension += _playerHandler.OnEscapingPocket;
            Player.Died += _playerHandler.OnDied;
            Player.Dying += _playerHandler.OnDying;
            Player.ChangingMicroHIDState += _playerHandler.OnChangingMicroHIDState;
            Player.UsedItem += _playerHandler.OnUsedItem;
            Player.Hurting += _playerHandler.OnHurting;
            Player.Handcuffing += _playerHandler.OnHandcuffing;
            Player.ActivatingWorkstation += _playerHandler.OnActivatingWorkstation;
            Player.IntercomSpeaking += _playerHandler.OnIntercomSpeaking;
            Player.ThrowingItem += _playerHandler.OnThrowingItem;
            Player.DroppingItem += _playerHandler.OnDroppingItem;
            Player.FlippingCoin += _playerHandler.OnFlippingCoin;
            Player.EnteringPocketDimension += _playerHandler.OnEnteringPocket;
            Exiled.Events.Handlers.Scp914.UpgradingPlayer  += _playerHandler.OnUpgradingPlayer;

            _serverHandler = new ServerHandler();

            Server.RestartingRound += _serverHandler.OnRestartingRound;
            Server.RoundEnded += _serverHandler.OnRoundEnded;
            Server.RespawningTeam += _serverHandler.OnRespawningTeam;
            Warhead.Starting += _serverHandler.OnStartingWarhead;
            
            base.OnEnabled();
        }
    }
}