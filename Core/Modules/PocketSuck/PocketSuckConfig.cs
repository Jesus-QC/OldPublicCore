using Exiled.API.Interfaces;

namespace Core.Modules.PocketSuck;

public class PocketSuckConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;
}