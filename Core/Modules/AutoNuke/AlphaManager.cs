using System.Collections.Generic;
using Core.Modules.AutoNuke.Features;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace Core.Modules.AutoNuke;

public class AlphaManager
{
    private readonly AutoNukeConfig _config;
    public AlphaManager(AutoNukeModule autoNukeModule) => _config = autoNukeModule.Config;
        
    private readonly List<CoroutineHandle> _coroutineHandles = new();

    private bool _canBeDisabled = true;
        
    public void OnRestartingRound()
    {
        _canBeDisabled = true;
            
        foreach (var coroutine in _coroutineHandles)
            Timing.KillCoroutines(coroutine);
            
        _coroutineHandles.Clear();
    }

    public void OnRoundStarted()
    {
        foreach (var ev in _config.Events)
            PlayAnnouncement(ev);
        _coroutineHandles.Add(Timing.RunCoroutine(Explode(_config.TimeToInitTheProcedure)));
    }

    public void OnStopping(StoppingEventArgs ev)
    {
        if(_canBeDisabled || ev.Player == null)
            return;

        ev.IsAllowed = false;
        ev.Player.ShowHint("<color=red>The automatic warhead can't be disabled.</color>", 5);
    }

    public void OnDetonated()
    {
        foreach (var coroutine in _coroutineHandles)
            Timing.KillCoroutines(coroutine);
        _coroutineHandles.Clear();
    }
        
    private void PlayAnnouncement(WarheadEvent ev) => _coroutineHandles.Add(Timing.RunCoroutine(PlayAnnouncement(ev.Time, ev.Cassie)));

    private static IEnumerator<float> PlayAnnouncement(float delay, string message)
    {
        yield return Timing.WaitForSeconds(delay);
            
        Map.TurnOffAllLights(0.5f);
            
        Cassie.Message(message);
    }

    private IEnumerator<float> Explode(float delay)
    {
        yield return Timing.WaitForSeconds(delay);

        _canBeDisabled = false;
        Map.ShowHint("<sprite=12> <color=red>the automatic alpha warhead procedure has started</color> <sprite=12>", 5);
        Warhead.Start();
    }
}