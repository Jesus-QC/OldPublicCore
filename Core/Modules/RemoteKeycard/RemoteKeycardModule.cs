using Core.Features.Data.Configs;
using Core.Loader.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Keycards;

namespace Core.Modules.RemoteKeycard;

public class RemoteKeycardModule : CoreModule<EmptyConfig>
{
    public override string Name { get; } = "RemoteKeycard";

    public override void OnEnabled()
    {
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
        Exiled.Events.Handlers.Player.ActivatingWarheadPanel += OnInteractingWarhead;
        Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingScpLocker;
        Exiled.Events.Handlers.Player.UnlockingGenerator += OnInteractingGenerator;
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
        Exiled.Events.Handlers.Player.ActivatingWarheadPanel -= OnInteractingWarhead;
        Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingScpLocker;
        Exiled.Events.Handlers.Player.UnlockingGenerator -= OnInteractingGenerator;
        
        base.OnDisabled();
    }

    private void OnInteractingDoor(InteractingDoorEventArgs ev)
    {
        if(ev.IsAllowed)
            return;
        
        foreach (var item in ev.Player.Items)
        {
            if (item is not Keycard key || !key.Base.Permissions.HasFlagFast(ev.Door.Base.RequiredPermissions.RequiredPermissions))
                continue;
            
            ev.IsAllowed = true;
        }
    }

    private void OnInteractingScpLocker(InteractingLockerEventArgs ev)
    {
        if(ev.IsAllowed)
            return;
        
        foreach (var item in ev.Player.Items)
        {
            if (item is not Keycard key || (key.Base.Permissions & ev.Chamber.RequiredPermissions) == 0)
                continue;
            
            ev.IsAllowed = true;
        }
    }

    private void OnInteractingGenerator(UnlockingGeneratorEventArgs ev)
    {
        if(ev.IsAllowed)
            return;
        
        foreach (var item in ev.Player.Items)
        {
            if (item is not Keycard key || !key.Base.Permissions.HasFlagFast(ev.Generator.Base._requiredPermission))
                continue;
            
            ev.IsAllowed = true;
        }
    }

    private void OnInteractingWarhead(ActivatingWarheadPanelEventArgs ev)
    {
        if(ev.IsAllowed)
            return;
        
        foreach (var item in ev.Player.Items)
        {
            if (item is not Keycard key || !key.Base.Permissions.HasFlagFast(KeycardPermissions.AlphaWarhead))
                continue;
            
            ev.IsAllowed = true;
        }
    }
}