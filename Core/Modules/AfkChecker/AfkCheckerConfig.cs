using Exiled.API.Interfaces;

namespace Core.Modules.AfkChecker;

public class AfkCheckerConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public int AfkTime { get; set; } = 80;
}