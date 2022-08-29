﻿using System.Collections.Generic;
using Core.Features.Data.Enums;

namespace Core.Modules.Subclasses.Features.Subclasses.Scientist;

public class InsiderSubclass : Subclass
{
    public override string Name { get; set; } = "Insider";
    public override string Description { get; set; } = "You are a scientist of high rank.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Rare;
    public override List<RoleType> AffectedRoles { get; set; } = new List<RoleType>() { RoleType.Scientist };
    public override RoleType SpawnAs { get; set; } = RoleType.Scientist;
    public override Team Team { get; set; } = Team.RSC;

    public override List<ItemType> SpawnInventory { get; set; } = new List<ItemType>()
    {
        ItemType.Flashlight, ItemType.KeycardResearchCoordinator, ItemType.Medkit, ItemType.Coin
    };
}