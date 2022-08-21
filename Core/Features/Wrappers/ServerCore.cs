using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Features.Wrappers;

public static class ServerCore
{
    public static double Tps;
    
    public static async Task CheckTps()
    {
        while (true)
        {
            Tps = Math.Round(1.0 / Time.smoothDeltaTime);
            await Task.Delay(1000);
        }
    }
}