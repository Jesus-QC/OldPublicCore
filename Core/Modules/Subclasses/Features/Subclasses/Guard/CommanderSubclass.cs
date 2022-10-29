using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.Guard;

public class CommanderSubclass : Subclass
{
    public override string Name { get; set; } = "commander";
    public override string Color { get; set; } = "#4e66ed";
    public override string Description { get; set; } = "You are a guard commander.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Mythic;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.FacilityGuard };
    public override RoleType SpawnAs { get; set; } = RoleType.FacilityGuard;
    public override Team Team { get; set; } = Team.MTF;

    public override List<ItemType> SpawnInventory { get; set; } = new()
    {
        ItemType.GunE11SR, ItemType.GunLogicer, ItemType.GrenadeHE, ItemType.Medkit, ItemType.ArmorLight, ItemType.Radio, ItemType.KeycardNTFLieutenant
    };

    public override Dictionary<ItemType, ushort> SpawnAmmo { get; set; } = new()
    {
        [ItemType.Ammo9x19] = 100, [ItemType.Ammo556x45] = 100, [ItemType.Ammo762x39] = 100, [ItemType.Ammo12gauge] = 100,
        [ItemType.Ammo44cal] = 100
    };
}