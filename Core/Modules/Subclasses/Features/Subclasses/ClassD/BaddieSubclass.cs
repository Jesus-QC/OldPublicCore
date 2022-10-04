using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Modules.Subclasses.Features.Abilities;
using Core.Modules.Subclasses.Features.Enums;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Subclasses.ClassD;

public class BaddieSubclass : Subclass
{
    public override string Name { get; set; } = "baddie";
    public override string Color { get; set; } = "#000";
    public override string Description { get; set; } = "You are a baddie, you can steal from humans.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Rare;
    public override List<RoleType> AffectedRoles { get; set; } = new() { RoleType.ClassD };
    public override Team Team { get; set; } = Team.CDP;
    public override Vector3 Scale { get; set; } = Vector3.one;
    public override List<ItemType> SpawnInventory { get; set; } = new () { ItemType.Medkit };
    public override SubclassAbility Abilities { get; set; } = SubclassAbility.Pickpocket;
    public override IAbility MainAbility { get; set; } = new PickPocketAbility();
}