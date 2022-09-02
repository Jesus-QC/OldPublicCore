using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.ClassD;

public class HusterSubclass : Subclass
{
    public override string Name { get; set; } = "huster";
    public override string Color { get; set; } = "#ffdc42";
    public override string Description { get; set; } = "You like gold, you really like gold.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.ClassD };
    public override Team Team { get; set; } = Team.CDP;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.Coin, ItemType.Coin, ItemType.Coin
    };
}