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

    private static CoreRarity GetRarity(this LevelToken token)
    {
        return token switch
        {
            LevelToken.Traveler  => CoreRarity.Common,
            LevelToken.Curator => CoreRarity.Common,
            LevelToken.Collect => CoreRarity.Common,
            LevelToken.Increase => CoreRarity.Common,
            LevelToken.Access => CoreRarity.Common,
            LevelToken.Decay => CoreRarity.Common,
            LevelToken.Snap => CoreRarity.Common,
            LevelToken.Bite => CoreRarity.Common,
            LevelToken.Purge => CoreRarity.Common,
            LevelToken.Scream => CoreRarity.Common,
            LevelToken.Hyper => CoreRarity.Common,
            LevelToken.Ointment => CoreRarity.Common,
            LevelToken.Cash => CoreRarity.Common,
            LevelToken.CrimesPay => CoreRarity.Common,
            LevelToken.Control => CoreRarity.Common,
            LevelToken.Destruction => CoreRarity.Common,
            LevelToken.Oops => CoreRarity.Common,
            LevelToken.Toss => CoreRarity.Common,
            LevelToken.Welcome => CoreRarity.Common,
            LevelToken.Erase => CoreRarity.Rare,
            LevelToken.Mad => CoreRarity.Rare,
            LevelToken.Detonate => CoreRarity.Rare,
            LevelToken.Rebuild => CoreRarity.Rare,
            LevelToken.Gossip => CoreRarity.Rare,
            LevelToken.Deplete => CoreRarity.Rare,
            LevelToken.Warlord => CoreRarity.Rare,
            LevelToken.Glimmer => CoreRarity.Rare,
            LevelToken.Bang => CoreRarity.Rare,
            LevelToken.Gambling => CoreRarity.Rare,
            LevelToken.Bet => CoreRarity.Rare,
            LevelToken.Saviors => CoreRarity.Rare,
            LevelToken.Invaders => CoreRarity.Rare,
            LevelToken.Electrified => CoreRarity.Rare,
            LevelToken.Psychotic => CoreRarity.Epic,
            LevelToken.Subdue => CoreRarity.Epic,
            LevelToken.Restore => CoreRarity.Epic,
            LevelToken.Awaken => CoreRarity.Epic,
            LevelToken.Atomic => CoreRarity.Epic,
            LevelToken.Revoke => CoreRarity.Epic,
            LevelToken.Port => CoreRarity.Epic,
            LevelToken.ZombieSlayer => CoreRarity.Epic,
            LevelToken.Hate => CoreRarity.Epic,
            LevelToken.Stalker => CoreRarity.Epic,
            LevelToken.Sharpshooter => CoreRarity.Epic,
            LevelToken.Particles => CoreRarity.Epic,
            LevelToken.Disappear => CoreRarity.Legendary,
            LevelToken.Renounce => CoreRarity.Legendary,
            LevelToken.Deranged => CoreRarity.Legendary,
            LevelToken.NoMansLand => CoreRarity.Legendary,
            LevelToken.Survivor => CoreRarity.Legendary,
            LevelToken.Ace => CoreRarity.Legendary,
            LevelToken.SerialKiller => CoreRarity.Legendary,
            LevelToken.MonsterHunter => CoreRarity.Legendary,
            LevelToken.Rupture => CoreRarity.Legendary,
            LevelToken.TheMask => CoreRarity.Legendary,
            LevelToken.BigLizard => CoreRarity.Legendary,
            LevelToken.Jesus => CoreRarity.Mythic,
            LevelToken.Cursed => CoreRarity.Mythic,
            LevelToken.ButtonCombo => CoreRarity.Mythic,
            LevelToken.WolfPackForever => CoreRarity.Mythic,
            LevelToken.JesusSupportCode => CoreRarity.Mythic,
            _ => CoreRarity.Common
        };
    }

    public static string GetString(this LevelToken token, int exp)
    {
        return token.GetRarity() switch
        {
            CoreRarity.Common => $"⌈ <color=#aeff70>◉</color> | {token.GetName()} | <color=#aeff70>+ {exp}XP</color> ⌋",
            CoreRarity.Rare => $"⌈ <color=#70b0ff>◈</color> | {token.GetName()} | <color=#70b0ff>+ {exp}XP</color> ⌋",
            CoreRarity.Epic => $"⌈ <color=#e070ff>❖</color> | {token.GetName()} | <color=#e070ff>+ {exp}XP</color> ⌋",
            CoreRarity.Legendary => $"⌈ <color=#ffc670>✴</color> | {token.GetName()} | <color=#ffc670>+ {exp}XP</color> ⌋",
            CoreRarity.Mythic => $"⌈ <color=#f7ff66>✽</color> | {token.GetName()} | <color=#f7ff66>+ {exp}XP</color> ⌋",
            _ => string.Empty,
        };
    }
}