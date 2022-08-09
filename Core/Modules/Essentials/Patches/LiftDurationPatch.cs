using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using NorthwoodLib.Pools;

using static HarmonyLib.AccessTools;

namespace Core.Modules.Essentials.Patches;

[HarmonyPatch(typeof(Lift), nameof(Lift.UseLift))]
public class LiftDurationPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions,
        ILGenerator generator)
    {
        var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
        newInstructions.InsertRange(0, new CodeInstruction[]
        {
            new (OpCodes.Ldarg_0),
            new (OpCodes.Ldc_R4, 7f),
            new (OpCodes.Stfld, Field(typeof(Lift), nameof(Lift.movingSpeed))),
        });

        foreach (var ins in newInstructions)
            yield return ins;
       
        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }
}