using System.Collections.Generic;
using Core.Features.Data.Enums;
using Exiled.API.Enums;

namespace Core.Modules.Subclasses.Features.Structs.Subclasses;

public class Subclass
{
    public int Id = 0;
    public string Name { get; set; }
    public string Description { get; set; }
    public CoreRarity Rarity { get; set; }
    public List<RoleType> AffectedRoles { get; set; }
    public RoleType SpawnAs { get; set; }
    public Team Team { get; set; }
    public List<RoomType> SpawnLocations { get; set; }
    
    public int WaitForSpawnWaves { get; set; }
    public uint MaxAlive { get; set; }
    
    public float Health { get; set; }
    public float Armor { get; set; }
    public double DamageMultiplier { get; set; }
    
    public List<ItemType> Inventory { get; set; }
    public Dictionary<AmmoType, uint> Ammo { get; set; }
    
    public List<Abilities.Ability> Abilities { get; set; }
    public Dictionary<Abilities.Ability, object> AbilityConfig { get; set; }
}