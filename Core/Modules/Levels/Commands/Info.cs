using System;
using CommandSystem;

namespace Core.Modules.Levels.Commands
{
    public class Info : ICommand
    {
        public static Info Instance { get; } = new();
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "On this server you gain xp from various tasks and objectives. DNT Players data isnt saved.";
            return true;
        }

        public string Command { get; } = "info";
        public string[] Aliases { get; } = Array.Empty<string>();
        public string Description { get; } = "Shows you information about the leveling system";
    }
}