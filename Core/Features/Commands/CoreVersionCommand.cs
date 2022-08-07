using System;
using CommandSystem;

namespace Core.Features.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler)), CommandHandler(typeof(ClientCommandHandler)), CommandHandler(typeof(GameConsoleCommandHandler))]
public class CoreVersionCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "The server is running the version: " + Core.GlobalVersion;
        return true;
    }

    public string Command { get; } = "coreversion";
    public string[] Aliases { get; } = { "corever" };
    public string Description { get; } = "Shows the server Core version.";
}