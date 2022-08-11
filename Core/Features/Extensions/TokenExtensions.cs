using System;
using Core.Features.Data.Enums;
using Core.Modules.Levels;

namespace Core.Features.Extensions;

public static class TokenExtensions
{
    public static string GetName(this LevelToken token)
    {
        if (LevelsModule.ModuleConfig.TokenNames.ContainsKey(token))
            return LevelsModule.ModuleConfig.TokenNames[token];

        return token.ToString();
    }

    public static int GetExp(this LevelToken token)
    {
        if (LevelsModule.ModuleConfig.TokenExp.ContainsKey(token))
            return LevelsModule.ModuleConfig.TokenExp[token];

        return 0;
    }

    private static LevelRarity GetRarity(this LevelToken token)
    {
        return token switch
        {
            LevelToken.Traveler  => LevelRarity.Common,
            LevelToken.Curator => LevelRarity.Common,
            LevelToken.Collect => LevelRarity.Common,
            LevelToken.Increase => LevelRarity.Common,
            LevelToken.Access => LevelRarity.Common,
            LevelToken.Decay => LevelRarity.Common,
            LevelToken.Snap => LevelRarity.Common,
            LevelToken.Bite => LevelRarity.Common,
            LevelToken.Purge => LevelRarity.Common,
            LevelToken.Scream => LevelRarity.Common,
            LevelToken.Hyper => LevelRarity.Common,
            LevelToken.Ointment => LevelRarity.Common,
            LevelToken.Cash => LevelRarity.Common,
            LevelToken.CrimesPay => LevelRarity.Common,
            LevelToken.Control => LevelRarity.Common,
            LevelToken.Destruction => LevelRarity.Common,
            LevelToken.Oops => LevelRarity.Common,
            LevelToken.Toss => LevelRarity.Common,
            LevelToken.Welcome => LevelRarity.Common,
            LevelToken.Erase => LevelRarity.Rare,
            LevelToken.Mad => LevelRarity.Rare,
            LevelToken.Detonate => LevelRarity.Rare,
            LevelToken.Rebuild => LevelRarity.Rare,
            LevelToken.Gossip => LevelRarity.Rare,
            LevelToken.Deplete => LevelRarity.Rare,
            LevelToken.Warlord => LevelRarity.Rare,
            LevelToken.Glimmer => LevelRarity.Rare,
            LevelToken.Bang => LevelRarity.Rare,
            LevelToken.Gambling => LevelRarity.Rare,
            LevelToken.Bet => LevelRarity.Rare,
            LevelToken.Saviors => LevelRarity.Rare,
            LevelToken.Invaders => LevelRarity.Rare,
            LevelToken.Electrified => LevelRarity.Rare,
            LevelToken.Psychotic => LevelRarity.Epic,
            LevelToken.Subdue => LevelRarity.Epic,
            LevelToken.Restore => LevelRarity.Epic,
            LevelToken.Awaken => LevelRarity.Epic,
            LevelToken.Atomic => LevelRarity.Epic,
            LevelToken.Revoke => LevelRarity.Epic,
            LevelToken.Port => LevelRarity.Epic,
            LevelToken.ZombieSlayer => LevelRarity.Epic,
            LevelToken.Hate => LevelRarity.Epic,
            LevelToken.Stalker => LevelRarity.Epic,
            LevelToken.Sharpshooter => LevelRarity.Epic,
            LevelToken.Particles => LevelRarity.Epic,
            LevelToken.Disappear => LevelRarity.Legendary,
            LevelToken.Renounce => LevelRarity.Legendary,
            LevelToken.Deranged => LevelRarity.Legendary,
            LevelToken.NoMansLand => LevelRarity.Legendary,
            LevelToken.Survivor => LevelRarity.Legendary,
            LevelToken.Ace => LevelRarity.Legendary,
            LevelToken.SerialKiller => LevelRarity.Legendary,
            LevelToken.MonsterHunter => LevelRarity.Legendary,
            LevelToken.Rupture => LevelRarity.Legendary,
            LevelToken.TheMask => LevelRarity.Legendary,
            LevelToken.BigLizard => LevelRarity.Legendary,
            LevelToken.Jesus => LevelRarity.Mythic,
            LevelToken.Cursed => LevelRarity.Mythic,
            LevelToken.ButtonCombo => LevelRarity.Mythic,
            LevelToken.WolfPackForever => LevelRarity.Mythic,
            LevelToken.JesusSupportCode => LevelRarity.Mythic,
            _ => LevelRarity.Common
        };
    }

    public static string GetString(this LevelToken token, int exp)
    {
        return token.GetRarity() switch
        {
            LevelRarity.Common => $"⌈ <color=#aeff70>◉</color> | {token.GetName()} | <color=#aeff70>+ {exp}XP</color> ⌋",
            LevelRarity.Rare => $"⌈ <color=#70b0ff>◈</color> | {token.GetName()} | <color=#70b0ff>+ {exp}XP</color> ⌋",
            LevelRarity.Epic => $"⌈ <color=#e070ff>❖</color> | {token.GetName()} | <color=#e070ff>+ {exp}XP</color> ⌋",
            LevelRarity.Legendary => $"⌈ <color=#ffc670>✴</color> | {token.GetName()} | <color=#ffc670>+ {exp}XP</color> ⌋",
            LevelRarity.Mythic => $"⌈ <color=#f7ff66>✽</color> | {token.GetName()} | <color=#f7ff66>+ {exp}XP</color> ⌋",
            _ => string.Empty,
        };
    }
}