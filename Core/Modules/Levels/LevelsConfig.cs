using System;
using System.Collections.Generic;
using System.Linq;
using Core.Features.Data.Enums;
using Exiled.API.Interfaces;

namespace Core.Modules.Levels;

public class LevelsConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public Dictionary<LevelToken, string> TokenNames { get; set; } = Enum.GetValues(typeof(LevelToken)).Cast<LevelToken>().ToDictionary(level => level, level => level.ToString());
    public Dictionary<LevelToken, int> TokenExp { get; set; } = Enum.GetValues(typeof(LevelToken)).Cast<LevelToken>().ToDictionary(token => token, _ => 0);
}