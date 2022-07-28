using System.Collections.Generic;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using MEC;

namespace Core.Modules.BetterFlashlight;

public class FlashlightManager
{
    private CoroutineHandle _mainCoroutine;
    
    public readonly Dictionary<ushort, int> Batteries = new();

    public void OnRestartingRound()
    {
        Batteries.Clear();

        if (_mainCoroutine.IsRunning)
            Timing.KillCoroutines(_mainCoroutine);
    }

    public void OnRoundStarted()
    {
        _mainCoroutine = Timing.RunCoroutine(CheckFlashlights());
    }

    private IEnumerator<float> CheckFlashlights()
    {
        while (true)
        {
            foreach (var player in Player.List)
            {
                if(player is null || player.IsDead || player.CurrentItem is null || player.CurrentItem is not Flashlight flashlight)
                    continue;
                
                if(!flashlight.Active)
                    continue;
                    
                var serial = player.CurrentItem.Serial;
                
                if(!Batteries.ContainsKey(serial))
                    Batteries.Add(serial, 100);

                if (Batteries[serial] == 0)
                {
                    player.RemoveItem(flashlight);
                    player.SendHint(ScreenZone.Bottom, "\n\n<color=#ff7070>Your flashlight was broken!</color>", 3);
                    Batteries.Remove(serial);
                }

                player.SendHint(ScreenZone.Bottom, $"\n\nBattery: <color=#ffe08c>{Batteries[serial]}%</color>", 1);
                Batteries[serial]--;
            }

            yield return Timing.WaitForSeconds(1);
        }
    }
}