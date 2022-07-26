using Core.Features.Logger;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace Core.Loader.Features;

public abstract class CoreModule<TConfig> : ICoreModule<TConfig> where TConfig : IConfig, new()
{
    protected CoreModule()
    {
        Name = "some module";
        Priority = 10;
    }

    public virtual string Name { get; }
    public virtual byte Priority { get; }

    public TConfig Config { get; } = new();

    public virtual void OnEnabled()
    {
        Log.Info($"{LogUtils.GetColor(LogColor.BrightGreen)}Module {LogUtils.GetColor(LogColor.BrightMagenta)}[{Name}]{LogUtils.GetColor(LogColor.BrightGreen)} has been enabled.");
    }

    public virtual void OnDisabled()
    {
        Log.Info($"{LogUtils.GetBackgroundColor(LogColor.BrightRed)}{LogUtils.GetColor(LogColor.White)}[{Name}]{LogUtils.GetColor(LogColor.Reset)}{LogUtils.GetBackgroundColor(LogColor.Reset)} {LogUtils.GetColor(LogColor.BrightRed)} has been disabled.");
    }

    public virtual void UnPatch() {}
}