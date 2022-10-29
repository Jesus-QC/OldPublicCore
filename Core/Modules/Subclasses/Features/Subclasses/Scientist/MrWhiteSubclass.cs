using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Abilities;
using Core.Modules.Subclasses.Features.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.Scientist;

public class MrWhiteSubclass : Subclass
{
    public override string Name { get; set; } = "mr. white";
    public override string Color { get; set; } = "#fff";
    public override string Description { get; set; } = "You are the Mr. White.\nYou can convert coins into medical items.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Legendary;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.Scientist };
    public override Team Team { get; set; } = Team.RSC;
    public override SubclassAbility Abilities { get; set; } = SubclassAbility.Druggist;
    public override IAbility MainAbility { get; set; } = new DruggistAbility();

    public override List<ItemType> SpawnInventory { get; set; } = new()
    {
        ItemType.KeycardScientist, ItemType.Medkit, ItemType.Coin, ItemType.Coin, ItemType.Coin
    };
}