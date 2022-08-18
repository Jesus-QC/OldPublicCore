using System;
using System.Threading;
using System.Threading.Tasks;
using CommandSystem;
using Exiled.API.Features;

namespace Core.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler)), CommandHandler(typeof(ClientCommandHandler)), CommandHandler(typeof(GameConsoleCommandHandler))]
public class TpsViewerCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if(_cancellation is null)
        {
            _cancellation = new CancellationTokenSource();
            Task.Run(TpsTask, _cancellation.Token);
        }
        else
        {
            _cancellation.Cancel();   
        }
        
        response = "Switched tps task.";
        return true;
    }

    private static CancellationTokenSource _cancellation;

    private static async Task TpsTask()
    {
        while (true)
        {
            if (_cancellation.IsCancellationRequested)
            {
                _cancellation = null;
                return;
            }
            
            Log.Info($"TPS: {Server.Tps}");
            await Task.Delay(1000);
        }
    }

    public string Command { get; } = "tpsview";
    public string[] Aliases { get; } = { "viewtps", "tps" };
    public string Description { get; } = "Shows the server's tps.";
}