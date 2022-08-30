using System.Collections.Generic;
using Core.Features.Data.Enums;
using Exiled.API.Features;
using MEC;

namespace Core.Modules.Subclasses.Features.Subclasses.Scientist;

public class RunnerSubclass : Subclass
{
    public override string Name { get; set; } = "runner";
    public override string Color { get; set; } = "#c4fff6";
    public override string Description { get; set; } = "You have researched about human lungs and discovered a lot of things.\nYou have infinite stamina.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;

    public override List<RoleType> AffectedRoles { get; set; } = new ()
    {
        RoleType.Scientist
    };

    public override RoleType SpawnAs { get; set; } = RoleType.Scientist;
    public override Team Team { get; set; } = Team.RSC;
    public override float Ahp { get; set; } = 20;

    public override void OnSpawning(Player player)
    {
        Timing.CallDelayed(1, () => player.IsUsingStamina = false);
    }
}