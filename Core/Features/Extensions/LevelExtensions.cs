using System;
using System.Collections.Generic;
using System.Linq;
using Core.Features.Data;
using Core.Features.Data.Enums;
using Core.Features.Data.UI;
using Core.Features.Events;
using Core.Features.Events.EventArgs;
using Exiled.API.Features;

namespace Core.Features.Extensions;

public static class LevelExtensions
{
    private static readonly Dictionary<Player, Dictionary<Perk, int>> PerksCooldown = new Dictionary<Player, Dictionary<Perk, int>>();
    private static readonly List<int> LevelsExpList = new List<int>() {
        0, 250, 500, 750, 1000, 1250, 1500, 1750, 2000, 2500, 3000, 3500, 4000, 4500, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000, 15000, 16000, 17000, 18000, 19000, 20000, 22000, 24000, 26000, 28000, 30000, 32000, 34000, 36000, 38000, 40000, 42500, 45000, 47500, 50000, 55000, 60000, 70000, 80000, 90000, 100000, 100500, 101000, 101500, 102000, 102500, 103000, 104000, 104500, 105000, 106000, 107000, 108000, 109000, 110000, 111000, 112000, 113000, 114000, 115000, 117500, 120000, 122500, 125000, 127500, 130000, 135000, 140000, 145000, 150000, 155000, 160000, 165000, 170000, 175000, 180000, 185000, 190000, 195000, 200000, 220000, 240000, 260000, 280000, 300000, 325000, 350000, 375000, 400000, 450000, 500000, 505000
    };

    private static readonly Dictionary<Player, int> Exp = new Dictionary<Player, int>();

    public static void ResetLevelsCooldown()
    {
        PerksCooldown.Clear();
        Exp.Clear();
    }
        
    public static void ShowBadge(this Player player)
    {
        player.DisplayNickname = $"Lvl: {player.GetLevel()} |  {player.Nickname}";
    }
    public static void SetUpLevels(this Player player)
    {
        if (player.DoNotTrack)
        {
            player.DisplayNickname = $"Lvl: DNT | {player.Nickname}";
            return;
        }
            
        if(!PerksCooldown.ContainsKey(player))
            PerksCooldown.Add(player, PerkExtensions.GetDefaultCooldown());

        player.ShowBadge();

        var lastSeen = new DateTime((long) Core.Database.ExecuteScalar($"SELECT LastSeen FROM SlStats WHERE PlayerId='{player.GetId()}'"));

        if (lastSeen.DayOfYear < DateTime.UtcNow.DayOfYear || lastSeen.Year < DateTime.UtcNow.Year) 
            player.AddExp(25, Perk.Welcome);
    }
    public static void WipeLevels(this Player player)
    {
        if (PerksCooldown.ContainsKey(player))
            PerksCooldown.Remove(player);
        if(Exp.ContainsKey(player))
            Exp.Remove(player);
    }
        
    public static void AddExp(this Player player, int exp, Perk perk)
    {
        if (player.DoNotTrack)
            return;
            
        var msg = new PerkMessage
        {
            Color = perk.GetColor(),
            ExpAmount = exp,
            Message = perk.ToString().Replace("_", " ")
        };
            
        player.GetManager().AddMessage(perk, msg);

        Exp[player] += exp;
            
        player.AddUse(perk);
        player.AddExp(exp);
    }
    public static void AddExp(this Player player, int exp)
    {
        if(player.DoNotTrack)
            return;
            
        var args = new AddingExpEventArgs(player, exp);
        Levels.OnAddingExp(args);
        if(!args.IsAllowed)
            return;
            
        Core.Database.ExecuteNonQuery($"UPDATE Leveling SET Exp=Exp+{exp} WHERE PlayerId={player.GetId()}");
            
        player.ShowBadge();
    }

    public static bool CheckCooldown(this Player player, Perk perk, int maxUses) => player.GetUses(perk) < maxUses;
    public static void AddUse(this Player player, Perk perk)
    {
        if(player == null || player.DoNotTrack || !PerksCooldown.ContainsKey(player))
            return;

        PerksCooldown[player][perk]++;

        var unlockedPerks = player.UnlockedPerks();

        if (unlockedPerks.Contains(perk)) return;
            
        unlockedPerks.Add(perk);
        Core.Database.ExecuteNonQuery($"UPDATE Leveling SET Achievements='{Utf8Json.JsonSerializer.ToJsonString(unlockedPerks)}' WHERE PlayerId='{player.GetId()}'");
    }
    public static int GetUses(this Player player, Perk perk)
    {
        if (player == null || !PerksCooldown.ContainsKey(player))
            return 99;

        return PerksCooldown[player][perk];
    }
    public static int GetLevel(int exp)
    {
        if (exp >= 505000)
            return exp / 5000;
            
        return LevelsExpList.IndexOf(LevelsExpList.First(x => x > exp));
    }
    public static int GetLevel(this Player player) => GetLevel(GetExp(player));

    public static int GetExp(this Player player)
    {
        if (Exp.ContainsKey(player))
            return Exp[player];
            
        var exp = (int) Core.Database.ExecuteScalar($"SELECT Exp FROM Leveling WHERE PlayerId='{player.GetId()}'");
        Exp.Add(player, exp);
        return exp;
    }
}