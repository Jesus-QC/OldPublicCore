using Core.Loader.Features;
using Core.Modules.Essentials.Handlers;
using Exiled.Events.Handlers;
using HarmonyLib;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace Core.Modules.Essentials;

public class EssentialsModule : CoreModule<EssentialsConfig>
{
    public override string Name => "Essentials";
    public override byte Priority => 5;

    public static EssentialsConfig PluginConfig { get; private set; }

    private PlayerHandler _playerHandler;
    private ServerHandler _serverHandler;
        
    public override void OnEnabled()
    {
        PluginConfig = Config;

        _serverHandler = new ServerHandler();
        _playerHandler = new PlayerHandler();
            
        Map.AnnouncingNtfEntrance += _serverHandler.OnAnnouncingMtfEntrance;
        Server.RespawningTeam += _serverHandler.OnRespawningTeam;
        Server.RestartingRound += _serverHandler.OnRestartingRound;
        Server.WaitingForPlayers += _playerHandler.OnWaitingForPlayers;
            
        Player.Hurting += _playerHandler.OnHurting;
        Player.Died += _playerHandler.OnDied;
        Player.Left += _playerHandler.OnLeft;
        Player.Verified += _playerHandler.OnVerified;
        Player.TriggeringTesla += _playerHandler.OnTriggeringTesla;
        Player.ChangingRole += _playerHandler.OnChangingRole;

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Player.Hurting -= _playerHandler.OnHurting;
        Player.Verified -= _playerHandler.OnVerified;
        Player.TriggeringTesla -= _playerHandler.OnTriggeringTesla;
        Player.ChangingRole -= _playerHandler.OnChangingRole;
        
        Server.RestartingRound -= _serverHandler.OnRestartingRound;
        Server.WaitingForPlayers -= _playerHandler.OnWaitingForPlayers;
        Server.RespawningTeam -= _serverHandler.OnRespawningTeam;
        Map.AnnouncingNtfEntrance -= _serverHandler.OnAnnouncingMtfEntrance;
        
        _playerHandler = null;
        _serverHandler = null;

        PluginConfig = null;

        base.OnDisabled();
    }

    public override void UnPatch()
    {
        Core.Harmony.Unpatch(typeof(Lift).GetMethod(nameof(Lift.UseLift)), HarmonyPatchType.Prefix, Core.Harmony.Id);
        Core.Harmony.Unpatch(typeof(Radio).GetMethod(nameof(Radio.UserCode_CmdSyncTransmissionStatus)), HarmonyPatchType.Prefix, Core.Harmony.Id);
        Core.Harmony.Unpatch(typeof(NicknameSync).GetMethod(nameof(NicknameSync.CombinedName)), HarmonyPatchType.Prefix, Core.Harmony.Id);
    }
}