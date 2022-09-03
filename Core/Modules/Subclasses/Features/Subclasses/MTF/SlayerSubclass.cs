using System.Collections.Generic;
using Core.Features.Attribute;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

[DisabledFeature]
public class SlayerSubclass : Subclass
{
    public override string Name { get; set; } = "slayer";
    public override string Description { get; set; } = "You are the slayer.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Epic;
    public override List<RoleType> AffectedRoles { get; set; } = new () { RoleType.NtfCaptain };
    public override RoleType SpawnAs { get; set; } = RoleType.None;
    public override Team Team { get; set; } = Team.MTF;
}