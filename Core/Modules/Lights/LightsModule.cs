using System.Collections.Generic;
using Core.Loader.Features;
using Exiled.API.Features;
using MEC;
using UnityEngine;
using Server = Exiled.Events.Handlers.Server;

namespace Core.Modules.Lights;

public class LightsModule : CoreModule<LightsConfig>
{
    public override string Name { get; } = "Lights";

    public override void OnEnabled()
    {
        Server.RestartingRound += OnRestartingRound;
        Server.RoundStarted += OnStartingRound;
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Server.RestartingRound -= OnRestartingRound;
        Server.RoundStarted -= OnStartingRound;
        
        base.OnDisabled();
    }

    private CoroutineHandle _coroutine;
    
    private void OnRestartingRound()
    {
        if (_coroutine.IsRunning)
            Timing.KillCoroutines(_coroutine);
    }

    private void OnStartingRound()
    {
        _coroutine = Timing.RunCoroutine(Lights());
    }

    private IEnumerator<float> Lights()
    {
        yield return Timing.WaitForSeconds(Config.MinStartOffset);
        while (true)
        {
            yield return Timing.WaitForSeconds(Random.Range(Config.MinRandomInterval, Config.MaxRandomInterval));

            var time = Random.Range(Config.MinRandomBlackoutTime, Config.MaxRandomBlackoutTime);
            
            Cassie.Message(Config.CassieMessage);
            Map.TurnOffAllLights(time);

            yield return Timing.WaitForSeconds(time);
        }
    }
}