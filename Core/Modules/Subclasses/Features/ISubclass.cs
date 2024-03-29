﻿using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace Core.Modules.Subclasses.Features;

public interface ISubclass
{
    string Name { get; set; }
    string Color { get; set; }
    string Description { get; set; }
    CoreRarity Rarity { get; set; }
    List<RoleType> AffectedRoles { get; set; }
    RoleType SpawnAs { get; set; }
    Team Team { get; set; }
    string CustomTeam { get; set; }
    RoleType SpawnLocation { get; set; }
    float DamageMultiplier { get; set; }
    List<ItemType> SpawnInventory { get; set; }
    Dictionary<ItemType, ushort> SpawnAmmo { get; set; }
    SubclassAbility Abilities { get; set; }
    
    IAbility MainAbility { get; set; }
    IAbility SecondaryAbility { get; set; }
    IAbility TertiaryAbility { get; set; }
    
    Vector3 Scale { get; set; }
    float Health { get; set; }
    float Ahp { get; set; }

    void OnSpawning(Player player);
    public string TopBar { get; set; }
    public string SecondaryTopBar { get; set; }
}