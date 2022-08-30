using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.Guard;

public class DefaultSubclass : Subclass
{
    public override string Name { get; set; } = "default";
    public override string Description { get; set; } = "You are not special, kinda sad.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new List<RoleType>() { RoleType.FacilityGuard };
    public override RoleType SpawnAs { get; set; } = RoleType.FacilityGuard;
    public override Team Team { get; set; } = Team.MTF;
}