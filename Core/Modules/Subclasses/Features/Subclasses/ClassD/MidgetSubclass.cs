using System.Collections.Generic;
using Core.Features.Data.Enums;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Subclasses.ClassD;

public class MidgetSubclass : Subclass
{
    public override string Name { get; set; } = "midget";
    public override string Color { get; set; } = "#aaa";
    public override string Description { get; set; } = "You are short, use this perk correctly!\nOh, but you have less health.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Rare;
    public override List<RoleType> AffectedRoles { get; set; } = new List<RoleType>() { RoleType.ClassD };
    public override RoleType SpawnAs { get; set; } = RoleType.ClassD;
    public override Team Team { get; set; } = Team.CDP;
    public override Vector3 Scale { get; set; } = new (1.1f, 0.6f, 1.1f);
    public override float Health { get; set; } = 25f;
}