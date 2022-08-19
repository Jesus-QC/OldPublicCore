using System;
using CommandSystem;
using Core.Features.Wrappers;

namespace Core.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler)), CommandHandler(typeof(ClientCommandHandler)), CommandHandler(typeof(GameConsoleCommandHandler))]
public class TpsViewerCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "TPS: " + ServerCore.Tps;
        return true;
    }

    public string Command { get; } = "tpsview";
    public string[] Aliases { get; } = { "viewtps", "tps" };
    public string Description { get; } = "Shows the server's tps.";
}