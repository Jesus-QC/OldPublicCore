using Core.Loader.Features;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace Core.Modules.Lobby;

public class LobbyModule : CoreModule<LobbyConfig>
{
    public override string Name { get; } = "Lobby";
    public override byte Priority { get; } = 15;

    private LobbySpawner _lobbySpawner;
    public static LobbyConfig LobbyConfig;
        
    public override void OnEnabled()
    {
        LobbyConfig = Config;
            
        _lobbySpawner = new LobbySpawner();

        Server.WaitingForPlayers += _lobbySpawner.OnWaitingForPlayers;
        Server.RoundStarted += _lobbySpawner.OnStarting;

        Player.TogglingOverwatch += _lobbySpawner.OnTogglingOverwatch;
        Player.ChangingRole += _lobbySpawner.OnChangingRole;
        Player.Spawning += _lobbySpawner.OnSpawning;
        Player.Verified += _lobbySpawner.OnVerified;

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Server.WaitingForPlayers -= _lobbySpawner.OnWaitingForPlayers;
        Server.RoundStarted -= _lobbySpawner.OnStarting;
            
        Player.TogglingOverwatch -= _lobbySpawner.OnTogglingOverwatch;
        Player.ChangingRole -= _lobbySpawner.OnChangingRole;
        Player.Spawning -= _lobbySpawner.OnSpawning;
        Player.Verified -= _lobbySpawner.OnVerified;

        _lobbySpawner = null;

        LobbyConfig = null;
            
        base.OnDisabled();
    }
}