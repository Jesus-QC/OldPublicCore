using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using MEC;

namespace Core.Modules.Levels.Events;

public static class LevelManager
{
    public static readonly HashSet<Player> IntercomUsedPlayers = new HashSet<Player>();
    public static readonly HashSet<CoroutineHandle> Coroutines = new();
    public static bool FirstKill = false;
        
    public static void ClearCoroutines()
    {
        foreach (var c in Coroutines)
            Timing.KillCoroutines(c);
        Coroutines.Clear();
    }
        
    public static IEnumerator<float> Explorer(Player player)
    {
        var roomsVisited = new HashSet<Room>();
        var secondsPlayed = 0;
        var secondsAlive = 0;
        for (;;)
        {
            yield return Timing.WaitForSeconds(1);
                
            if(player == null || !player.IsConnected)
                yield break;

            secondsPlayed++;

            if (secondsPlayed is 300)
            {
                player.AddExp(LevelToken.Awaken);
                secondsPlayed = 0;
            }

            secondsAlive = 0;
            
            if(!Round.IsStarted || player.Role == RoleType.Spectator)
                continue;

            if (!roomsVisited.Contains(player.CurrentRoom))
            {
                roomsVisited.Add(player.CurrentRoom);
                player.AddExp(LevelToken.Traveler);
            }

            secondsAlive++;

            if (secondsAlive == 1200)
            {
                player.AddExp(LevelToken.Survivor);
                secondsAlive = 0;
            }
        }
    }
}