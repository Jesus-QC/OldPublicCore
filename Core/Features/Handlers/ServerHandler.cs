using System;
using Core.Features.Extensions;
using Exiled.API.Features;

namespace Core.Features.Handlers
{
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
            LevelExtensions.ResetLevelsCooldown();
            PerkExtensions.ClearCache();
        }
    }
}