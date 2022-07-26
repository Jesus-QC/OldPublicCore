using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Core.Features.Logger;
using Core.Loader.Features;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace Core.Loader;

public static class ModuleLoader
{
    public static SortedSet<ICoreModule<IConfig>> Modules { get; } = new (new PriorityComparer());
    public static HashSet<ICoreModule<IConfig>> EnabledModules { get; } = new ();

    public static void Load()
    {
        Directory.CreateDirectory(Path.Combine(Paths.Configs, $"Core {Server.Port}"));
        
        Log.Info("Loading modules");
        
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.IsAbstract || type.IsInterface)
            {
                continue;
            }
            
            if (!type.BaseType.IsGenericType || type.BaseType.GetGenericTypeDefinition() != typeof(CoreModule<>))
            {
                continue;
            }

            var ctr = type.GetConstructor(Type.EmptyTypes)!;
            var module = ctr.Invoke(null) as ICoreModule<IConfig>;

            ConfigManager.LoadConfig(module);

            Modules.Add(module!);
        }

        foreach (var module in Modules)
        {
            try
            {
                if (module.Config.IsEnabled)
                {
                    module.OnEnabled();
                    EnabledModules.Add(module);
                }
            }
            catch (Exception e)
            {
                Log.Error($"{LogUtils.GetBackgroundColor(LogColor.BrightRed)}{LogUtils.GetColor(LogColor.Black)}Module {module.Name} threw an exception while enabling:");
                Log.Error(e);
            }
        }
    }

    public static void UnLoad()
    {
        foreach (var module in EnabledModules)
        {
            module.OnDisabled();
        }
    }
}