using System.Collections.Generic;
using MEC;

namespace Core.Modules.Subclasses.Features.Handlers
{
    public class ServerHandler
    {
        public readonly List<CoroutineHandle> Coroutines = new();

        public void OnRestartingRound()
        {
            foreach (var coroutine in Coroutines)
                Timing.KillCoroutines(coroutine);
            Coroutines.Clear();
        }
    }
}