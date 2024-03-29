﻿using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using MEC;

namespace Core.Modules.Levels.Events;

public static class LevelManager
{
    public static readonly HashSet<Player> IntercomUsedPlayers = new ();
    public static readonly HashSet<CoroutineHandle> Coroutines = new ();
    public static Dictionary<Player, HashSet<Door>> DoorsDictionary = new ();
    public static bool FirstKill = false;
        
    public static void ClearCoroutines()
    {
        foreach (CoroutineHandle c in Coroutines)
            Timing.KillCoroutines(c);
        Coroutines.Clear();
    }
        
    public static IEnumerator<float> Explorer(Player player)
    {
        int secondsPlayed = 0;
        int secondsAlive = 0;
        byte spectator = 0;
        for (;;)
        {
            yield return Timing.WaitForSeconds(1);
                
            if(player is null || !player.IsConnected)
                yield break;

            secondsPlayed++;

            if (secondsPlayed is 300)
            {
                player.AddExp(LevelToken.Awaken, 20);
                secondsPlayed = 0;
            }

            if (player.Role == RoleType.Spectator)
            {
                spectator++;

                if (spectator is 240)
                {
                    spectator = 0;
                    player.AddExp(LevelToken.Stalker, 50);
                }
                
                secondsAlive = 0;
                continue;
            }
            
            secondsAlive++;

            if (secondsAlive == 1200)
            {
                player.AddExp(LevelToken.Survivor, 100);
                secondsAlive = 0;
            }
        }
    }
}