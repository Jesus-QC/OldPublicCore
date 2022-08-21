using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using Core.Features.Data.Configs;
using Core.Loader.Features;
using Exiled.API.Features;
using HarmonyLib;
using NorthwoodLib.Pools;

namespace Core.Modules.SuperProfiler;

public class SuperProfilerModule : CoreModule<EmptyConfig>
{
    public override string Name { get; } = "SuperProfiler";

    public override void OnEnabled()
    {
        base.OnEnabled();
        return;
        foreach (var type in typeof(ServerConsole).Module.GetTypes())
        {
            if(type.Name.Contains("Anon") || type.FullName.Contains("Utf8Json") || type.Name.Contains("Config") || type.Name.Contains("AbilityManager") || type.IsInterface || type.IsAbstract)
                continue;

            foreach (var method in type.GetMethods())
            {
                if (!method.IsDeclaredMember<MethodBase>() || method.IsGenericMethod)
                    continue;

                try
                {
                    Core.Harmony.Patch(method, null, null, new HarmonyMethod(AccessTools.Method(typeof(SuperProfilerModule), nameof(ProfilerPatch))));
                }
                catch
                {
                    // ignore
                }
            }
        }
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        base.OnDisabled();
    }

    private static IEnumerable<CodeInstruction> ProfilerPatch(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
    {
        var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

        var local = generator.DeclareLocal(typeof(Stopwatch));
        var refLoc = generator.DeclareLocal(typeof(TimeSpan));
        var ret = generator.DefineLabel();
        
        newInstructions.InsertRange(0, new []
        {
            new CodeInstruction(OpCodes.Newobj, AccessTools.GetDeclaredConstructors(typeof(Stopwatch))[0]),
            new CodeInstruction(OpCodes.Dup),
            new CodeInstruction(OpCodes.Stloc, local.LocalIndex),
            new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(Stopwatch), nameof(Stopwatch.Start))),
        });
        
        newInstructions.InsertRange(newInstructions.Count - 1, new []
        {
            new CodeInstruction(OpCodes.Ldloc, local.LocalIndex),
            new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(Stopwatch), nameof(Stopwatch.Stop))),
            new CodeInstruction(OpCodes.Ldloc, local.LocalIndex),
            new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Stopwatch), nameof(Stopwatch.ElapsedMilliseconds))),
            new CodeInstruction(OpCodes.Ldc_I4_7),
            new CodeInstruction(OpCodes.Conv_I8),
            new CodeInstruction(OpCodes.Blt_S, ret),
            new CodeInstruction(OpCodes.Ldloc, local.LocalIndex),
            new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Stopwatch), nameof(Stopwatch.Elapsed))),
            new CodeInstruction(OpCodes.Stloc, refLoc.LocalIndex),
            new CodeInstruction(OpCodes.Ldloca, refLoc.LocalIndex),
            new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(TimeSpan), nameof(TimeSpan.TotalMilliseconds))),
            new CodeInstruction(OpCodes.Box, typeof(double)),
            new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(object), nameof(object.ToString))),
            new CodeInstruction(OpCodes.Ldstr, "ms - " + original.ReflectedType!.FullName + "::" + original.Name),
            new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(string), nameof(string.Concat), new []{typeof(string), typeof(string)})),
            new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Log), nameof(Log.Info), new []{typeof(string)})),
        });
        
        newInstructions[newInstructions.Count - 1].labels.Add(ret);

        foreach (var instruction in newInstructions)
            yield return instruction;

        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
}