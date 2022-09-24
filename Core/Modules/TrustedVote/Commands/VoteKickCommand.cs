using System;
using System.Threading.Tasks;
using CommandSystem;
using Core.Features.Handlers;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace Core.Modules.TrustedVote.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class VoteKickCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("cursed.votekick") || !Player.TryGet(sender, out Player send))
        {
            response = "You don't have perms to do that.";
            return false;
        }
        
        if (arguments.Count != 1)
        {
            response = "Invalid syntax. Syntax: .votekick <PlayerUsername>";
            return false;
        }

        if (!Player.TryGet(arguments.At(0), out Player player))
        {
            response = "Player not found.";
            return false;
        }

        if (PollHandler.Enabled)
        {
            response = "A poll is already enabled, try it later.";
            return false;
        }
        
        PollHandler.AddPoll(send.Nickname.ToLower(), $"kick <color=red>{player.Nickname.ToLower()}</color>", 15, () => KickPlayer(player));
        response = "Done!";
        return true;
    }

    private static void KickPlayer(Player player) => player?.Kick("A poll against you has decided to kick you.\n[Kicked by a plugin]");

    public string Command { get; } = "votekick";
    public string[] Aliases { get; } = { "votek" };
    public string Description { get; } = "Starts a poll to kick a player.";
}