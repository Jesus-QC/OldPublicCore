using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using NorthwoodLib.Pools;
using static HarmonyLib.AccessTools;

namespace Core.Modules.Essentials.Patches;

[HarmonyPatch(typeof(NicknameSync), nameof(NicknameSync.CombinedName), MethodType.Getter)]
public class RaNamesPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent();
        
        newInstructions.AddRange(new CodeInstruction[]
        {
            new (OpCodes.Ldarg_0),
            new (OpCodes.Callvirt, PropertyGetter(typeof(NicknameSync), nameof(NicknameSync.MyNick))),
            new (OpCodes.Ret)
        });
        
        foreach (CodeInstruction instruction in newInstructions)
            yield return instruction;

        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
}