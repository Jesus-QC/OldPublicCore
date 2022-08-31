using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

public class EngineerSubclass : Subclass
{
    public override string Name { get; set; } = "engineer";
    public override string Color { get; set; } = "#eb5e34";
    public override string Description { get; set; } = "You are the engineer of the facility.\nTake control of the situation.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.NtfPrivate, RoleType.NtfSergeant, RoleType.NtfSpecialist };
    public override Team Team { get; set; } = Team.MTF;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.GunFSP9, ItemType.KeycardResearchCoordinator, ItemType.Radio, ItemType.ArmorLight, ItemType.Medkit,
        ItemType.GrenadeFlash, ItemType.GrenadeHE
    };
}