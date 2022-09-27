using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Enums;
using Core.Modules.Subclasses.Features.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;

namespace Core.Modules.Subclasses.Features.Subclasses.Chaos;

public class PeaceBreakerSubclass : Subclass
{
    public override string Name { get; set; } = "peace breaker spy";
    public override string Color { get; set; } = "#4e9eed";
    public override string Description { get; set; } = "You are an infiltrated mtf, make chaos and SCPs fight";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Mythic;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.ChaosConscript, RoleType.ChaosMarauder, RoleType.ChaosRepressor, RoleType.ChaosRifleman };
    public override Team Team { get; set; } = Team.MTF;

    public override SubclassAbility Abilities { get; set; } = SubclassAbility.Disguised;
    public override RoleType SpawnAs { get; set; } = RoleType.NtfPrivate;
    public override RoleType SpawnLocation { get; set; } = RoleType.ChaosConscript;
    
    public override void OnSpawning(Player player)
    {
        Timing.CallDelayed(1, () => player.Disguise(RoleType.ChaosConscript, new HashSet<Side> {Side.Mtf}));
    }
}