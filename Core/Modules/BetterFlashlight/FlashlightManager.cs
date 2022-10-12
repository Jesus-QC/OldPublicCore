using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Features.Logger;
using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace Core.Modules.BetterFlashlight;

public class FlashlightManager
{
    private CancellationTokenSource _cancellation;

    private readonly Dictionary<ushort, int> _batteries = new();

    public void OnRestartingRound()
    {
        _batteries.Clear();
        _cancellation.Cancel();
    }

    public void OnRoundStarted()
    {
        if(!_cancellation?.IsCancellationRequested ?? false)
            return;
        
        _cancellation?.Dispose();
        _cancellation = new CancellationTokenSource();
        Task.Run(CheckFlashlights, _cancellation.Token);
    }

    private async Task CheckFlashlights()
    {
        Log.Info($"{LogUtils.GetColor(LogColor.Yellow)}Started Flashlight Timer");
        while (true)
        {
            if(_cancellation.IsCancellationRequested)
            {
                Log.Info($"{LogUtils.GetColor(LogColor.Red)}Ended Flashlight Cooldown Timer");
                return;
            }
            
            foreach (Player player in Player.List)
            {
                if(player is null || player.IsDead || player.CurrentItem is null || player.CurrentItem is not Flashlight flashlight)
                    continue;
                
                if(!flashlight.Active)
                    continue;
                    
                ushort serial = player.CurrentItem.Serial;
                
                if(!_batteries.ContainsKey(serial))
                    _batteries.Add(serial, 100);

                if (_batteries[serial] == 0)
                {
                    player.RemoveItem(flashlight);
                    player.SendHint(ScreenZone.InteractionMessage,"<color=#ff7070>Your flashlight was broken!</color>", 5);
                    _batteries.Remove(serial);
                }
                
                _batteries[serial] -= 2;
            }

            await Task.Delay(1000);
        }
    }
}