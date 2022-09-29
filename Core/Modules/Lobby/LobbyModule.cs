using Core.Loader.Features;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace Core.Modules.Lobby;

public class LobbyModule : CoreModule<LobbyConfig>
{
    public override string Name { get; } = "Lobby";
    public override byte Priority { get; } = 15;

    public static LobbySpawner LobbySpawner;
    public static LobbyConfig LobbyConfig;
    public static string Status = string.Empty;
        
    public override void OnEnabled()
    {
        LobbyConfig = Config;
            
        LobbySpawner = new LobbySpawner();

        Server.WaitingForPlayers += LobbySpawner.OnWaitingForPlayers;
        Server.RoundStarted += LobbySpawner.OnStarting;

        Player.TogglingOverwatch += LobbySpawner.OnTogglingOverwatch;
        Player.ChangingRole += LobbySpawner.OnChangingRole;
        Player.Spawning += LobbySpawner.OnSpawning;
        Player.Verified += LobbySpawner.OnVerified;

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Server.WaitingForPlayers -= LobbySpawner.OnWaitingForPlayers;
        Server.RoundStarted -= LobbySpawner.OnStarting;
            
        Player.TogglingOverwatch -= LobbySpawner.OnTogglingOverwatch;
        Player.ChangingRole -= LobbySpawner.OnChangingRole;
        Player.Spawning -= LobbySpawner.OnSpawning;
        Player.Verified -= LobbySpawner.OnVerified;

        LobbySpawner = null;

        LobbyConfig = null;
            
        base.OnDisabled();
    }
}