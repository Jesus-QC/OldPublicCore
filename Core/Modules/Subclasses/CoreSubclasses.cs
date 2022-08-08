using System;
using Core.Modules.Subclasses.Features;
using Core.Modules.Subclasses.Features.Handlers;
using Exiled.API.Features;

namespace Core.Modules.Subclasses;

public class CoreSubclasses : Plugin<Config>
{
    public override string Name { get; } = "Subclasses";
    public override string Prefix { get; } = "subclasses";
    public override string Author { get; } = "Jesus-QC";
    public override Version Version { get; } = new(1, 0, 0);

    public static Config PluginConfig { get; private set; }
    public static SubclassesManager SubclassesManager { get; private set; }
    public static PlayerHandler PlayerManager { get; private set; }
    public static ServerHandler ServerHandler { get; private set; }

    public override void OnEnabled()
    {
        PluginConfig = Config;
            
        Paths.Load();

        SubclassesManager = new SubclassesManager();
        SubclassesManager.Load();
            
        PlayerManager = new PlayerHandler();

        ServerHandler = new ServerHandler();

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
            
            
            
        base.OnDisabled();
    }
}