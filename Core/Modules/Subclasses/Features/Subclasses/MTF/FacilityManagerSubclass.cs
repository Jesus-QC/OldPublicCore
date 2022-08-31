﻿using System.Collections.Generic;
using Core.Features.Data.Enums;
using Exiled.API.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

public class FacilityManagerSubclass : Subclass
{
    public override string Name { get; set; } = "facility manager";
    public override string Color { get; set; } = "#85bcd4";
    public override string Description { get; set; } = "You are the manager of the facility, help your team contain the breach.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Rare;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.NtfPrivate, RoleType.NtfSergeant, RoleType.NtfSpecialist };
    public override Team Team { get; set; } = Team.MTF;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.Flashlight, ItemType.Radio, ItemType.Medkit, ItemType.GrenadeHE, ItemType.KeycardFacilityManager,
        ItemType.ArmorLight, ItemType.GunRevolver
    };

    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new Dictionary<ItemType, ushort>()
    {
        [ItemType.Ammo9x19] = 80, [ItemType.Ammo12gauge] = 100, [ItemType.Ammo556x45] = 200, [ItemType.Ammo44cal] = 28,
        [ItemType.Ammo762x39] = 80
    };
}