﻿using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.Guard;

public class BreacherSubclass : Subclass
{
    public override string Name { get; set; } = "breacher";
    public override string Color { get; set; } = "#42cef5";
    public override string Description { get; set; } = "You are ready for war.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Rare;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.FacilityGuard };
    public override RoleType SpawnAs { get; set; } = RoleType.FacilityGuard;
    public override Team Team { get; set; } = Team.MTF;

    public override List<ItemType> SpawnInventory { get; set; } = new()
    {
        ItemType.GunShotgun, ItemType.GrenadeFlash, ItemType.ArmorLight, ItemType.Medkit, ItemType.Radio,
        ItemType.KeycardGuard
    };

    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new()
    {
        [ItemType.Ammo9x19] = 100, [ItemType.Ammo556x45] = 100, [ItemType.Ammo762x39] = 100, [ItemType.Ammo12gauge] = 100,
        [ItemType.Ammo44cal] = 100
    };
}