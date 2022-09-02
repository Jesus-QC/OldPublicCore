using System.Collections.Generic;
using Exiled.API.Features;

namespace Core.Modules.Subclasses.Features.Extensions;

public static class PlayerExtensions
{
    private static readonly Dictionary<Player, Subclass> SubclassesByPlayer = new ();

    public static void SetSubclass(this Player player, Subclass subclass)
    {
        if(!SubclassesByPlayer.ContainsKey(player))
            SubclassesByPlayer.Add(player, null);

        SubclassesByPlayer[player] = subclass;
        Log.Info($"{player.Nickname} is now a {subclass?.Name ?? "null"} subclass.");
    }
    
    public static Subclass GetSubclass(this Player player)
    {
        if (!SubclassesByPlayer.ContainsKey(player))
            return null;

        return SubclassesByPlayer[player];
    }
}