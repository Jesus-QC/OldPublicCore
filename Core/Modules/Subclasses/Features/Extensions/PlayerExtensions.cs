using System.Collections.Generic;
using System.Linq;
using Core.Features.Logger;
using Exiled.API.Enums;
using Exiled.API.Extensions;
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
        Log.Info($"{LogUtils.GetColor(LogColor.Red)}{player.Nickname} {LogUtils.GetColor(LogColor.Cyan)}is now a {LogUtils.GetColor(LogColor.BrightMagenta)}{subclass?.Name ?? "null"}.");
    }
    
    public static Subclass GetSubclass(this Player player)
    {
        if (!SubclassesByPlayer.ContainsKey(player))
            return null;

        return SubclassesByPlayer[player];
    }

    public static void TryMainAbility(this Player player)
    {
        
    }
    
    public static void TrySecondaryAbility(this Player player)
    {
        
    }
    
    public static void TryTertiaryAbility(this Player player)
    {
        
    }
    
    public static void Disguise(this Player player, RoleType type, HashSet<Side> playerSide)
    {
        foreach (Player target in Player.List.Where(x => x != player && !playerSide.Contains(x.Role.Side)))
            target.SendFakeSyncVar(player.ReferenceHub.networkIdentity, typeof(CharacterClassManager), nameof(CharacterClassManager.NetworkCurClass), (sbyte)type);
    }
}