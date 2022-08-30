using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.ClassD;

public class CleanerSubclass : Subclass
{
    public override string Name { get; set; } = "cleaner";
    public override string Color { get; set; } = "#ffb7f7";
    public override string Description { get; set; } = "You have more trust than other class-ds\n you have a Janitor's KeyCard.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.ClassD };
    public override RoleType SpawnAs { get; set; } = RoleType.ClassD;
    public override Team Team { get; set; } = Team.CDP;
    public override List<ItemType> SpawnInventory { get; set; } = new()
    {
        ItemType.KeycardJanitor
    };
}