using System;
using CommandSystem;
using Core.Features.Handlers;
using Exiled.API.Features;

namespace Core.Features.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class VoteCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1 || !Player.TryGet(sender, out Player send))
        {
            response = "Syntax: .vote yes / .vote no";
            return false;
        }

        if (PollHandler.Voted.Contains(send.RawUserId))
        {
            response = "You already voted!";
            return false;
        }
        
        if (arguments.At(0).ToLower() is "yes")
        {
            PollHandler.Voted.Add(send.RawUserId);
            PollHandler.YesVotes++;
            response = "Voted yes!";
            return true;
        }
        
        if (arguments.At(0).ToLower() is "no")
        {
            PollHandler.Voted.Add(send.RawUserId);
            PollHandler.NoVotes++;
            response = "Voted np!";
            return true;
        }
        
        response = "Syntax: .vote yes / .vote no";
        return false;
    }

    public string Command { get; } = "vote";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Vote command.";
}