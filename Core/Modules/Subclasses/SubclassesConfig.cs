﻿using Exiled.API.Interfaces;

namespace Core.Modules.Subclasses;

public class SubclassesConfig : IConfig
{
    public bool IsEnabled { get; set; } = true;
}