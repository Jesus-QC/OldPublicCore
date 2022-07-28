using System.ComponentModel;
using Exiled.API.Interfaces;

namespace Core.Modules.Lights;

public class LightsConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;

    public string CassieMessage { get; set; } = "generator .g3 malfunction detected .g4 .g3 .g3 .g4";

    [Description("This will be added to the random time the first time it rans.")]
    public int MinStartOffset { get; set; } = 60;
    [Description("Random intervals to turn off all lights")]
    public int MinRandomInterval { get; set; } = 40;
    public int MaxRandomInterval { get; set; } = 400;

    [Description("Random intervals of light.")]
    public int MinRandomBlackoutTime { get; set; } = 10;
    public int MaxRandomBlackoutTime { get; set; } = 100;
}