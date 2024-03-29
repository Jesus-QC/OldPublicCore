﻿using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Enums;
using Core.Modules.Subclasses.Features.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;

namespace Core.Modules.Subclasses.Features.Subclasses.Scientist;

public class ChaosSpySubclass : Subclass
{
    public override string Name { get; set; } = "chaos spy";
    public override string Color { get; set; } = "#c4fff6";
    public override string Description { get; set; } = "You are an infiltred chaos, help class-ds.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Epic;

    public override List<RoleType> AffectedRoles { get; set; } = new ()
    {
        RoleType.Scientist
    };

    public override RoleType SpawnLocation { get; set; } = RoleType.Scientist;
    public override RoleType SpawnAs { get; set; } = RoleType.ChaosConscript;
    public override Team Team { get; set; } = Team.CHI;
    public override float Ahp { get; set; } = 20;

    public override SubclassAbility Abilities { get; set; } = SubclassAbility.Disguised;

    public override void OnSpawning(Player player)
    {
        Timing.CallDelayed(1, () => player.Disguise(RoleType.Scientist, new HashSet<Side> {Side.ChaosInsurgency, Side.Scp}));
    }
}