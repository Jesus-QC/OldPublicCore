using Exiled.API.Interfaces;

namespace Core.Modules.Stalky;

public class StalkyConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public int InitialCooldown { get; set; } = 30;
    public int Cooldown { get; set; } = 30;
}