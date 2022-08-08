using System;
using CommandSystem;

namespace Core.Modules.Levels.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class Level : ParentCommand
{
    public Level() => LoadGeneratedCommands();
        
    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new Info());
        RegisterCommand(new Show());
        RegisterCommand(new Leaderboard());
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = "Please, specify a valid subcommand! Available ones: show, leaderboard, info";
        return true;
    }

    public override string Command { get; } = "level";
    public override string[] Aliases { get; } = {"levels", "lvl"};
    public override string Description { get; } = "Leveling system commands";
}