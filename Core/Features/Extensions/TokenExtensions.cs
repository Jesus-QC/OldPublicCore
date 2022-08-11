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
        switch (token)
        {
            case LevelToken.Traveler:
                return LevelRarity.Common;
            case LevelToken.Curator:
                return LevelRarity.Common;
            case LevelToken.Collect:
                return LevelRarity.Common;
            case LevelToken.Erase:
                return LevelRarity.Rare;
            case LevelToken.Disappear:
                return LevelRarity.Legendary;
            case LevelToken.Renounce:
                return LevelRarity.Legendary;
            case LevelToken.Mad:
                return LevelRarity.Rare;
            case LevelToken.Psychotic:
                return LevelRarity.Epic;
            case LevelToken.Deranged:
                return LevelRarity.Legendary;
            case LevelToken.Subdue:
                return LevelRarity.Epic;
            case LevelToken.Restore:
                return LevelRarity.Epic;
            case LevelToken.Increase:
                return LevelRarity.Common;
            case LevelToken.Detonate:
                return LevelRarity.Rare;
            case LevelToken.Awaken:
                return LevelRarity.Epic;
            case LevelToken.Rebuild:
                return LevelRarity.Rare;
            case LevelToken.Gossip:
                return LevelRarity.Rare;
            case LevelToken.Deplete:
                return LevelRarity.Rare;
            case LevelToken.NoMansLand:
                return LevelRarity.Legendary;
            case LevelToken.Access:
                return LevelRarity.Common;
            case LevelToken.Decay:
                return LevelRarity.Common;
            case LevelToken.Snap:
                return LevelRarity.Common;
            case LevelToken.Bite:
                return LevelRarity.Common;
            case LevelToken.Purge:
                return LevelRarity.Common;
            case LevelToken.Scream:
                return LevelRarity.Common;
            case LevelToken.Warlord:
                return LevelRarity.Rare;
            case LevelToken.Glimmer:
                return LevelRarity.Rare;
            case LevelToken.Bang:
                return LevelRarity.Rare;
            case LevelToken.Survivor:
                return LevelRarity.Legendary;
            case LevelToken.Gambling:
                return LevelRarity.Rare;
            case LevelToken.Cash:
                return LevelRarity.Common;
            case LevelToken.Ace:
                return LevelRarity.Legendary;
            case LevelToken.Hyper:
                return LevelRarity.Common;
            case LevelToken.Ointment:
                return LevelRarity.Common;
            case LevelToken.Atomic:
                return LevelRarity.Epic;
            case LevelToken.Revoke:
                return LevelRarity.Epic;
            case LevelToken.Port:
                return LevelRarity.Epic;
            case LevelToken.Jesus:
                return LevelRarity.Mythic;
            case LevelToken.SerialKiller:
                return LevelRarity.Legendary;
            case LevelToken.MonsterHunter:
                return LevelRarity.Legendary;
            case LevelToken.ZombieSlayer:
                return LevelRarity.Epic;
            case LevelToken.Electrified:
                return LevelRarity.Rare;
            case LevelToken.CrimesPay:
                return LevelRarity.Common;
            case LevelToken.Control:
                return LevelRarity.Common;
            case LevelToken.Destruction:
                return LevelRarity.Common;
            case LevelToken.Cursed:
                return LevelRarity.Mythic;
            case LevelToken.Rupture:
                return LevelRarity.Legendary;
            case LevelToken.Oops:
                return LevelRarity.Common;
            case LevelToken.Toss:
                return LevelRarity.Common;
            case LevelToken.Welcome:
                return LevelRarity.Common;
            case LevelToken.Hate:
                return LevelRarity.Epic;
            case LevelToken.Bet:
                return LevelRarity.Rare;
            case LevelToken.Saviors:
                return LevelRarity.Rare;
            case LevelToken.Invaders:
                return LevelRarity.Rare;
        }

        return LevelRarity.Common;
    }

    public static string GetString(this LevelToken token, int exp)
    {
        switch (token.GetRarity())
        {
            case LevelRarity.Common:
                return $"⌈ <color=#aeff70>◉</color> | {token.GetName()} | <color=#aeff70>+ {exp}XP</color> ⌋";
            case LevelRarity.Rare:
                return $"⌈ <color=#70b0ff>◈</color> | {token.GetName()} | <color=#70b0ff>+ {exp}XP</color> ⌋";
            case LevelRarity.Epic:
                return $"⌈ <color=#e070ff>❖</color> | {token.GetName()} | <color=#e070ff>+ {exp}XP</color> ⌋";
            case LevelRarity.Legendary:
                return $"⌈ <color=#ffc670>✴</color> | {token.GetName()} | <color=#ffc670>+ {exp}XP</color> ⌋";
            case LevelRarity.Mythic:
                return $"⌈ <color=#f7ff66>✽</color> | {token.GetName()} | <color=#f7ff66>+ {exp}XP</color> ⌋";
        }

        return string.Empty;
    }
}