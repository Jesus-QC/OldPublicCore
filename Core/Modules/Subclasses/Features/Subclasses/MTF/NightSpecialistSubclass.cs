using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

public class NightSpecialistSubclass : Subclass
{
    public override string Name { get; set; } = "night specialist";
    public override string Color { get; set; } = "#c8e9f7";
    public override string Description { get; set; } = "Your mission is to recontain all the entities.\nEveryone trusts in you.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.NtfCaptain };
    public override Team Team { get; set; } = Team.MTF;

    public override List<ItemType> SpawnInventory { get; set; } = new()
    {
        ItemType.GunAK, ItemType.Medkit, ItemType.Painkillers, ItemType.KeycardNTFCommander,
        ItemType.ArmorCombat, ItemType.Radio, ItemType.GrenadeFlash
    };
    
    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new()
    {
        [ItemType.Ammo762x39] = 100
    };
}