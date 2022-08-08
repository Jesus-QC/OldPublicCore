using System;
using Core.Features.Extensions;
using Exiled.API.Features;

namespace Core.Features.Handlers;

public class ServerHandler
{
    public void OnRestartingRound()
    {
        try
        {
            foreach (var player in Player.List)
                player.Goodbye();
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
            
        PlayerExtensions.ClearHubs();
        LevelExtensions.ResetLevels();
        LevelExtensions.ExpMultiplier = DateTime.Today.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday or DayOfWeek.Friday ? 2 : 1;
    }
}