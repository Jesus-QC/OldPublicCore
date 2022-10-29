using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.ClassD;

public class HalloweenEnjoyer : Subclass
{
    public override string Name { get; set; } = "halloween enjoyer";
    public override string Description { get; set; } = "You love halloween.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new () { RoleType.ClassD };
    public override RoleType SpawnAs { get; set; } = RoleType.None;
    public override Team Team { get; set; } = Team.CDP;
    public override string Color { get; set; } = "#e87f2e";
    public override List<ItemType> SpawnInventory { get; set; } = new() { ItemType.SCP330 };
}