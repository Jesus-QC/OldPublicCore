using System;
using CommandSystem;

namespace Core.Modules.Subclasses.Features.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class AbilityCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        throw new NotImplementedException();
    }

    public string Command { get; } = "ability";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Uses the ability of your subclass";
}