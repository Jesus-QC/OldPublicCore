using System;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using HarmonyLib;
using Hints;

namespace Core.Features.Patches;

[HarmonyPatch(typeof(HintDisplay), nameof(HintDisplay.Show))]
public static class HintRedirect
{
    public static bool Prefix(HintDisplay __instance, Hint hint)
    {
        Type type = hint.GetType();
            
        if (type == typeof(TranslationHint))
            return false;
            
        if (type == typeof(TextHint))
        {
            TextHint t = hint as TextHint;
            Player.Get(__instance.gameObject).SendHint(ScreenZone.Center ,t.Text, t.DurationScalar);
            return false;
        }

        return true;
    }

   /* public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions,
        ILGenerator generator)
    {
        var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

        var typeLocal = generator.DeclareLocal(typeof(Type));
        
        newInstructions.InsertRange(0, new []
        {
            new CodeInstruction(OpCodes.Ldarg_1),
            new CodeInstruction(OpCodes.Box),
            new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(object), nameof(GetType))),
            new CodeInstruction(OpCodes.Stloc_S, typeLocal.LocalIndex),
            new CodeInstruction(OpCodes.Ldloc_S, typeLocal.LocalIndex),
            new CodeInstruction(Ldtoken, )

        });

        foreach (var instruction in newInstructions)
            yield return instruction;
        
        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }*/
}