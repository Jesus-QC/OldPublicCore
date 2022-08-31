using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

public class TankSubclass : Subclass
{
    public override string Name { get; set; } = "tank";
    public override string Color { get; set; } = "#4287f5";
    public override string Description { get; set; } = "You are the tank of your unit.\nHelp others with.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Rare;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.NtfPrivate, RoleType.NtfSergeant, RoleType.NtfSpecialist };
    public override Team Team { get; set; } = Team.MTF;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    { ItemType.Radio, ItemType.KeycardNTFCommander, ItemType.ArmorHeavy, ItemType.Medkit, ItemType.Medkit, ItemType.Medkit, ItemType.Medkit, ItemType.Medkit };
    
    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new Dictionary<ItemType, ushort>()
    {
        [ItemType.Ammo9x19] = 100, [ItemType.Ammo556x45] = 100, [ItemType.Ammo762x39] = 100, [ItemType.Ammo12gauge] = 100,
        [ItemType.Ammo44cal] = 100
    };
}