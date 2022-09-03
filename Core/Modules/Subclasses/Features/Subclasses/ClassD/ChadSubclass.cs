using System.Collections.Generic;
using Core.Features.Data.Enums;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Subclasses.ClassD;

public class ChadSubclass : Subclass
{
    public override string Name { get; set; } = "chad";
    public override string Color { get; set; } = "#000";
    public override string Description { get; set; } = "You are a Chad.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Rare;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.ClassD };
    public override Team Team { get; set; } = Team.CDP;
    public override float Health { get; set; } = 175;
    public override Vector3 Scale { get; set; } = Vector3.one * 1.1f;
    public override List<ItemType> SpawnInventory { get; set; } = new () { ItemType.Medkit, ItemType.Flashlight };
}