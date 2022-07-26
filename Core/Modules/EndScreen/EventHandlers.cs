using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace Core.Modules.EndScreen
{
    public class EventHandlers
    {
        public void OnVerified(VerifiedEventArgs ev)
        {
            if(!_killsCounter.ContainsKey(ev.Player))
                _killsCounter.Add(ev.Player, 0);
            if(!_damageCounter.ContainsKey(ev.Player))
                _damageCounter.Add(ev.Player, 0);
        }

        public void OnEndedRound(RoundEndedEventArgs ev)
        {
            Map.ClearBroadcasts();
            Map.Broadcast(20,GetFirstScreen());
            
            _killsCounter.Clear();
            _damageCounter.Clear();
            _firstEscapist = null;
        }

        #region kills
        
        private readonly Dictionary<Player, int> _killsCounter = new();

        public void OnDied(DiedEventArgs ev)
        {
            if(ev.Killer == null || ev.Target == null || ev.Target == ev.Killer)
                return;
            
            if(!_killsCounter.ContainsKey(ev.Killer))
                _killsCounter.Add(ev.Killer, 0);

            _killsCounter[ev.Killer]++;
        }
        
        #endregion

        #region damage
        
        private readonly Dictionary<Player, int> _damageCounter = new();

        public void OnHurting(HurtingEventArgs ev)
        {
            if(ev.Attacker == null || ev.Target == null || ev.Target == ev.Attacker || ev.Attacker.Role.Team == Team.SCP)
                return;
            
            if(!_damageCounter.ContainsKey(ev.Attacker)) 
                _damageCounter.Add(ev.Attacker, 0);

            _damageCounter[ev.Attacker] += (int)ev.Amount;
        }
        
        #endregion

        #region escape

        private Player _firstEscapist;

        public void OnEscaping(EscapingEventArgs ev)
        {
            if(ev.Player == null)
                return;
            
            if (_firstEscapist == null)
                _firstEscapist = ev.Player;
        }

        #endregion

        private Player GetMax(Dictionary<Player, int> dictionary)
        {
            var maxAmount = 0;
            Player maxPlayer = null;

            foreach (var pair in dictionary)
            {
                if (pair.Value <= maxAmount) 
                    continue;
                maxAmount = pair.Value;
                maxPlayer = pair.Key;
            }

            return maxPlayer;
        }

        private string GetFirstScreen()
        {
            var kills = GetMax(_killsCounter);
            var damage = GetMax(_damageCounter);

            var killsText = kills == null ? "<color=#baffe8>Nobody</color> did any <color=#ffbaf8>kill</color>" : $"<color=#baffe8>{kills.Nickname}</color> made a total of <color=#ffbaf8>{_killsCounter[kills]} kills</color>";
            var damageText = damage == null ? "<color=#fc90e5>Nobody</color> did <color=#fcc390>damage</color>" : $"<color=#fc90e5>{damage.Nickname}</color> did a total of <color=#fcc390>{_damageCounter[damage]} of damage</color>";
            var escapeText = _firstEscapist == null ? "<color=#6cd0f5>Nobody</color> did <color=#86eb5b>escape</color>" : $"<color=#6cd0f5>{_firstEscapist.Nickname}</color> was the first one to <color=#86eb5b>escape</color>";
            var scpKillsText = RoundSummary.KilledBySCPs < 1 ? "<color=#854ede>SCPs</color> made no <color=#ed3b41>kills</color>" : $"<color=#854ede>SCPs </color>made a total of <color=#ed3b41>{RoundSummary.KilledBySCPs} human kills</color>";
            
            return $"<size=28><b>✮ MVP SCREEN ✮</b>\n«────── « ⋅ʚ♡ɞ⋅ » ──────»\n{killsText}\n{damageText}\n{escapeText}\n{scpKillsText}\n</size>";
        }
    }
}