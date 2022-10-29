using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.Scientist;

public class EngineerSubclass : Subclass
{
    public override string Name { get; set; } = "engineer";
    public override string Color { get; set; } = "#eb5e34";
    public override string Description { get; set; } = "You are the engineer of the facility.\nTake control of the situation.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Legendary;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.Scientist };
    public override Team Team { get; set; } = Team.RSC;

    public override List<ItemType> SpawnInventory { get; set; } = new()
    {
        ItemType.GunFSP9, ItemType.KeycardContainmentEngineer, ItemType.Radio, ItemType.ArmorLight, ItemType.Medkit,
        ItemType.GrenadeFlash, ItemType.GrenadeHE
    };

    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new ()
    {
        [ItemType.Ammo9x19] = 60
    };
}