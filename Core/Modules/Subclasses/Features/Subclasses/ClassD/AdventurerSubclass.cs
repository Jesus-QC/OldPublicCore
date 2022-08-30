using System.Collections.Generic;
using Core.Features.Data.Enums;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace Core.Modules.Subclasses.Features.Subclasses.ClassD;

public class AdventurerSubclass : Subclass
{
    public override string Name { get; set; } = "adventurer";
    public override string Color { get; set; } = "#80d0c4";
    public override string Description { get; set; } = "You enjoy the adventure!\nYou like to play with others and you feel confident.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new () { RoleType.ClassD };
    public override RoleType SpawnAs { get; set; } = RoleType.ClassD;
    public override Team Team { get; set; } = Team.CDP;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.Coin, ItemType.Coin, ItemType.Coin
    };
}