using System.Threading.Tasks;
using Exiled.API.Features;

namespace Core.Features.Wrappers;

public static class ServerCore
{
    public static double Tps;
    
    public static async Task CheckTps()
    {
        while (true)
        {
            Tps = Server.Tps;
            await Task.Delay(1000);
        }
    }
}