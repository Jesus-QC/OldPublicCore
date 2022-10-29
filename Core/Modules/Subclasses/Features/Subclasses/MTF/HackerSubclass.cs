using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

public class HackerSubclass : Subclass
{
    public override string Name { get; set; } = "hacker";
    public override string Color { get; set; } = "#34eb5e";
    public override string Description { get; set; } = "You are one of the hackers on the unit.\nEnjoy your powers";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.NtfPrivate, RoleType.NtfSergeant, RoleType.NtfSpecialist };
    public override RoleType SpawnAs { get; set; } = RoleType.None;
    public override Team Team { get; set; } = Team.MTF;

    public override List<ItemType> SpawnInventory { get; set; } = new()
    {
        ItemType.GunCrossvec, ItemType.KeycardChaosInsurgency, ItemType.Medkit,
        ItemType.Adrenaline, ItemType.ArmorLight, ItemType.Radio,
    };

    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new()
    {
        [ItemType.Ammo9x19] = 120, [ItemType.Ammo556x45] = 40
    };
}