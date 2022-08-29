using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Enums;
using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace Core.Modules.Subclasses.Features;

public interface ISubclass
{
    string Name { get; set; }
    string Description { get; set; }
    CoreRarity Rarity { get; set; }
    List<RoleType> AffectedRoles { get; set; }
    RoleType SpawnAs { get; set; }
    Team Team { get; set; }
    string CustomTeam { get; set; }
    List<RoomType> SpawnLocations { get; set; }
    double DamageMultiplier { get; set; }
    List<ItemType> SpawnInventory { get; set; }
    Dictionary<AmmoType, uint> SpawnAmmo { get; set; }
    List<SubclassAbility> Abilities { get; set; }
    Vector3 Scale { get; set; }
    float Health { get; set; }
    float Ahp { get; set; }

    void OnSpawning(Player player);
}