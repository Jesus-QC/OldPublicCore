using System;
using System.Collections.Generic;
using System.Linq;
using Core.Features.Data.Enums;
using Core.Features.Data.UI;
using Core.Features.Events;
using Core.Features.Events.EventArgs;
using Exiled.API.Features;

namespace Core.Features.Extensions;

public static class LevelExtensions
{
    public const int Divider = 5000; 
    
    private static readonly Dictionary<Player, Dictionary<Perk, int>> PerksCooldown = new();

    private static readonly Dictionary<Player, int> Exp = new();

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
    public static int GetLevel(int exp) => exp / Divider;
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