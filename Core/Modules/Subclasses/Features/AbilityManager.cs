using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Features.Logger;
using Exiled.API.Features;

namespace Core.Modules.Subclasses.Features;

public static class AbilityManager
{
    public static readonly Dictionary<Player, uint> MainCooldown = new ();
    public static readonly Dictionary<Player, uint> SecondaryCooldown = new ();
    public static readonly Dictionary<Player, uint> TertiaryCooldown = new ();

    private static CancellationTokenSource _cancellation;
    
    public static bool IsPrimaryInCooldown(this Player player)
    {
        if (MainCooldown.ContainsKey(player))
            return MainCooldown[player] > 0;

        MainCooldown.Add(player, 0);
        return false;
    }
    
    public static bool IsSecondaryInCooldown(this Player player)
    {
        if (SecondaryCooldown.ContainsKey(player))
            return SecondaryCooldown[player] > 0;

        SecondaryCooldown.Add(player, 0);
        return false;
    }
    
    public static bool IsTertiaryInCooldown(this Player player)
    {
        if (TertiaryCooldown.ContainsKey(player))
            return TertiaryCooldown[player] > 0;

        TertiaryCooldown.Add(player, 0);
        return false;
    }

    private static bool _already;
    
    private static async Task CooldownCheck()
    {
        Log.Info($"{LogUtils.GetColor(LogColor.Yellow)}Started Subclasses Cooldown Timer");
        while (true)
        {
            if (_cancellation.IsCancellationRequested)
            {
                Log.Info($"{LogUtils.GetColor(LogColor.Red)}Ended Subclasses Cooldown Timer");
                return;
            }
                
            foreach (Player p in MainCooldown.Keys.ToArray())
                if(MainCooldown[p] > 0) MainCooldown[p]--;
            foreach (Player p in SecondaryCooldown.Keys.ToArray())
                if(SecondaryCooldown[p] > 0) SecondaryCooldown[p]--;
            foreach (Player p in TertiaryCooldown.Keys.ToArray())
                if(TertiaryCooldown[p] > 0) TertiaryCooldown[p]--;
            
            await Task.Delay(1000);
        }
    }

    public static void OnRoundStarted()
    {
        if(!_cancellation.IsCancellationRequested)
            return;
        
        _cancellation?.Dispose();
        _cancellation = new CancellationTokenSource();
        Task.Run(CooldownCheck, _cancellation.Token);
    }

    public static void OnRoundRestarting()
    {
        _cancellation.Cancel();
        MainCooldown.Clear();
        SecondaryCooldown.Clear();
        TertiaryCooldown.Clear();
    }

    public static void ClearCooldown(this Player player)
    {
        if (MainCooldown.ContainsKey(player))
            MainCooldown[player] = 0;
        if (SecondaryCooldown.ContainsKey(player))
            SecondaryCooldown[player] = 0;
        if (TertiaryCooldown.ContainsKey(player))
            TertiaryCooldown[player] = 0;
    }
}