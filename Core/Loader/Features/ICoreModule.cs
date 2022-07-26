using Exiled.API.Interfaces;

namespace Core.Loader.Features;

public interface ICoreModule<out TConfig> where TConfig : IConfig
{
    string Name { get; }
    byte Priority { get; }

    TConfig Config { get; }

    void OnEnabled();
    void OnDisabled();
    void UnPatch();
}