using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Modules.Subclasses.Features.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Abilities;

public class DruggistAbility : IAbility
{
    public SubclassAbility Ability { get; set; } = SubclassAbility.Druggist;
    public uint Cooldown { get; set; } = 60;
    public bool OnUsing(Player player)
    {
        Item cur = player.CurrentItem;

        if (cur?.Type is not ItemType.Coin)
        {
            player.SendHint(ScreenZone.SubclassAlert, "<color=red>you have to hold a coin to use the ability.</color>");
            return false;
        }

        ItemType itemType = GetRndItem(Random.Range(0, 100));
        player.RemoveItem(cur);
        player.CurrentItem = player.AddItem(itemType);
        player.SendHint(ScreenZone.SubclassAlert, "<color=green>you successfully created a medical item.</color>");
        return true;
    }

    private static ItemType GetRndItem(int chance)
    {
        return chance switch
        {
            < 10 => ItemType.SCP500,
            < 25 => ItemType.Medkit,
            < 50 => ItemType.Adrenaline,
            _ => ItemType.Painkillers
        };
    }
}