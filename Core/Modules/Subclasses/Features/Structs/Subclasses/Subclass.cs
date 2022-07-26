using System.Collections.Generic;
using Exiled.API.Enums;

namespace Core.Modules.Subclasses.Features.Structs.Subclasses
{
    public class Subclass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Rarity Rarity { get; set; }
        public List<RoleType> AffectedRoles { get; set; }
        public int WaitForSpawnWaves { get; set; }
        public uint MaxAlive { get; set; }
        public RoleType SpawnAs { get; set; }
        public List<RoomType> SpawnLocations { get; set; }
        public Team EndsRoundWith { get; set; }

        public float Health { get; set; }
        public float Armor { get; set; }
        public double DamageMultiplier { get; set; }
        public Dictionary<AmmoType, uint> Ammo { get; set; }
        
        public bool CanTakeFf { get; set; }
        public bool CanGiveFf { get; set; }
        public List<RoleType> RolesThatCantDamage { get; set; }

        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float ScaleZ { get; set; }
        
        public Inventory.Inventory Inventory { get; set; }
        
        public List<Abilities.Ability> Abilities { get; set; }
        public Dictionary<Abilities.Ability, object> AbilityConfig { get; set; }
    }
}