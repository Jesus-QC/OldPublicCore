using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

public class ReconSubclass : Subclass
{
    public override string Name { get; set; } = "recon";
    public override string Color { get; set; } = "#c8e9f7";
    public override string Description { get; set; } = "Your mission is to recontain all the entities.\nEveryone trusts you.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.NtfPrivate, RoleType.NtfSergeant, RoleType.NtfSpecialist };
    public override Team Team { get; set; } = Team.MTF;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.GunE11SR, ItemType.Flashlight, ItemType.GrenadeFlash, ItemType.KeycardNTFLieutenant, ItemType.Radio,
        ItemType.ArmorCombat, ItemType.Medkit
    };
    
    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new Dictionary<ItemType, ushort>()
    {
        [ItemType.Ammo9x19] = 100, [ItemType.Ammo556x45] = 100, [ItemType.Ammo762x39] = 100, [ItemType.Ammo12gauge] = 0,
        [ItemType.Ammo44cal] = 100
    };
}