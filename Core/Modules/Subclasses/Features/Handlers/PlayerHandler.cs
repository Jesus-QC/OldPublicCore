using System.Collections.Generic;
using Core.Modules.Subclasses.Features.Extensions;
using Exiled.Events.EventArgs;

namespace Core.Modules.Subclasses.Features.Handlers
{
    public class PlayerHandler
    {
        public readonly Dictionary<string, ushort> Subclasses = new Dictionary<string, ushort>();

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if(ev.NewRole == RoleType.Spectator)
                return;
            
            ev.Player.SetRandomSubclass(ev.NewRole);
        }

        public void OnDied(DiedEventArgs ev) => ev.Target?.RemoveSubclass();
    }
}