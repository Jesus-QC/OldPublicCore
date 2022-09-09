using System;
using CommandSystem;
using Core.Features.Extensions;
using Exiled.API.Features;

namespace Core.Features.Commands.DiscordSync;

[CommandHandler(typeof(ClientCommandHandler))]
public class DiscordDeSyncCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        Player ply = Player.Get(sender);
            
        if (Core.Database.ExecuteScalar($"SELECT DiscordId FROM NewPlayers WHERE Id='{ply.GetId()}'") is DBNull)
        {
            ply.Broadcast(10, "Your account is not synced.");
            response = "Your account is not synced, if you want to sync it, write .sync";
            return true;
        }
            
        Core.Database.ExecuteNonQuery($"UPDATE NewPlayers SET DiscordId=NULL WHERE Id='{ply.GetId()}'");
        ply.Broadcast(10, "Desynced!");
        response = "Desynced!";
        return true;
    }

    public string Command { get; } = "desync";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Lets you desync your account with the discord one.";
}