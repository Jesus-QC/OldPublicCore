using System.Collections.Generic;
using Core.Modules.Subclasses.Features.Structs.Subclasses;
using Exiled.API.Features;
using MEC;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Extensions;

public static class PlayerExtensions
{
    public static void RemoveSubclass(this Player player)
    {
            
    }

    public static Subclass GetSubclass(this Player player)
    {
        if (!CoreSubclasses.PlayerManager.Subclasses.ContainsKey(player.UserId))
            return null;

        return CoreSubclasses.SubclassesManager.GetSubclassById(CoreSubclasses.PlayerManager.Subclasses[player.UserId]);
    }

    public static void SetRandomSubclass(this Player player, RoleType role)
    {
        if(player.GetSubclass() != null)
            player.RemoveSubclass();
            
        var subclass = role.GetAvailableSubclasses().GetRandomSubclass(RarityExtensions.GetRandomRarity(), out var id);
        player.SetSubclass(id, subclass);
    }

    public static void SetSubclass(this Player player, ushort subclassId, Subclass s, bool force = false)
    {
        if (force)
        {
            player.SetRole(s.SpawnAs);
            player.SetSubclass(subclassId, s);
            return;
        }
            
        CoreSubclasses.ServerHandler.Coroutines.Add(Timing.RunCoroutine(SetPlayerSubclass(player, subclassId, s)));
    }

    private static IEnumerator<float> SetPlayerSubclass(Player player, ushort subclassId, Subclass s)
    {
        if (CoreSubclasses.PlayerManager.Subclasses.ContainsKey(player.UserId))
            CoreSubclasses.PlayerManager.Subclasses.Remove(player.UserId);
            
        CoreSubclasses.PlayerManager.Subclasses.Add(player.UserId, subclassId);
            
        player.ClearBroadcasts();
        player.Broadcast(10, $"<b><i>({s.Rarity}) {s.Name}</i></b>\n{s.Description}");
            
        yield return Timing.WaitForSeconds(0.25f);
            
        player.ResetInventory(s.Inventory.ToList());
        player.Position = Vector3.zero;
    }
}