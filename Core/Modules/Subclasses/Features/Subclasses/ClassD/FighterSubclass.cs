using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.ClassD;

public class FighterSubclass : Subclass
{
    public override string Name { get; set; } = "fighter";
    public override string Color { get; set; } = "#ff6363";
    public override string Description { get; set; } = "You are so good at shooting, your damage is multiplied x2.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Rare;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.ClassD };
    public override RoleType SpawnAs { get; set; } = RoleType.ClassD;
    public override Team Team { get; set; } = Team.CDP;
    public override float DamageMultiplier { get; set; } = 2;
}