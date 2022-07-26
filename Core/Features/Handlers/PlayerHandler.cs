using System;
using Core.Features.Data;
using Core.Features.Events.EventArgs;
using Core.Features.Extensions;
using Exiled.Events.EventArgs;

namespace Core.Features.Handlers
{
    public class PlayerHandler
    {
        public void OnVerified(VerifiedEventArgs ev)
        {
            ev.Player.Authenticate();
            ev.Player.AddToTheHub();
            ev.Player.SetUpLevels();
        }

        public void OnLeft(LeftEventArgs ev)
        {
            ev.Player.Goodbye();
            ev.Player.WipeLevels();
        }

        public void OnBanned(BannedEventArgs ev)
        {
            if(ev.Target == null)
                return;
            
            var tId = ev.Target.GetId();
            var iId = ev.Issuer?.GetId() ?? 0;
            Core.Database.ExecuteNonQuery($"INSERT INTO SlBans (PlayerId, IssuerId, Reason, Starts, Expires) VALUES ('{tId}', '{iId}', '{ev.Details.Reason}', '{DateTime.UtcNow.Ticks}', '{ev.Details.Expires}')");
        }

        public void OnKicking(KickingEventArgs ev)
        {
            if(ev.Target == null)
                return;
            
            var tId = ev.Target.GetId();
            var iId = ev.Issuer?.GetId() ?? 0;
            Core.Database.ExecuteNonQuery($"INSERT INTO SlKicks (PlayerId, IssuerId, Reason, Date) VALUES ('{tId}', '{iId}', '{ev.Reason}', '{DateTime.UtcNow.Ticks}')");
        }

        public void OnWarned(WarnedEventArgs ev)
        {
            if(ev.Player == null)
                return;
            
            var tId = ev.Player.GetId();
            var iId = ev.Issuer?.GetId() ?? 0;
            Core.Database.ExecuteNonQuery($"INSERT INTO SlWarns (PlayerId, IssuerId, Reason, Date) VALUES ('{tId}', '{iId}', '{ev.Reason}', '{DateTime.UtcNow.Ticks}')");
        }
    }
}