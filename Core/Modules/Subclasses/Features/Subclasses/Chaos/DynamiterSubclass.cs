using System.Collections.Generic;
using Core.Features.Data.Enums;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Subclasses.Chaos;

public class DynamiterSubclass : Subclass
{
    public override string Name { get; set; } = "dynamiter";
    public override string Color { get; set; } = "#e84f4f";
    public override string Description { get; set; } = "You are a dynamiter.\nYou like to explode everything.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Rare;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.ChaosConscript, RoleType.ChaosMarauder, RoleType.ChaosRepressor, RoleType.ChaosRifleman };
    public override Team Team { get; set; } = Team.CHI;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.KeycardChaosInsurgency, ItemType.GunLogicer, ItemType.ArmorHeavy, ItemType.GrenadeHE, ItemType.GrenadeHE, ItemType.GrenadeHE, ItemType.GrenadeHE
    };
    
    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new Dictionary<ItemType, ushort>()
    {
        [ItemType.Ammo9x19] = 30, [ItemType.Ammo556x45] = 20, [ItemType.Ammo762x39] = 200, [ItemType.Ammo12gauge] = 0,
        [ItemType.Ammo44cal] = 20
    };

    public override Vector3 Scale { get; set; } = new Vector3(1.1f, 1.1f, 1.1f);
    public override float Ahp { get; set; } = 50;
}