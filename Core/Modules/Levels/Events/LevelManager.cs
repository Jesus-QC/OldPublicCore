using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using MEC;

namespace Core.Modules.Levels.Events
{
    public static class LevelManager
    {
        public static HashSet<CoroutineHandle> Coroutines = new();
        public static bool FirstKill = false;
        
        public static void ClearCoroutines()
        {
            foreach (var c in Coroutines)
                Timing.KillCoroutines(c);
            Coroutines.Clear();
        }
        
        public static IEnumerator<float> Explorer(Player player)
        {
            var roomsVisited = new List<Room>();
            for (;;)
            {
                yield return Timing.WaitForSeconds(1.5f);
                
                if(player == null || !player.IsConnected)
                    yield break;
                
                if(!Round.IsStarted || player.Role == RoleType.Spectator)
                    continue;

                if (!roomsVisited.Contains(player.CurrentRoom))
                {
                    roomsVisited.Add(player.CurrentRoom);
                    player.AddExp(2, Perk.Adventurer);
                }
            }
        }
    }
}