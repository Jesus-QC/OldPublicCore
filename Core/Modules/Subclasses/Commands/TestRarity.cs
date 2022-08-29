using System;
using CommandSystem;
using Core.Modules.Subclasses.Features.Extensions;

namespace Core.Modules.Subclasses.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class TestRarity : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var role = Enum.Parse(typeof(RoleType), arguments.At(0));
        response = ((RoleType)role).GetRandomSubclass().Name;
        return true;
    }

    public string Command { get; } = "testrarity";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Test raritu";
}