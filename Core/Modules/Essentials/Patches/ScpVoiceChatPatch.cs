using System.Collections.Generic;
using System.Reflection.Emit;
using Assets._Scripts.Dissonance;
using HarmonyLib;
using NorthwoodLib.Pools;
using static HarmonyLib.AccessTools;

namespace Core.Modules.Essentials.Patches;

[HarmonyPatch(typeof(Radio), nameof(Radio.UserCode_CmdSyncTransmissionStatus))]
public class ScpVoiceChatPatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

        var lab = generator.DefineLabel();
        
        newInstructions.InsertRange(0, new []
        {
            new CodeInstruction(OpCodes.Ldsfld, Field(typeof(ScpVoiceChatPatch), nameof(AllowedScps))),
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(OpCodes.Ldfld, Field(typeof(Radio), nameof(Radio._hub))),
            new CodeInstruction(OpCodes.Ldfld, Field(typeof(ReferenceHub), nameof(ReferenceHub.characterClassManager))),
            new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(CharacterClassManager), nameof(CharacterClassManager.NetworkCurClass))),
            new CodeInstruction(OpCodes.Callvirt, Method(typeof(List<RoleType>), nameof(List<RoleType>.Contains))),
            new CodeInstruction(OpCodes.Brfalse_S, lab),
            new CodeInstruction(OpCodes.Ldarg_0),
            new CodeInstruction(OpCodes.Ldfld, Field(typeof(Radio), nameof(Radio._dissonanceSetup))),
            new CodeInstruction(OpCodes.Ldarg_1),
            new CodeInstruction(OpCodes.Callvirt, PropertySetter(typeof(DissonanceUserSetup), nameof(DissonanceUserSetup.MimicAs939))),
            new CodeInstruction(OpCodes.Nop).WithLabels(lab)
        });

        foreach (var instruction in newInstructions)
            yield return instruction;
        
        ListPool<CodeInstruction>.Shared.Return(newInstructions);
    }

    private static readonly List<RoleType> AllowedScps = new ()
    {
        RoleType.Scp049, RoleType.Scp096, RoleType.Scp106, RoleType.Scp173, RoleType.Scp0492, RoleType.Scp93953,
        RoleType.Scp93989
    };
}