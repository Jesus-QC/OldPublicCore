using System.Collections.Generic;
using Exiled.API.Interfaces;

namespace Core.Loader.Features;

public class PriorityComparer : IComparer<ICoreModule<IConfig>>
{
    public int Compare(ICoreModule<IConfig> x, ICoreModule<IConfig> y)
    {
        var value = y.Priority.CompareTo(x.Priority);
        if (value == 0)
            value = x.GetHashCode().CompareTo(y.GetHashCode());

        return value == 0 ? 1 : value;
    }
}