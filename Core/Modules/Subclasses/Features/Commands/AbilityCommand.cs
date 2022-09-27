using System;
using CommandSystem;
using Core.Modules.Subclasses.Features.Extensions;
using Exiled.API.Features;

namespace Core.Modules.Subclasses.Features.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class AbilityCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1 || !Player.TryGet(sender, out Player player))
        {
            response = "Usage: .ability 1/2/3";
            return false;
        }

        switch (arguments.At(0))
        {
            case "1":
                player.TryMainAbility();
                break;
            case "2":
                player.TrySecondaryAbility();
                break;
            case "3":
                player.TryTertiaryAbility();
                break;
            default:
                response = $"Ability {arguments.At(0)} not found.";
                return false;
        }

        response = "Done";
        return true;
    }

    public string Command { get; } = "ability";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Uses the ability of your subclass";
}