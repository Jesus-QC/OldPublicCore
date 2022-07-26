using System;
using System.Collections.Generic;
using Exiled.API.Features;
using UnityEngine;

namespace Core.Modules.Lobby.Components
{
    public class TeamTrigger : MonoBehaviour
    {
        public Team team;
        private string _name;
        
        private readonly List<Player> _players = new();

        public void Start()
        {
            var col = gameObject.AddComponent<CapsuleCollider>();
            col.isTrigger = true;

            _name = $"{LobbyModule.LobbyConfig.TeamSelected} <i>{GetTeamName(team)}</i>";
        }


        private void OnTriggerEnter(Collider other)
        {
            try
            {
                var ply = Player.Get(other.gameObject);
                if (ply != null)
                {
                    if(_players.Contains(ply))
                        return;
                
                    _players.Add(ply);
                    ply.ClearBroadcasts();
                    for (int i = 0; i < 10; i++)
                    {
                        ply.Broadcast(100, _name);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var ply = Player.Get(other.gameObject);
            if (ply != null)
            {
                if (_players.Contains(ply))
                    _players.Remove(ply);
                
                ply.ClearBroadcasts();
            }
        }

        private static string GetTeamName(Team t)
        {
            switch (t)
            {
                case Team.MTF:
                    return LobbyModule.LobbyConfig.MtfSelected;
                case Team.CDP:
                    return LobbyModule.LobbyConfig.ClassDSelected;
                case Team.RSC:
                    return LobbyModule.LobbyConfig.ScientistsSelected;
                case Team.SCP:
                    return LobbyModule.LobbyConfig.ScpsSelected;
                default:
                    return "Invalid";
            }
        }

        public bool ContainsPlayer(Player player) => _players.Contains(player);
    }
}