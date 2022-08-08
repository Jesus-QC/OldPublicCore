using System.Collections.Generic;
using Core.Features.Data.Enums;
using Exiled.API.Enums;

namespace Core.Modules.Subclasses.Features.Structs.Subclasses.Abilities;

public class AbilityConfig
{
    public class Xp
    {
        public LevelToken Token;
        public double XpMultiplier;
    }

    public class Thief
    {
        public uint Cooldown;
        public List<ItemType> Items;
    }

    public class Pry
    {
        public uint Cooldown;
    }

    public class ArmoRegen
    {
        public uint Seconds;
    }

    public class DejaVu
    {
        public uint Cooldown;
        public uint Duration;
        public int Uses;
    }

    public class CheatDeath
    {
        public uint Uses;
        public uint HealthOnRespawn;
    }

    public class Invisible
    {
        public uint Cooldown;
    }
        
    public class InvisibleCommand
    {
        public uint Cooldown;
        public uint Duration;
    }

    public class PowerSurge
    {
        public uint Cooldown;
        public uint Duration;
        public ZoneType AffectedZone;
    }

    public class SizeChange
    {
        public uint Cooldown;
        public uint Duration;
    }

    public class Teleport
    {
        public uint Cooldown;
        public uint TimeToTeleport;
        public bool SameZone;
    }

    public class Stab
    {
        public uint Cooldown;
    }

    public class Punch
    {
        public uint Cooldown;
        public float Damage;
    }

    public class Scp939Command
    {
        public uint Cooldown;
        public uint Duration;
    }

    public class Heal
    {
        public float Amount;
        public uint Cooldown;
        public bool Area;
    }
        
    public class Revive
    {
        public float HealAmount;
        public uint Cooldown;
        public bool Area;
    }

    public class DamageArea
    {
        public uint Amount;
        public uint SecondsPerTick;
    }
        
    public class HealArea
    {
        public uint Amount;
        public uint SecondsPerTick;
    }

    public class AirStrike
    {
        public uint Cooldown;
        public uint GrenadesAmount;
        public uint FlashAmount;
        public uint FallDuration;
    }
        
    public class CarePackage
    {
        public uint Cooldown;
        public List<ItemType> Items;
        public uint FallDuration;
    }

    public class Vampire
    {
        public uint Amount;
    }

    public class DropGrenade
    {
        public uint Cooldown;
        public uint FuseTime;
    }

    public class DropFlash
    {
        public uint Cooldown;
        public uint FuseTime;
    }

    public class Bounty
    {
        public List<ItemType> PossibleItems;
        public uint NumberOfItems;
    }

    public class Deposit
    {
        public uint Cooldown;
        public List<ItemType> PossibleItems;
        public uint CoinsNeeded;
    }

    public class AreaEffect
    {
        public uint Cooldown;
        public float Range;
        public string Effect;
        public uint Duration;
    }

    public class Rage
    {
        public uint HealthPercentage;
    }

    public class Vanish
    {
        public uint Duration;
        public uint KillsNeeded;
    }

    public class Evolve
    {
        public int Amount;
    }

    public class SelfHeal
    {
        public uint HealthPerSecond;
        public bool DamageStopsHealing;
    }

    public class Scan
    {
        public uint Cooldown;
        public float Range;
    }

    public class Buff
    {
        public float Range;
        public uint Amount;
    }

    public class Shock
    {
        public uint Cooldown;
        public float Range;
        public bool Area;
    }

    public class AmmoBox
    {
        public uint AmmoOnGrab;
    }

    public class HealthBox
    {
        public uint HealthOnGrab;
    }

    public class Disguise
    {
        public uint Cooldown;
        public uint Duration;
        public List<RoleType> PossibleMorphs;
    }

    public class DamageEffect
    {
        public uint TotalBullets;
        public uint EffectDuration;
        public string EffectName;
    }

    public class HeadShot
    {
        public List<RoleType> RolesAffected;
    }

    public class Execute
    {
        public uint Percentage;
    }

    public class Backup
    {
        public uint Cooldown;
        public uint MaxUses;
    }
}