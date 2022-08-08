using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Features.Data.Enums;
using Core.Features.Events;
using Core.Features.Events.EventArgs;
using Exiled.API.Features;

namespace Core.Features.Extensions;

public static class LevelExtensions
{
    public const int Divider = 5000;

    public static int ExpMultiplier = 1;
    
    private static readonly Dictionary<Player, Dictionary<LevelToken, int>> PerksCooldown = new();
    private static readonly Dictionary<Player, int> Exp = new();
    private static readonly Dictionary<Player, int> RoundExp = new();

    public static void ResetLevels()
    {
        PerksCooldown.Clear();
        Exp.Clear();
        RoundExp.Clear();
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
        
        if (!PerksCooldown.ContainsKey(player))
            PerksCooldown.Add(player, new Dictionary<LevelToken, int>());

        if(!RoundExp.ContainsKey(player))
            RoundExp.Add(player, 0);
        
        player.ShowBadge();

        var lastSeen = new DateTime((long) Core.Database.ExecuteScalar($"SELECT LastSeen FROM SlStats WHERE PlayerId='{player.GetId()}';"));

        if (lastSeen.DayOfYear < DateTime.UtcNow.DayOfYear || lastSeen.Year < DateTime.UtcNow.Year) 
            player.AddExp(LevelToken.Welcome);
    }
    public static void WipeLevels(this Player player)
    {
        if (PerksCooldown.ContainsKey(player))
            PerksCooldown.Remove(player);
        if (Exp.ContainsKey(player))
            Exp.Remove(player);
        if (RoundExp.ContainsKey(player))
            RoundExp.Remove(player);
    }
        
    public static void AddExp(this Player player, LevelToken perk)
    {
        if (player.DoNotTrack)
            return;
        
        var exp = perk.GetExp();

        exp *= ExpMultiplier;

        player.SendHint(ScreenZone.Notifications, perk.GetString(exp));

        Exp[player] += exp;
        RoundExp[player] += exp;

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
            
        Task.Run(() =>
        {
            Core.Database.ExecuteNonQuery($"UPDATE Leveling SET Exp=Exp+{exp} WHERE PlayerId={player.GetId()};");
        });

        player.ShowBadge();
    }

    public static bool CheckCooldown(this Player player, LevelToken perk, int maxUses) => player.GetUses(perk) < maxUses;
    public static void AddUse(this Player player, LevelToken perk)
    {
        if(player == null || player.DoNotTrack || !PerksCooldown.ContainsKey(player))
            return;
        
        if(!PerksCooldown[player].ContainsKey(perk))
            PerksCooldown[player].Add(perk, 0);

        PerksCooldown[player][perk]++;
    }
    public static int GetUses(this Player player, LevelToken perk)
    {
        if (player == null || !PerksCooldown.ContainsKey(player))
            return 99;

        return !PerksCooldown[player].ContainsKey(perk) ? 0 : PerksCooldown[player][perk];
    }
    public static int GetLevel(int exp) => exp / Divider;
    public static int GetLevel(this Player player) => GetLevel(GetExp(player));

    public static int GetExp(this Player player)
    {
        if (Exp.ContainsKey(player))
            return Exp[player];
            
        var exp = (int) Core.Database.ExecuteScalar($"SELECT Exp FROM Leveling WHERE PlayerId='{player.GetId()}';");
        Exp.Add(player, exp);
        return exp;
    }

    public static int GetRoundExp(this Player player)
    {
        return RoundExp.ContainsKey(player) ? RoundExp[player] : 0;
    }
}