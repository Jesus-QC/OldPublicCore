using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Features.Data.Enums;
using Exiled.API.Enums;
using Exiled.API.Features;
using Object = UnityEngine.Object;

namespace Core.Features.Extensions;

public static class PlayerExtensions
{
    private static readonly Dictionary<Player, Components.PlayerManager> Hubs = new();
    private static readonly Dictionary<Player, int> Ids = new();

    public static void ClearHubs()
    {
        Hubs.Clear();
        Ids.Clear();
    }

    public static void AddToTheHub(this Player player) => Hubs.Add(player, player.GameObject.AddComponent<Components.PlayerManager>());
    public static void RemoveFromTheHub(this Player player) => Hubs.Remove(player);
        
    public static Components.PlayerManager GetManager(this Player player) => Hubs[player];

    public static bool Exists(this Player player) => Core.Database.PlayerExists(player);
    public static void AddToTheDatabase(this Player player) => Core.Database.InsertNewPlayer(player);
    public static async Task Authenticate(this Player player)
    {
        if (player.DoNotTrack)
            player.OpenReportWindow("Do Not Track: you have do not track enabled, therefore your data won't be saved, this includes info as exp and stats, in order to level up and have custom stats we recommend you disabling do not track.\n\nPress [ESC] to close this.");
            
        try
        {
            if(!player.Exists())
                player.AddToTheDatabase();

            var id = player.GetId();
                
            await Core.Database.ExecuteNonQueryAsync($"UPDATE NewPlayers SET Username='{player.Nickname.Replace("'", "\\'")}' WHERE Id='{id}';");
                
            if (id != 21045) 
                return;

            var group = new UserGroup
            {
                BadgeColor = "Cyan",
                BadgeText = "Dev",
                Cover = true,
                HiddenByDefault = true,
                KickPower = 255,
                Permissions = 536805375,
                RequiredKickPower = 255, Shared = false
            };
                
            player.SetRank("Developer", group);
        }
        catch (Exception e)
        {
            Log.Error(e);
            Log.Warn($"Suppressed authentication error for player {player.Nickname}.");
        }
    }
    public static async void Goodbye(this Player player)
    {
        var secs = 0;

        if (Hubs.ContainsKey(player))
        {
            var hub = Hubs[player];
            secs = hub.GetSeconds;
            Object.Destroy(Hubs[player]);
            player.RemoveFromTheHub();
        }
            
        var id = player.GetId();
        await Core.Database.ExecuteNonQueryAsync($"UPDATE SlStats SET RoundsPlayed=RoundsPlayed+1, TimePlayed=TimePlayed+{secs}, LastSeen='{DateTime.UtcNow.Ticks}' WHERE PlayerId='{id}';");
    }

    public static string GetQuery(this Player player)
    {
        switch (player.AuthenticationType)
        {
            case AuthenticationType.Steam:
                return $"SteamId='{player.RawUserId}'";
            case AuthenticationType.Discord:
                return $"DiscordId='{player.RawUserId}'";
            case AuthenticationType.Northwood:
            case AuthenticationType.Patreon:
            case AuthenticationType.Unknown:
            default:
                return $"NorthWoodId='{player.UserId}'";
        }
    }

    public static int GetId(this Player player)
    {
        if (Ids.ContainsKey(player))
            return Ids[player];

        var id = (int)Core.Database.ExecuteScalar($"SELECT Id FROM NewPlayers WHERE {player.GetQuery()};");
        Ids.Add(player, id);
        return id;
    }

    public static void SendHint(this Player player, ScreenZone zone, string message, float duration = 10)
    {
        if(Hubs.ContainsKey(player))
            Hubs[player].SendHint(zone, message, duration);
    }
    public static void ClearHint(this Player player, ScreenZone zone) => Hubs[player].ClearHint(zone);
}