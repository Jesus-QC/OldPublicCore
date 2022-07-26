using MEC;

namespace Core.Modules.Essentials.Handlers
{
    public class ServerHandler
    {
        public void OnRestartingRound()
        {
            foreach (var c in CoroutinesHandler.Coroutines)
                Timing.KillCoroutines(c);
            
            CoroutinesHandler.Coroutines.Clear();
        }
    }
}