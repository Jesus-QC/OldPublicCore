using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

public class BomberSubclass : Subclass
{
    public override string Name { get; set; } = "bomber";
    public override string Color { get; set; } = "#ff0f0f";
    public override string Description { get; set; } = "You are the bomb.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Epic;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.NtfPrivate, RoleType.NtfSergeant, RoleType.NtfSpecialist };
    public override Team Team { get; set; } = Team.MTF;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.KeycardNTFLieutenant, ItemType.Radio, ItemType.GunFSP9, ItemType.Adrenaline, ItemType.GrenadeHE, ItemType.GrenadeHE, ItemType.GrenadeFlash, ItemType.GrenadeFlash
    };

    public override SubclassAbility Abilities { get; set; } = SubclassAbility.GrenadeImmunity;
}