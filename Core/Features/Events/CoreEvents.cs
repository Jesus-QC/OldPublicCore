using System;
using Core.Features.Events.EventArgs;
using Exiled.Events.Extensions;
using UnityEngine.Events;

namespace Core.Features.Events;

public class CoreEvents
{
    public static event Exiled.Events.Events.CustomEventHandler<WarnedEventArgs> Warned;
    public static void OnWarned(WarnedEventArgs ev) => Warned.InvokeSafely(ev);
}