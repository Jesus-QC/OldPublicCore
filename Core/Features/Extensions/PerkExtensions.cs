using System;
using System.Collections.Generic;
using System.Linq;
using Core.Features.Data;
using Core.Features.Data.Enums;
using Exiled.API.Features;

namespace Core.Features.Extensions;

public static class PerkExtensions
{
    private static readonly Dictionary<Player, List<Perk>> CachedPerks = new Dictionary<Player, List<Perk>>();

    private static readonly Dictionary<Perk, string> PerksColor = new Dictionary<Perk, string>
    {
        {
            Perk.Adventurer, "26ff5e"
        },
        {
            Perk.Gather, "E1921A"
        },
        {
            Perk.Slay, "930000"
        },
        {
            Perk.Vanish, "FF9B00"
        },
        {
            Perk.Depart, "FFE800"
        },
        {
            Perk.Intensify, "AA00FF"
        },
        {
            Perk.Cripple, "AED6F1"
        },
        {
            Perk.Stitching, "EC7063"
        },
        {
            Perk.Strain, "CD6155"
        },
        {
            Perk.Immortal, "641E16"
        },
        {
            Perk.Progress, "A2D9CE"
        },
        {
            Perk.Blasted, "BA4A00"
        },
        {
            Perk.Recall, "8E44AD"
        },
        {
            Perk.Adapt, "1B4F72"
        },
        {
            Perk.Radio_Host, "884EA0"
        },
        {
            Perk.Oops, "FAE5D3"
        },
        {
            Perk.Broken_Bones, "AF7AC5"
        },
        {
            Perk.Maze_Runner, "2ECC71"
        },
        {
            Perk.Council_Approved, "273746"
        },
        {
            Perk.SCP_106, "17202A"
        },
        {
            Perk.SCP_173, "F5B041"
        },
        {
            Perk.SCP_93953, "7B241C"
        },
        {
            Perk.SCP_93989, "7B241C"
        },
        {
            Perk.SCP_049, "2E4053"
        },
        {
            Perk.SCP_096, "AAB7B8"
        },
        {
            Perk.Rambo, "1D8348"
        },
        {
            Perk.Bomber, "943126"
        },
        {
            Perk.Lightning, "FAD7A0"
        },
        {
            Perk.Lord, "9A7D0A"
        },
        {
            Perk.SCP_1162, "7FB3D5"
        },
        {
            Perk.Pocket_Money, "FEF5E7"
        },
        {
            Perk.Syringe, "CA6F1E"
        },
        {
            Perk.The_End, "2E4053"
        },
        {
            Perk.Choices, "CB4335"
        },
        {
            Perk.Gandalf, "4D5656"
        },
        {
            Perk.Jesus, "4A235A"
        },
        {
            Perk.Bloodthirst, "922B21"
        },
        {
            Perk.Baseball, "BDC3C7"
        },
        {
            Perk.Disappear, "F2D7D5"
        },
        {
            Perk.Cheater, "641E16"
        },
        {
            Perk.Sink_Back, "283747"
        },
        {
            Perk.Silence, "717D7E"
        },
        {
            Perk.Last_Stand, "145A32"
        },
        {
            Perk.Really_Cured, "D4AC0D"
        },
        {
            Perk.Fried, "AED6F1"
        },
        {
            Perk.Champions, "E59866"
        },
        {
            Perk.Freedom, "641E16"
        },
        {
            Perk.Closed_Zone, "AED6F1"
        },
        {
            Perk.Wolf_Pack, "73C6B6"
        },
        {
            Perk.Overcharge, "00FFCE"
        },
        {
            Perk.Toss, "FAE5D3"
        },
        {
            Perk.Welcome, "404045"
        },
        {
            Perk.First_Blood, "000"
        },
        {
            Perk.ClassD, "ff7f17"
        },
        {
            Perk.Random, "ff17bd"
        },
        {
            Perk.SCP, "820000"
        },
        {
            Perk.Scientist, "ffdc42"
        },
        {
            Perk.MTF, "424cff"
        },
        {
            Perk.The_Owner, "8C0AFF"
        },
        {
            Perk.The_Creator, "FF0000"
        },
        {
            Perk.The_Sexy_Man, "2D20EC"
        },
        {
            Perk.Heads_Or_Tails, "F3CE37"
        },
        {
            Perk.We_Bring_Chaos, "00A339"
        },
        {
            Perk.Secure_Contain_Protect, "1B6CE1"
        },
        {
            Perk.L1_L2_R1_R2, "DA2CF2"
        },
        {
            Perk.Hold_Your_Breath, "00A339"
        },
        {
            Perk.SCP_Hunter, "922B21"
        },
        {
            Perk.SCP_Destroyer, "922B21"
        },
        {
            Perk.Escapist, "FFC100"
        },
        {
            Perk.Dark_Pocket, "17202A"
        },
        {
            Perk.Last_Plugin, "DA2CF2"
        },
        {
            Perk.Mythical, "000"
        },
        {
            Perk.Legend, "000"
        },
        {
            Perk.Epic, "000"
        },
        {
            Perk.Rare, "000"
        },
        {
            Perk.Uncommon, "000"
        },
        {
            Perk.Common, "000"
        },
        {
            Perk.The_Reaper, "560000"
        },
    };

    public static string GetColor(this Perk perk) => PerksColor[perk];

    public static Dictionary<Perk, int> GetDefaultCooldown() => Enum.GetValues(typeof(Perk)).Cast<object>().ToDictionary(perk => (Perk)perk, perk => 0);

    public static void ClearCache() => CachedPerks.Clear();

    public static List<Perk> UnlockedPerks(this Player player)
    {
        if (player.DoNotTrack)
            return new List<Perk>();
            
        if (CachedPerks.ContainsKey(player))
            return CachedPerks[player];

        var perks = Utf8Json.JsonSerializer.Deserialize<List<Perk>>((string) Core.Database.ExecuteScalar($"SELECT Achievements FROM Leveling WHERE PlayerId='{player.GetId()}'"));
        CachedPerks[player] = perks;
        return perks;
    }

    public static bool HasUnlockedThePerk(this Player player, Perk perk) => player.UnlockedPerks().Contains(perk);
}