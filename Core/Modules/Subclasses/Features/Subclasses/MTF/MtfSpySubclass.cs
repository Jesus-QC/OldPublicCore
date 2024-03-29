﻿using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Enums;
using Core.Modules.Subclasses.Features.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

public class MtfSpySubclass : Subclass
{
    public override string Name { get; set; } = "chaos spy";
    public override string Color { get; set; } = "#49a5f5";
    public override string Description { get; set; } = "You are an infiltrated chaos, help class-ds.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Legendary;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.NtfPrivate, RoleType.NtfSergeant, RoleType.NtfSpecialist };
    public override Team Team { get; set; } = Team.CHI;

    public override SubclassAbility Abilities { get; set; } = SubclassAbility.Disguised;
    public override RoleType SpawnAs { get; set; } = RoleType.ChaosMarauder;
    public override RoleType SpawnLocation { get; set; } = RoleType.NtfPrivate;
    
    public override void OnSpawning(Player player)
    {
        Timing.CallDelayed(1, () => player.Disguise(RoleType.NtfPrivate, new HashSet<Side> {Side.ChaosInsurgency, Side.Scp}));
    }
}