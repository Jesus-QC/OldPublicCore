using System.Linq;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Modules.Subclasses.Features.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Abilities;

public class PickPocketAbility : IAbility
{
    public SubclassAbility Ability { get; set; } = SubclassAbility.Pickpocket;
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
            if (target.Role.Side == player.Role.Side)
            {
                player.SendHint(ScreenZone.SubclassAlert,"<color=red>you can't steal players from your team</color>");
                return false;
            }

            if (target.Items.Count == 0)
            {
                player.SendHint(ScreenZone.SubclassAlert,"<color=red>the player has no items</color>");
                return true;
            }

            if (player.Items.Count == 8)
            {
                player.SendHint(ScreenZone.SubclassAlert,"<color=red>your inventory is full</color>");
                return true;
            }

            Item item = target.Items.ElementAt(Random.Range(0, target.Items.Count));
            target.RemoveItem(item);
            player.AddItem(item);
            player.SendHint(ScreenZone.SubclassAlert,"<color=green>you stole something!</color>");
            return true;
        }

        player.SendHint(ScreenZone.SubclassAlert,"<color=red>target to pickpocket not found</color>");
        return false;
    }
}