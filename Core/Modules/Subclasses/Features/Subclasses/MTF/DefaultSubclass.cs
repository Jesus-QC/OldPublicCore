using System.Collections.Generic;
using Core.Features.Data.Enums;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Subclasses.MTF;

public class DefaultSubclass : Subclass
{
    public override string Name { get; set; } = "default";
    public override string Description { get; set; } = "You are not special, kinda sad.";
    public override CoreRarity Rarity { get; set; } = CoreRarity.Common;
    public override List<RoleType> AffectedRoles { get; set; } = new () { RoleType.NtfCaptain, RoleType.NtfPrivate, RoleType.NtfSergeant, RoleType.NtfSpecialist };
    public override RoleType SpawnAs { get; set; } = RoleType.None;
    public override Team Team { get; set; } = Team.MTF;
}