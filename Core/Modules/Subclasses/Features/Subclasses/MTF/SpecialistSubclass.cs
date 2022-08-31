using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

public class SpecialistSubclass : Subclass
{
    public override string Name { get; set; } = "specialist";
    public override string Color { get; set; } = "#000";
    public override string Description { get; set; } = "You are the specialist of your recontainment unit.\nRecontain all the anomalies.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Epic;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.NtfSpecialist };
    public override Team Team { get; set; } = Team.MTF;
    public override float Ahp { get; set; } = 50;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.MicroHID,
        ItemType.GunShotgun,
        ItemType.GrenadeHE,
        ItemType.Flashlight,
        ItemType.KeycardNTFLieutenant,
        ItemType.Radio,
        ItemType.ArmorHeavy,
    };
    
    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new Dictionary<ItemType, ushort>()
    {
        [ItemType.Ammo9x19] = 20, [ItemType.Ammo556x45] = 30, [ItemType.Ammo762x39] = 20, [ItemType.Ammo12gauge] = 100,
        [ItemType.Ammo44cal] = 20
    };
}