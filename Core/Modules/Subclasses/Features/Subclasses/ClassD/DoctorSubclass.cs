using System.Collections.Generic;
using Core.Features.Attribute;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.ClassD;

[DisabledFeature]
public class DoctorSubclass : Subclass
{
    public override string Name { get; set; } = "Doctor";
    public override string Description { get; set; } = "You were once a medical student.\nYou can heal allies without items.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Rare;
    public override List<RoleType> AffectedRoles { get; set; } = new List<RoleType>() { RoleType.ClassD };
    public override RoleType SpawnAs { get; set; } = RoleType.ClassD;
    public override Team Team { get; set; } = Team.CDP;

    public override List<ItemType> SpawnInventory { get; set; } = new ()
    {
        ItemType.Medkit, ItemType.Painkillers, ItemType.Adrenaline
    };

    public override List<SubclassAbility> Abilities { get; set; } = new List<SubclassAbility>()
    {
        SubclassAbility.HealCmd
    };
}