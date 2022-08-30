using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.Guard;

public class GrenadierSubclass : Subclass
{
    public override string Name { get; set; } = "grenadier";
    public override string Color { get; set; } = "#57fa88";
    public override string Description { get; set; } = "You are very ready for war.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Epic;
    public override List<RoleType> AffectedRoles { get; set; } = new List<RoleType>() { RoleType.FacilityGuard };
    public override RoleType SpawnAs { get; set; } = RoleType.FacilityGuard;
    public override Team Team { get; set; } = Team.MTF;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.GunShotgun, ItemType.GrenadeFlash, ItemType.ArmorLight, ItemType.Medkit, ItemType.Radio,
        ItemType.KeycardGuard
    };

    public override SubclassAbility Abilities { get; set; } = SubclassAbility.GrenadeImmunity;
}