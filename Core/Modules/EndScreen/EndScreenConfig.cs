using Exiled.API.Interfaces;

namespace Core.Modules.EndScreen;

public class EndScreenConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;
}