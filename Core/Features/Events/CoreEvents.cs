using Core.Features.Events.EventArgs;
using Exiled.Events.Extensions;

namespace Core.Features.Events;

public class CoreEvents
{
    public static event Exiled.Events.Events.CustomEventHandler<WarnedEventArgs> Warned;
    public static void OnWarned(WarnedEventArgs ev) => Warned.InvokeSafely(ev);
}