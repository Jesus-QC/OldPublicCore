using System;
using CommandSystem;
using Core.Features.Extensions;
using Exiled.API.Features;
using Random = UnityEngine.Random;

namespace Core.Features.Commands.DiscordSync;

[CommandHandler(typeof(ClientCommandHandler))]
public class DiscordSyncCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "This command is disabled right now.";
        return false;
        
        var ply = Player.Get(sender);
            
        if (Core.Database.ExecuteScalar($"SELECT DiscordId FROM NewPlayers WHERE Id='{ply.GetId()}'") is not DBNull)
        {
            ply.Broadcast(10, "Your account is already synced.");
            response = "Your account is already synced, if you want to desync it, write .desync";
            return true;
        }
            
        if ((int)Core.Database.ExecuteScalar($"SELECT EXISTS(SELECT Code FROM SyncQueue WHERE PlayerId='{ply.GetId()}')") == 1)
        {
            var oldCode = Core.Database.ExecuteScalar($"SELECT Code FROM SyncQueue WHERE PlayerId='{ply.GetId()}'");
            ply.ClearBroadcasts();
            ply.Broadcast(10, $"<i><color=red>Sync code:</color></i> <b>{oldCode}</b>");
            response = $"You already have a sync code. ({oldCode})";
            return true;
        }
           
        var newGeneratedCode = Random.Range(900000, 1000000);
        Core.Database.ExecuteNonQuery($"INSERT INTO SyncQueue (Code, PlayerId) VALUES ({newGeneratedCode}, {ply.GetId()})");
        ply.Broadcast(10, $"<i><color=red>Sync code:</color></i> <b><color=#03f8fc>{newGeneratedCode}</color></b>\n<b>Use</b> /sync code:{newGeneratedCode}\n<b>Inside our discord!</b>");
        response = $"Your new sync code: ({newGeneratedCode})";
        return true;
    }

    public string Command { get; } = "sync";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Lets you sync your account with the discord one.";
}