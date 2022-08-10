using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Loader.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using UnityEngine;

namespace Core.Modules.SCP1162;

public class Scp1162Module : CoreModule<Scp1162Config>
{
    public override string Name => "Scp1162";

    public override void OnEnabled()
    {
        Exiled.Events.Handlers.Player.DroppingItem += OnDroppingItem;
        Exiled.Events.Handlers.Server.RoundStarted += OnStartedRound;
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingItem;
        Exiled.Events.Handlers.Server.RoundStarted -= OnStartedRound;
        
        base.OnDisabled();
    }

    private Vector3 _cachedPos = Vector3.zero;

    private void OnStartedRound()
    {
        _cachedPos = Exiled.API.Extensions.RoleExtensions.GetRandomSpawnProperties(RoleType.Scp173).Item1;
    }
    
    private void OnDroppingItem(DroppingItemEventArgs ev)
    {
        if (!ev.IsAllowed || Vector3.Distance(ev.Player.Position, _cachedPos) > 8.2f)
            return;
        
        ev.Player.SendHint(ScreenZone.Center, Config.ItemDropMessage, Config.ItemDropMessageDuration);
        ev.IsAllowed = false;
        ev.Player.RemoveItem(ev.Item);
        if(ev.Player.CheckCooldown(LevelToken.Gambling, 3))
            ev.Player.AddExp(LevelToken.Gambling);
        var newItemType = Config.Chances[Random.Range(0, Config.Chances.Count)];
        var newItem = Item.Create(newItemType);
        ev.Player.AddItem(newItem);
        ev.Player.DropItem(newItem);
    }
}