using System.Collections.Generic;
using Core.Modules.AutoNuke.Features;
using Exiled.API.Interfaces;

namespace Core.Modules.AutoNuke;

public class AutoNukeConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;

    public List<WarheadEvent> Events { get; set; } = new()
    {
        new WarheadEvent()
        {
            Cassie = "pitch_0.2 .g4 jam_020_3 .g4 pitch_1 Warning . automatic Alpha jam_030_1 Warhead jam_040_2 detonation sequence .g1 jam_040_2 in tminus . 20 minutes pitch_0.2 jam_050_3 .g2",
            Time = 60
        },
        new WarheadEvent()
        {
            Cassie = "pitch_0.2 .g4 jam_020_3 .g4 pitch_1 Warning . automatic Alpha jam_030_1 Warhead jam_040_2 detonation sequence .g1 jam_040_2 in tminus . jam_040_2 10 minute pitch_0.2 jam_050_3 .g2",
            Time = 600
        },
        new WarheadEvent()
        {
            Cassie = "pitch_0.2 .g4 jam_020_3 .g4 pitch_1 Warning  . automatic Alpha jam_030_3 Warhead jam_040_3 detonation sequence . . commencing in tminus 1 minute .g1 pitch_0.2 jam_050_3 .g2",
            Time = 1140
        },

    };

    public uint TimeToInitTheProcedure { get; set; } = 1200;
}