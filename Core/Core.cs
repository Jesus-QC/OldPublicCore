using System.Threading.Tasks;
using HarmonyLib;
using Exiled.API.Enums;
using Core.Features.Data;
using Exiled.API.Features;
using Core.Features.Handlers;
using Core.Features.Data.Configs;
using Core.Features.Wrappers;
using Core.Loader;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;
using CorePlayer = Core.Features.Events.CoreEvents;

namespace Core;

public class Core : Plugin<EmptyConfig>
{
    public override string Name { get; } = "Core";
    public override string Prefix { get; } = "core";
    public override string Author { get; } = "Jesus-QC";
    public override PluginPriority Priority { get; } = PluginPriority.Last;

    public const string GlobalVersion = "1.0.2.9";
        
    public static Database Database;
    public static Harmony Harmony;

    private PlayerHandler _playerHandler;
    private ServerHandler _serverHandler;

    public override void OnEnabled()
    {
        Harmony = new Harmony("com.core.patches");
        Harmony.PatchAll();

        Database = new Database();
            
        RegisterEvents();
            
        ModuleLoader.Load();
        
        Task.Run(ServerCore.CheckTps);

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        ModuleLoader.UnLoad();
            
        UnregisterEvents();
            
        Database = null;
            
        Harmony.UnpatchAll(Harmony.Id);
        Harmony = null;

        base.OnDisabled();
    }

    private void RegisterEvents()
    {
        _playerHandler = new PlayerHandler();
        _serverHandler = new ServerHandler();

        Player.Verified += _playerHandler.OnVerified;
        Player.Left += _playerHandler.OnLeft;

        Server.RestartingRound += _serverHandler.OnRestartingRound;
    }
        
    private void UnregisterEvents()
    {
        Player.Verified -= _playerHandler.OnVerified;
        Player.Left -= _playerHandler.OnLeft;

        Server.RestartingRound -= _serverHandler.OnRestartingRound;

        _serverHandler = null;
        _playerHandler = null;
    }
}