using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Enums;
using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace Core.Modules.Subclasses.Features;

public abstract class Subclass : ISubclass
{
    public int Id;

    public virtual string Name { get; set; } = string.Empty;
    public virtual string Color { get; set; } = null;
    public virtual string Description { get; set; } = string.Empty;
    public virtual CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public virtual List<RoleType> AffectedRoles { get; set; } = new ();
    public virtual RoleType SpawnAs { get; set; } = RoleType.None;
    public virtual Team Team { get; set; } = Team.RIP;
    public virtual string CustomTeam { get; set; } = string.Empty;
    public virtual RoleType SpawnLocation { get; set; } = RoleType.None;
    public virtual float DamageMultiplier { get; set; } = 1;
    public virtual List<ItemType> SpawnInventory { get; set; } = null;
    public virtual Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = null;
    public virtual SubclassAbility Abilities { get; set; } = SubclassAbility.None;
    public virtual SubclassAbility CommandAbilities { get; set; } = SubclassAbility.None;
    public virtual Vector3 Scale { get; set; } = Vector3.one;
    public virtual float Health { get; set; } = -1;
    public virtual float Ahp { get; set; } = -1;
    
    public virtual void OnSpawning(Player player) { }
    
    public string TopBar { get; set; }
    public string SecondaryTopBar { get; set; }
}