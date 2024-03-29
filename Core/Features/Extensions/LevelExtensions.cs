﻿using System;
using System.Collections.Generic;
using Core.Features.Commands;
using Core.Features.Data.Enums;
using Exiled.API.Features;

namespace Core.Features.Extensions;

public static class LevelExtensions
{
    public const int Divider = 5000;

    public static int ExpMultiplier = 1;
    
    private static readonly Dictionary<Player, Dictionary<LevelToken, int>> PerksCooldown = new();
    private static readonly Dictionary<Player, int> Exp = new();
    private static readonly Dictionary<Player, string> SpecialAdvancements = new();
    private static readonly Dictionary<Player, int> RoundExp = new();

    public static void ResetLevels()
    {
        PerksCooldown.Clear();
        Exp.Clear();
        RoundExp.Clear();
        SpecialAdvancements.Clear();
    }
        
    public static void ShowBadge(this Player player)
    {
        if (DisguiseCommand.DisguisedStaff.ContainsKey(player.UserId))
        {
            player.DisplayNickname = $"ᴸᵉᵛᵉˡ{player.GetLevel().ToString().PadLeft(3,'0')} › {DisguiseCommand.DisguisedStaff[player.UserId]}";
            return;
        }
        
        player.DisplayNickname = $"ᴸᵉᵛᵉˡ{player.GetLevel().ToString().PadLeft(3,'0')} › {player.Nickname}";
    }
    public static void SetUpLevels(this Player player)
    {
        if (player.DoNotTrack)
        {
            player.OpenReportWindow("Do Not Track: you have do not track enabled, therefore your data won't be saved, this includes info as exp and stats, in order to level up and have custom stats we recommend you disabling do not track.\n\nPress [ESC] to close this.");
            player.DisplayNickname = $"ᴰᵒᴺᵒᵗᵀʳᵃᶜᵏ › {player.Nickname}";
            return;
        }
        
        if (!PerksCooldown.ContainsKey(player))
            PerksCooldown.Add(player, new Dictionary<LevelToken, int>());

        if(!RoundExp.ContainsKey(player))
            RoundExp.Add(player, 0);

        player.ShowBadge();

        DateTime lastSeen = new((long) Core.Database.ExecuteScalar($"SELECT LastSeen FROM SlStats WHERE PlayerId='{player.GetId()}';"));

        if (lastSeen.DayOfYear < DateTime.UtcNow.DayOfYear || lastSeen.Year < DateTime.UtcNow.Year) 
            player.AddExp(LevelToken.Welcome, 50);
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
        
    public static void AddExp(this Player player, LevelToken perk, int exp)
    {
        if (player.DoNotTrack)
            return;

        player.SendHint(ScreenZone.Notifications, perk.GetString(exp));

        exp *= ExpMultiplier;
        
        if (Exp.ContainsKey(player))
            Exp[player] += exp;
        
        RoundExp[player] += exp;

        player.AddUse(perk);
        player.AddExp(exp);
    }
    public static async void AddExp(this Player player, int exp)
    {
        if(player.DoNotTrack)
            return;
            
        // var args = new AddingExpEventArgs(player, exp);
        // Levels.OnAddingExp(args);
        // if(!args.IsAllowed)
        //     return;
            
        await Core.Database.ExecuteNonQueryAsync($"UPDATE Leveling SET Exp=Exp+{exp} WHERE PlayerId={player.GetId()};");
        await Core.Database.ExecuteNonQueryAsync($"UPDATE Inventory SET Balance=Balance+{exp} WHERE PlayerId={player.GetId()};");

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
            
        int exp = (int) Core.Database.ExecuteScalar($"SELECT Exp FROM Leveling WHERE PlayerId='{player.GetId()}';");
        Exp.Add(player, exp);
        return exp;
    }

    public static int GetRoundExp(this Player player)
    {
        return RoundExp.ContainsKey(player) ? RoundExp[player] : 0;
    }
    
    public static string GetSpecialAdvancements(this Player player)
    {
        if (SpecialAdvancements.ContainsKey(player))
            return SpecialAdvancements[player];
            
        string sp = (string) Core.Database.ExecuteScalar($"SELECT Achievements FROM Leveling WHERE PlayerId='{player.GetId()}';");
        SpecialAdvancements.Add(player, sp);
        return sp;
    }
    
    public static async void SaveSpecialAdvancements(this Player player, string buffer)
    {
        SpecialAdvancements[player] = buffer;
        await Core.Database.ExecuteNonQueryAsync($"UPDATE Leveling SET Achievements='{buffer}' WHERE PlayerId='{player.GetId()}';");
    }

    public static async void AddKill(this Player player) => await Core.Database.ExecuteNonQueryAsync($"UPDATE SlStats SET Kills=Kills+1 WHERE PlayerId='{player.GetId()}';");
    public static async void AddDeath(this Player player) => await Core.Database.ExecuteNonQueryAsync($"UPDATE SlStats SET Deaths=Deaths+1 WHERE PlayerId='{player.GetId()}';");
    public static async void AddEscape(this Player player) => await Core.Database.ExecuteNonQueryAsync($"UPDATE SlStats SET Escapes=Escapes+1 WHERE PlayerId='{player.GetId()}';");
    public static async void AddTimeMvp(this Player player) => await Core.Database.ExecuteNonQueryAsync($"UPDATE SlStats SET TimesMVP=TimesMVP+1 WHERE PlayerId='{player.GetId()}';");
    public static async void AddMedicalItemUse(this Player player) => await Core.Database.ExecuteNonQueryAsync($"UPDATE SlStats SET MedicalItemsUsed=MedicalItemsUsed+1 WHERE PlayerId='{player.GetId()}';");
    public static async void AddScpItemUse(this Player player) => await Core.Database.ExecuteNonQueryAsync($"UPDATE SlStats SET SCPItemsUsed=SCPItemsUsed+1 WHERE PlayerId='{player.GetId()}';");
}