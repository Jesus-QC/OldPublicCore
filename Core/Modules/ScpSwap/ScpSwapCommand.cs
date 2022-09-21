using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;

namespace Core.Modules.ScpSwap;

[CommandHandler(typeof(ClientCommandHandler))]
public class ScpSwapCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count == 0)
        {
            response = "Not enough arguments provided. Example usage: .scpswap 096";
            return false;
        }

        if (!Player.TryGet(sender, out Player player) || player.Role.Team != Team.SCP)
        {
            response = "You have to be a SCP to execute this command!";
            return false;
        }

        if (!IsOpened)
        {
            response = "Time to swap SCP has ended!";
            return false;
        }

        switch (arguments.At(0).ToLower())
        {
            case "accept":
            {
                if (Requests.ContainsValue(player.Role.Type))
                {
                    Player p = Requests.First(x => x.Value == player.Role.Type).Key;

                    if (p is not null)
                    {
                        RoleType role1 = player.Role.Type;
                        RoleType role2 = p.Role.Type;

                        player.SetRole(role2);
                        p.SetRole(role1);

                        response = "Done!";
                        return true;
                    }
                }

                response = "Request not found";
                return true;
            }
            default:
            {
                if (!ScpAliases.ContainsKey(arguments.At(0).ToLower()))
                {
                    response = ":\n" + ScpAliases.Keys.Aggregate("That SCP was not found, possibilities:\n",
                        (current, alias) => current + (alias + '\n'));
                    return false;
                }

                RoleType role = ScpAliases[arguments.At(0).ToLower()];

                Player p = Player.Get(role).FirstOrDefault();
                if (p is null)
                {
                    player.SetRole(role);
                    response = "Done!";
                    return true;
                }
                
                if (!Requests.ContainsKey(player))
                {
                    p.Broadcast(8, "\n<b>You have received a Swap Request by <color=");
                    Requests.Add(player, role);
                }

                response = "Sent a swap request because there is already someone with that role.";
                return true;
            }
        }
    }

    private static readonly Dictionary<Player, RoleType> Requests = new();

    private static readonly Dictionary<string, RoleType> ScpAliases = new()
    {
        {"173", RoleType.Scp173},
        {"peanut", RoleType.Scp173},
        {"939", RoleType.Scp93953},
        {"dog", RoleType.Scp93953},
        {"079", RoleType.Scp079},
        {"computer", RoleType.Scp079},
        {"106", RoleType.Scp106},
        {"larry", RoleType.Scp106},
        {"096", RoleType.Scp096},
        {"shyguy", RoleType.Scp096},
        {"049", RoleType.Scp049},
        {"doctor", RoleType.Scp049},
        {"0492", RoleType.Scp0492},
        {"zombie", RoleType.Scp0492},
    };

    public static bool IsOpened;

    public string Command { get; } = "scpswap";
    public string[] Aliases { get; } = Array.Empty<string>();
    public string Description { get; } = "Allows you to choose another scp.";
}