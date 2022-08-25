using Core.Loader.Features;
using Exiled.Events.EventArgs;
using Exiled.Events.Handlers;

namespace Core.Modules.AfkChecker;

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
        if (ev.Player.GameObject.TryGetComponent(out AfkCheckerComponent _))
            return;
        
        ev.Player.GameObject.AddComponent<AfkCheckerComponent>().Player = ev.Player;
    }
}