using System;
using System.Collections.Generic;
using System.Linq;
using Core.Features.Data.Enums;
using Exiled.API.Interfaces;

namespace Core.Modules.Levels;

public class LevelsConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;
}