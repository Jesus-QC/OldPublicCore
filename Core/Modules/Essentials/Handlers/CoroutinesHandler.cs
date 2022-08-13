using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using MEC;
using Mirror;
using UnityEngine;

namespace Core.Modules.Essentials.Handlers;

public static class CoroutinesHandler
{
    public static readonly List<CoroutineHandle> Coroutines = new ();

    public static IEnumerator<float> CleanerCoroutine()
    {
        yield return Timing.WaitForSeconds(600);
        for(;;)
        {
            foreach (var doll in Map.Ragdolls)
                NetworkServer.Destroy(doll.GameObject);

            foreach (var pickup in Map.Pickups)
                pickup.Destroy();
                
            yield return Timing.WaitForSeconds(300);
        }
    }
        
    public static IEnumerator<float> BetterDisarm()
    {
        var escapeZone = Vector3.zero;

        for (;;)
        {
            yield return Timing.WaitForSeconds(1.5f);

            foreach (Player player in Player.List)
            {
                if (escapeZone == Vector3.zero)
                    escapeZone = player.GameObject.GetComponent<Escape>().worldPosition;

                if (!player.IsCuffed || (player.Role.Team != Team.CHI && player.Role.Team != Team.MTF) || (escapeZone - player.Position).sqrMagnitude > 400f)
                    continue;

                switch (player.Role.Type)
                {
                    case RoleType.FacilityGuard:
                    case RoleType.NtfPrivate:
                    case RoleType.NtfSergeant:
                    case RoleType.NtfCaptain:
                    case RoleType.NtfSpecialist:
                        Coroutines.Add(Timing.RunCoroutine(DropItems(player, player.Items.ToList())));
                        player.SetRole(RoleType.ChaosConscript);
                        break;
                    case RoleType.ChaosConscript:
                    case RoleType.ChaosMarauder:
                    case RoleType.ChaosRepressor:
                    case RoleType.ChaosRifleman:
                        Coroutines.Add(Timing.RunCoroutine(DropItems(player, player.Items.ToList())));
                        player.SetRole(RoleType.NtfPrivate);
                        break;
                }
            }
        }
    }
        
    public static IEnumerator<float> DropItems(Player player, IEnumerable<Item> items)
    {
        yield return Timing.WaitForSeconds(1f);

        foreach (Item item in items)
            item.Spawn(player.Position);
    }
}