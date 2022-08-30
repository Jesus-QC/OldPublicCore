using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.Chaos;

public class ChaosJuggernautSubclass : Subclass
{
    public override string Name { get; set; } = "juggernaut";
    public override string Color { get; set; } = "#99ffb8";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Epic;
    public override List<RoleType> AffectedRoles { get; set; } = new () { RoleType.ChaosConscript, RoleType.ChaosMarauder, RoleType.ChaosRepressor, RoleType.ChaosRifleman };
    public override RoleType SpawnAs { get; set; } = RoleType.None;
    public override Team Team { get; set; } = Team.CHI;
    public override float Health { get; set; } = 200;
    public override float Ahp { get; set; } = 100;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.GunLogicer, ItemType.ArmorCombat, ItemType.KeycardChaosInsurgency, ItemType.Flashlight,
        ItemType.GrenadeHE, ItemType.GrenadeFlash
    };
}