using Core.Features.Events.EventArgs;
using Exiled.Events.Extensions;

namespace Core.Features.Events
{
    public static class Levels
    {
        public static event Exiled.Events.Events.CustomEventHandler<AddingExpEventArgs> AddingExp;
        public static void OnAddingExp(AddingExpEventArgs ev) => AddingExp.InvokeSafely(ev);
    }
}