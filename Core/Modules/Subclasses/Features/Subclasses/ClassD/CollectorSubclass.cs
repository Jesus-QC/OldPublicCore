using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.ClassD;

public class CollectorSubclass : Subclass
{
    public override string Name { get; set; } = "collector";
    public override string Color { get; set; } = "#c2ffb7";
    public override string Description { get; set; } = "You love picking up any item you find in the facility.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Rare;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.ClassD };
    public override RoleType SpawnAs { get; set; } = RoleType.ClassD;
    public override Team Team { get; set; } = Team.CDP;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.KeycardJanitor, ItemType.Medkit, ItemType.Flashlight, ItemType.Painkillers, ItemType.Adrenaline
    };
}