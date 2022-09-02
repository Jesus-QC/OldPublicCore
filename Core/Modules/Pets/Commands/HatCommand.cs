using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Mirror;

namespace Core.Modules.Pets.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class HatCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("pet.hat") || !Player.TryGet(sender, out var player))
        {
            response = "You don't have enough perms to do that.";
            return true;
        }

        if (!PetsManager.Pets.ContainsKey(player))
        {
            PetsManager.SpawnHat(player);
            response = "Added your hat.";
            return true;
        }

        foreach (var pet in PetsManager.Pets[player])
            NetworkServer.Destroy(pet);
        
        PetsManager.Pets.Remove(player);
        response = "Removed your hat.";
        return true;
    }

    public string Command { get; } = "hat";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Hat command for donators.";
}