using System;
using System.IO;
using Core.Features.Logger;
using Core.Loader.Features;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using ELoader = Exiled.Loader.Loader;
using EConfigManager = Exiled.Loader.ConfigManager;

namespace Core.Loader;

public static class ConfigManager
{
    private static string GetPath(this ICoreModule<IConfig> module) => Path.Combine(Paths.Configs, $"Core {Server.Port}", module.Name.ToLowerInvariant() + ".yml");
    
    public static void LoadConfig(ICoreModule<IConfig> module)
    {
        var path = GetPath(module);
        if (!File.Exists(path))
        {
            Log.Warn($"{LogUtils.GetBackgroundColor(LogColor.Magenta)}{LogUtils.GetColor(LogColor.White)}{module.Name} doesn't have default configs, generating...");
            File.WriteAllText(path, ELoader.Serializer.Serialize(module.Config));
            return;
        }

        try
        {
            module.Config.CopyProperties((IConfig)ELoader.Deserializer.Deserialize(File.ReadAllText(path), module.Config.GetType()));
            File.WriteAllText(path, ELoader.Serializer.Serialize(module.Config));
        }
        catch (Exception e)
        {
            Log.Error($"{LogUtils.GetBackgroundColor(LogColor.BrightRed)}{LogUtils.GetColor(LogColor.Black)}Module {module.Name} configs could not be read.");
            Log.Error(e);
        }
    }
}