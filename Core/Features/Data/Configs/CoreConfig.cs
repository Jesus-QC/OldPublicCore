using Exiled.API.Interfaces;

namespace Core.Features.Data.Configs
{
    public class CoreConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}