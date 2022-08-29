using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.Scientist;

public class DefaultSubclass : Subclass
{
    public override string Name { get; set; } = "Default";
    public override string Description { get; set; } = "You are not special, that is kinda sad.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new List<RoleType>() { RoleType.Scientist };
    public override RoleType SpawnAs { get; set; } = RoleType.Scientist;
    public override Team Team { get; set; } = Team.RSC;
}