using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

public class SniperSubclass : Subclass
{
    public override string Name { get; set; } = "sniper";
    public override string Color { get; set; } = "#000";
    public override string Description { get; set; } = "You are the most professional shooter of the unit.\nYou have a very good precision.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.NtfPrivate, RoleType.NtfSergeant, RoleType.NtfSpecialist, RoleType.NtfCaptain };
    public override Team Team { get; set; } = Team.MTF;

    public override List<ItemType> SpawnInventory { get; set; } = new()
    {
        ItemType.GunE11SR, ItemType.GunRevolver, ItemType.GrenadeFlash,
        ItemType.KeycardNTFLieutenant, ItemType.Radio, ItemType.ArmorCombat
    };
    
    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new()
    {
        [ItemType.Ammo9x19] = 40, [ItemType.Ammo556x45] = 160, [ItemType.Ammo44cal] = 40
    };
}