using Exiled.API.Interfaces;

namespace Core.Modules.Levels
{
    public class LevelsConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}