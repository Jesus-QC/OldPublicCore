using System.Collections.Generic;
using Core.Features.Data.Configs;
using Core.Loader.Features;
using Core.Modules.Stalky.Components;
using Exiled.Events.EventArgs;
using Exiled.Events.Handlers;
using EPlayer = Exiled.Events.Handlers.Player;
using Player = Exiled.API.Features.Player;

namespace Core.Modules.Stalky
{
    public class StalkyModule : CoreModule<StalkyConfig>
    {
        public override string Name { get; } = "Stalky";

        public static readonly Dictionary<Player, StalkController> Controllers = new ();

        public static bool AreTeslasEnabled = true;

        public static StalkyConfig ModuleConfig;
        
        public override void OnEnabled()
        {
            ModuleConfig = Config;
            
            EPlayer.ChangingRole += OnChangingRole;
            EPlayer.TriggeringTesla += OnTriggeringTesla;
            Scp106.CreatingPortal += OnCreatingPortal;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EPlayer.ChangingRole -= OnChangingRole;
            EPlayer.TriggeringTesla -= OnTriggeringTesla;
            Scp106.CreatingPortal -= OnCreatingPortal;

            ModuleConfig = null;
            
            base.OnDisabled();
        }

        private void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if(ev.Player.Role != RoleType.Scp106)
                return;

            ev.IsTriggerable = AreTeslasEnabled;
        }
        
        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if(ev.NewRole != RoleType.Scp106)
                return;
            
            if(ev.Player.GameObject.TryGetComponent(out StalkController _))
                return;

            ev.Player.GameObject.AddComponent<StalkController>();
        }

        private void OnCreatingPortal(CreatingPortalEventArgs ev) => ev.IsAllowed = Controllers[ev.Player].Stalk();
    }
}