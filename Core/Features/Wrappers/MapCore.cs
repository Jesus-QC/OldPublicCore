using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;

namespace Core.Features.Wrappers;

public static class MapCore
{
    public static void SendHint(ScreenZone zone, string message, float duration = 10)
    {
        foreach (Player player in Player.List)
            player.SendHint(zone, message, duration);
    }
    
    public static void ClearHintZone(ScreenZone zone)
    {
        foreach (Player player in Player.List)
            player.ClearHint(zone);
    }
}