using Exiled.API.Interfaces;

namespace Core.Features.Data.Configs
{
    public class EmptyConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}