using Core.Loader.Features;
using Exiled.Events.EventArgs;
using Exiled.Events.Handlers;
using RemoteAdmin;

namespace Core.Modules.AfkChecker;

[DisabledModule]
public class AfkCheckerModule : CoreModule<AfkCheckerConfig>
{
    public override string Name { get; } = "AfkChecker";

    public static AfkCheckerConfig StaticConfig;
    
    public override void OnEnabled()
    {
        StaticConfig = Config;

        Player.Verified += OnVerified;
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Player.Verified -= OnVerified;
        
        StaticConfig = null;
        
        base.OnDisabled();
    }

    private void OnVerified(VerifiedEventArgs ev)
    {
        if (CommandProcessor.CheckPermissions(ev.Player.Sender, PlayerPermissions.AFKImmunity) || !ev.Player.GameObject.TryGetComponent(out AfkCheckerComponent _))
            return;
        
        ev.Player.GameObject.AddComponent<AfkCheckerComponent>().Player = ev.Player;
    }
}