using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Modules.Subclasses.Features.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Abilities;

public class HealCmdAbility : IAbility
{
    public SubclassAbility Ability { get; set; } = SubclassAbility.HealCmd;
    public uint Cooldown { get; set; } = 30;
    public bool OnUsing(Player player)
    {
        if (player.IsCuffed)
        {
            player.SendHint(ScreenZone.SubclassAlert,"<color=red>you can't do that cuffed</color>");
            return false;
        }
        
        Transform t = player.CameraTransform;
        if (Physics.Raycast(t.position, t.forward, out RaycastHit hit, 1.3f) && Player.TryGet(hit.collider.gameObject, out Player target))
        {
            if (target.Role.Side != player.Role.Side)
            {
                player.SendHint(ScreenZone.SubclassAlert,"<color=red>you can't heal players from another team</color>");
                return false;
            }

            target.Health += 30;
            player.SendHint(ScreenZone.SubclassAlert,"<color=green>you healed your teammate!</color>");
            return true;
        }

        player.SendHint(ScreenZone.SubclassAlert,"<color=red>target to heal not found</color>");
        return false;
    }
}