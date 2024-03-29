﻿using Core.Features.Data.Enums;

namespace Core.Features.Extensions;

public static class TokenExtensions
{
    public static string GetName(this LevelToken token)
    {
        return token switch
        {
            LevelToken.Access => " <color=#1c8671>A</color><color=#4d946f>c</color><color=#7ea36c>c</color><color=#7ea36c>e</color><color=#4d946f>s</color><color=#1c8671>s</color>",
            LevelToken.Ace => " <color=#ffc400>A</color><color=#97853f>c</color><color=#ffc400>e</color>",
            LevelToken.Atomic => " <color=#00027b>A</color><color=#004eb0>t</color><color=#0099e5>o</color><color=#0099e5>m</color><color=#004eb0>i</color><color=#00027b>c</color>",
            LevelToken.Awaken => " <color=#9d00ff>A</color><color=#c466ff>w</color><color=#ebccff>a</color><color=#ebccff>k</color><color=#c466ff>e</color><color=#9d00ff>n</color>",
            LevelToken.Bang => " <color=#6a0000>B</color><color=#773f34>a</color><color=#773f34>n</color><color=#6a0000>g</color>",
            LevelToken.Bet => " <color=#00ff5e>B</color><color=#307139>e</color><color=#00ff5e>t</color>",
            LevelToken.Bite => " <color=#ff7b7b>B</color><color=#b44047>i</color><color=#b44047>t</color><color=#ff7b7b>e</color>",
            LevelToken.Cash => " <color=#b39e53>C</color><color=#b6a976>a</color><color=#b6a976>s</color><color=#b39e53>h</color>",
            LevelToken.Collect => " <color=#75c3ff>C</color><color=#70cbea>o</color><color=#6ad3d4>l</color><color=#65dbbf>l</color><color=#6ad3d4>e</color><color=#70cbea>c</color><color=#75c3ff>t</color>",
            LevelToken.Control => " <color=#2599ff>C</color><color=#196aff>o</color><color=#0c3cff>n</color><color=#000dff>t</color><color=#0c3cff>r</color><color=#196aff>o</color><color=#2599ff>l</color>",
            LevelToken.CrimesPay => " <color=#ffa200>C</color><color=#db8c05>r</color><color=#b7750a>i</color><color=#945f0f>m</color><color=#704814>e</color><color=#704814>s</color> <color=#b7750a>P</color><color=#db8c05>a</color><color=#ffa200>y</color>",
            LevelToken.Curator => " <color=#ff7575>C</color><color=#ef8787>u</color><color=#e09898>r</color><color=#d0aaaa>a</color><color=#e09898>t</color><color=#ef8787>o</color><color=#ff7575>r</color>",
            LevelToken.Cursed => " <color=#5c2668>C</color><color=#53515c>u</color><color=#4a7c51>r</color><color=#4a7c51>s</color><color=#53515c>e</color><color=#5c2668>d</color>",
            LevelToken.Decay => " <color=#6f6f6f>D</color><color=#5f5f5f>e</color><color=#4f4f4f>c</color><color=#5f5f5f>a</color><color=#6f6f6f>y</color>",
            LevelToken.Deplete => " <color=#d2faff>D</color><color=#b5dce7>e</color><color=#98bdcf>p</color><color=#7b9fb7>l</color><color=#98bdcf>e</color><color=#b5dce7>t</color><color=#d2faff>e</color>",
            LevelToken.Deranged => " <color=#441616>D</color><color=#3e1414>e</color><color=#381212>r</color><color=#321010>a</color><color=#321010>n</color><color=#381212>g</color><color=#3e1414>e</color><color=#441616>d</color>",
            LevelToken.Destruction => " <color=#b5363f>D</color><color=#af494a>e</color><color=#a85b55>s</color><color=#a26e5f>t</color><color=#9b806a>r</color><color=#959375>u</color><color=#9b806a>c</color><color=#a26e5f>t</color><color=#a85b55>i</color><color=#af494a>o</color><color=#b5363f>n</color>",
            LevelToken.Detonate => " <color=#443131>D</color><color=#5b4e3f>e</color><color=#736b4d>t</color><color=#8a885b>o</color><color=#8a885b>n</color><color=#736b4d>a</color><color=#5b4e3f>t</color><color=#443131>e</color>",
            LevelToken.Disappear => " <color=#9d835c>D</color><color=#887456>i</color><color=#736551>s</color><color=#5d554b>a</color><color=#484645>p</color><color=#5d554b>p</color><color=#736551>e</color><color=#887456>a</color><color=#9d835c>r</color>",
            LevelToken.Electrified => " <color=#00aaff>E</color><color=#0094de>l</color><color=#007fbd>e</color><color=#00699d>c</color><color=#00547c>t</color><color=#003e5b>r</color><color=#00547c>i</color><color=#00699d>f</color><color=#007fbd>i</color><color=#0094de>e</color><color=#00aaff>d</color>",
            LevelToken.Erase => " <color=#7b0000>E</color><color=#7e3d29>r</color><color=#807a51>a</color><color=#7e3d29>s</color><color=#7b0000>e</color>",
            LevelToken.Gambling => " <color=#314979>G</color><color=#536486>a</color><color=#757f94>m</color><color=#979aa1>b</color><color=#979aa1>l</color><color=#757f94>i</color><color=#536486>n</color><color=#314979>g</color>",
            LevelToken.Glimmer => " <color=#fbff00>G</color><color=#e1db19>l</color><color=#c8b733>i</color><color=#ae934c>m</color><color=#c8b733>m</color><color=#e1db19>e</color><color=#fbff00>r</color>",
            LevelToken.Gossip => " <color=#00e5ff>G</color><color=#14b3d0>o</color><color=#2980a1>s</color><color=#2980a1>s</color><color=#14b3d0>i</color><color=#00e5ff>p</color>",
            LevelToken.Hate => " <color=#570000>H</color><color=#860000>a</color><color=#860000>t</color><color=#570000>e</color>",
            LevelToken.Hyper => " <color=#874599>H</color><color=#59237c>y</color><color=#2a005e>p</color><color=#59237c>e</color><color=#874599>r</color>",
            LevelToken.Increase => " <color=#5d5fb9>I</color><color=#5c56ab>n</color><color=#5a4d9d>c</color><color=#59448f>r</color><color=#59448f>e</color><color=#5a4d9d>a</color><color=#5c56ab>s</color><color=#5d5fb9>e</color>",
            LevelToken.Invaders => " <color=#2f5b00>I</color><color=#3b5a19>n</color><color=#475a33>v</color><color=#53594c>a</color><color=#53594c>d</color><color=#475a33>e</color><color=#3b5a19>r</color><color=#2f5b00>s</color>",
            LevelToken.Jesus => " <color=#7300ff>J</color><color=#530db3>e</color><color=#321a66>s</color><color=#770db3>u</color><color=#bb00ff>s</color>",
            LevelToken.Mad => " <color=#912b2b>M</color><color=#ff0000>a</color><color=#912b2b>d</color>",
            LevelToken.MonsterHunter => " <color=#2c008a>M</color><color=#2a0084>o</color><color=#28007e>n</color><color=#260078>s</color><color=#240071>t</color><color=#22006b>e</color><color=#200065>r</color> <color=#1c005a>H</color><color=#1b0054>u</color><color=#19004f>n</color><color=#170049>t</color><color=#160044>e</color><color=#14003e>r</color>",
            LevelToken.NoMansLand => " <color=#010853>N</color><color=#080d4c>o</color> <color=#15183e>M</color><color=#1b1d36>a</color><color=#22232f>n</color><color=#282828>'</color><color=#22232f>s</color> <color=#15183e>L</color><color=#0e1345>a</color><color=#080d4c>n</color><color=#010853>d</color>",
            LevelToken.Ointment => " <color=#189567>O</color><color=#3e7655>i</color><color=#645743>n</color><color=#8a3831>t</color><color=#8a3831>m</color><color=#645743>e</color><color=#3e7655>n</color><color=#189567>t</color>",
            LevelToken.Oops => " <color=#889162>O</color><color=#5d6e66>o</color><color=#5d6e66>p</color><color=#889162>s</color>",
            LevelToken.Port => " <color=#575757>P</color><color=#c7c7c7>o</color><color=#c7c7c7>r</color><color=#575757>t</color>",
            LevelToken.Psychotic => " <color=#912b2b>P</color><color=#792424>s</color><color=#601d1d>y</color><color=#481616>c</color><color=#2f0f0f>h</color><color=#481616>o</color><color=#601d1d>t</color><color=#792424>i</color><color=#912b2b>c</color>",
            LevelToken.Purge => " <color=#3b3735>P</color><color=#43643e>u</color><color=#4b9146>r</color><color=#43643e>g</color><color=#3b3735>e</color>",
            LevelToken.Rebuild => " <color=#0400ff>R</color><color=#162be6>e</color><color=#2857ce>b</color><color=#3a82b5>u</color><color=#2857ce>i</color><color=#162be6>l</color><color=#0400ff>d</color>",
            LevelToken.Renounce => " <color=#fbff00>R</color><color=#def019>e</color><color=#c1e131>n</color><color=#a4d24a>o</color><color=#a4d24a>u</color><color=#c1e131>n</color><color=#def019>c</color><color=#fbff00>e</color>",
            LevelToken.Restore => " <color=#ff6969>R</color><color=#de9b86>e</color><color=#becda4>s</color><color=#9dffc1>t</color><color=#becda4>o</color><color=#de9b86>r</color><color=#ff6969>e</color>",
            LevelToken.Revoke => " <color=#ff6c6c>R</color><color=#ffa7a7>e</color><color=#ffe2e2>v</color><color=#ffe2e2>o</color><color=#ffa7a7>k</color><color=#ff6c6c>e</color>",
            LevelToken.Rupture => " <color=#5badff>R</color><color=#3d97e4>u</color><color=#1e81c9>p</color><color=#006bae>t</color><color=#1e81c9>u</color><color=#3d97e4>r</color><color=#5badff>e</color>",
            LevelToken.Saviors => " <color=#0a8dff>S</color><color=#085eff>a</color><color=#062fff>v</color><color=#0400ff>i</color><color=#062fff>o</color><color=#085eff>r</color><color=#0a8dff>s</color>",
            LevelToken.Scream => " <color=#9f9f9f>S</color><color=#8b8b8b>c</color><color=#767676>r</color><color=#767676>e</color><color=#8b8b8b>a</color><color=#9f9f9f>m</color>",
            LevelToken.SerialKiller => " <color=#6c0000>S</color><color=#650000>e</color><color=#5e0000>r</color><color=#570101>i</color><color=#500101>a</color><color=#490101>l</color> <color=#400101>K</color><color=#3e0101>i</color><color=#3c0101>l</color><color=#390000>l</color><color=#370000>e</color><color=#350000>r</color>",
            LevelToken.Snap => " <color=#716e50>D</color><color=#605d40>e</color><color=#4f4b2f>c</color><color=#605d40>a</color><color=#716e50>y</color>",
            LevelToken.Subdue => " <color=#575757>S</color><color=#717171>u</color><color=#8a8a8a>b</color><color=#8a8a8a>d</color><color=#717171>u</color><color=#575757>e</color>",
            LevelToken.Survivor => " <color=#71356e>S</color><color=#81567f>u</color><color=#90778f>r</color><color=#a098a0>v</color><color=#a098a0>i</color><color=#90778f>v</color><color=#81567f>o</color><color=#71356e>r</color>",
            LevelToken.Traveler => " <color=#4aba3f>E</color><color=#2e814c>n</color><color=#124759>t</color><color=#2e814c>r</color><color=#4aba3f>y</color>",
            LevelToken.Warlord => " <color=#144f00>W</color><color=#623500>a</color><color=#b11a00>r</color><color=#ff0000>l</color><color=#ff4e00>o</color><color=#ff9c00>r</color><color=#ffea00>d</color>",
            LevelToken.Welcome => " <color=#ff00f2>W</color><color=#ff55f6>e</color><color=#ffaafb>l</color><color=#ffffff>c</color><color=#ffaafb>o</color><color=#ff55f6>m</color><color=#ff00f2>e</color>",
            LevelToken.ZombieSlayer => " <color=#ff0000>Z</color><color=#d61503>o</color><color=#ac2b06>m</color><color=#83400a>b</color><color=#5a550d>i</color><color=#306b10>e</color> <color=#306b10>S</color><color=#5a550d>l</color><color=#83400a>a</color><color=#ac2b06>y</color><color=#d61503>e</color><color=#ff0000>r</color>",
            LevelToken.Toss => " <color=#00ffaa>T</color><color=#65b6a3>o</color><color=#65b6a3>s</color><color=#00ffaa>s</color>",
            LevelToken.Stalker => " <color=#5900ff>S</color><color=#4037ff>t</color><color=#266dff>a</color><color=#0da4ff>l</color><color=#0da4ff>k</color><color=#266dff>e</color><color=#4037ff>r</color><color=#5900ff></color>",
            LevelToken.Sharpshooter => "<color=#91b06a>S</color><color=#8fa663>h</color><color=#8d9c5b>a</color><color=#8c9254>r</color><color=#8a884d>p</color><color=#887e45>s</color><color=#86743e>h</color><color=#8b8040>o</color><color=#908b42>o</color><color=#959745>t</color><color=#99a247>e</color><color=#9eae49>r</color>",
            LevelToken.ButtonCombo => " <color=#2b00ff>B</color><color=#5223d1>u</color><color=#7845a2>t</color><color=#9f6874>t</color><color=#c58b46>o</color><color=#ecae17>n</color> <color=#b9d017>C</color><color=#8bdc27>o</color><color=#5de836>m</color><color=#2ef346>b</color><color=#00ff55>o</color>",
            LevelToken.WolfPackForever => " <color=#a600ff>W</color><color=#b120ff>o</color><color=#bc40ff>l</color><color=#c760ff>f</color> <color=#de9fff>P</color><color=#e9bfff>a</color><color=#f4dfff>c</color><color=#ffffff>k</color> <color=#e9bfff>F</color><color=#de9fff>o</color><color=#d380ff>r</color><color=#c760ff>e</color><color=#bc40ff>v</color><color=#b120ff>e</color><color=#a600ff>r</color>",
            LevelToken.TheMask => " <color=#000000>T</color><color=#494949>h</color><color=#929292>e</color> <color=#dbdbdb>M</color><color=#929292>a</color><color=#494949>s</color><color=#000000>k</color>",
            LevelToken.BigLizard => " <color=#ff0000>B</color><color=#df1212>i</color><color=#bf2424>g</color> <color=#7f4949>L</color><color=#6d4c4c>i</color><color=#6a4040>z</color><color=#673535>a</color><color=#632929>r</color><color=#601d1d>d</color>",
            LevelToken.Particles => " <color=#55e8ff>P</color><color=#40c6f0>a</color><color=#2ba3e2>r</color><color=#1581d3>t</color><color=#005ec4>i</color><color=#1581d3>c</color><color=#2ba3e2>l</color><color=#40c6f0>e</color><color=#55e8ff>s</color>",
            LevelToken.JesusSupportCode => " <color=#6200ff>J</color><color=#4b05ff>e</color><color=#340aff>s</color><color=#1d0fff>u</color><color=#0614ff>s</color> <color=#047596>S</color><color=#06ac5a>u</color><color=#08e31e>p</color><color=#26e100>p</color><color=#60a500>o</color><color=#9a6900>r</color><color=#d42d00>t</color> <color=#ff4700>C</color><color=#ff8000>o</color><color=#ffb900>d</color><color=#fff200>e</color>",
            _ => token.ToString(),
        };
    }

    private static CoreRarity GetRarity(this LevelToken token)
    {
        return token switch
        {
            LevelToken.Traveler or LevelToken.Curator or LevelToken.Collect or LevelToken.Increase or LevelToken.Access or LevelToken.Decay or LevelToken.Snap or LevelToken.Bite or LevelToken.Purge or LevelToken.Scream or LevelToken.Hyper or LevelToken.Ointment or LevelToken.Cash or LevelToken.CrimesPay or LevelToken.Control or LevelToken.Destruction or LevelToken.Oops or LevelToken.Toss or LevelToken.Welcome 
            => CoreRarity.Common, 
            
            LevelToken.Erase or LevelToken.Mad or LevelToken.Detonate or LevelToken.Rebuild or LevelToken.Gossip or LevelToken.Deplete or LevelToken.Warlord or LevelToken.Glimmer or LevelToken.Bang or LevelToken.Gambling or LevelToken.Bet or LevelToken.Saviors or LevelToken.Invaders or LevelToken.Electrified 
            => CoreRarity.Rare,
            
            LevelToken.Psychotic or LevelToken.Subdue or LevelToken.Restore or LevelToken.Awaken or LevelToken.Atomic or LevelToken.Revoke or LevelToken.Port or LevelToken.ZombieSlayer or LevelToken.Hate or LevelToken.Stalker or LevelToken.Sharpshooter or LevelToken.Particles 
            => CoreRarity.Epic,
            
            LevelToken.Disappear or LevelToken.Renounce or LevelToken.Deranged or LevelToken.NoMansLand or LevelToken.Survivor or LevelToken.Ace or LevelToken.SerialKiller or LevelToken.MonsterHunter or LevelToken.Rupture or LevelToken.TheMask or LevelToken.BigLizard 
            => CoreRarity.Legendary,
            
            LevelToken.Jesus or LevelToken.Cursed or LevelToken.ButtonCombo or LevelToken.WolfPackForever or LevelToken.JesusSupportCode 
                => CoreRarity.Mythic,
            
            _ => CoreRarity.Common
        };
    }

    public static string GetString(this LevelToken token, int exp)
    {
        if (LevelExtensions.ExpMultiplier != 1)
        {
            return token.GetRarity() switch
            {
                CoreRarity.Common => $"⌈ <color=#aeff70>◉</color> | {token.GetName()} | <color=#aeff70>+ <color=#fce188>{LevelExtensions.ExpMultiplier}x</color>{exp}XP</color>  ⌋",
                CoreRarity.Rare => $"⌈ <color=#70b0ff>◈</color> | {token.GetName()} | <color=#70b0ff>+ <color=#fce188>{LevelExtensions.ExpMultiplier}x</color>{exp}XP</color> ⌋",
                CoreRarity.Epic => $"⌈ <color=#e070ff>❖</color> | {token.GetName()} | <color=#e070ff>+ <color=#fce188>{LevelExtensions.ExpMultiplier}x</color>{exp}XP</color> ⌋",
                CoreRarity.Legendary => $"⌈ <color=#ffc670>✴</color> | {token.GetName()} | <color=#ffc670>+ <color=#fce188>{LevelExtensions.ExpMultiplier}x</color>{exp}XP</color> ⌋",
                CoreRarity.Mythic => $"⌈ <color=#f7ff66>✽</color> | {token.GetName()} | <color=#f7ff66>+ <color=#fce188>{LevelExtensions.ExpMultiplier}x</color>{exp}XP</color> ⌋",
                _ => string.Empty,
            };
        }
        
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
    
    public static string GetIcon(this CoreRarity rarity)
    {
        return rarity switch
        {
            CoreRarity.Common => "<color=#aeff70>◉</color>",
            CoreRarity.Rare => "<color=#70b0ff>◈</color>",
            CoreRarity.Epic => "<color=#e070ff>❖</color>",
            CoreRarity.Legendary => "<color=#ffc670>✴</color>",
            CoreRarity.Mythic => "<color=#f7ff66>✽</color>",
            _ => string.Empty,
        };
    }
}