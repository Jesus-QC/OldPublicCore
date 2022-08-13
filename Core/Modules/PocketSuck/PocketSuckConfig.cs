using Exiled.API.Interfaces;

namespace Core.Modules.PocketSuck;

public class PocketSuckConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool SinkholesEnabled { get; set; } = true;
    public bool PortalEnabled { get; set; } = false;
}