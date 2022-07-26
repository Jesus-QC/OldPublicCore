using System;
using System.Collections.Generic;
using System.Linq;
using Core.Modules.Lobby.Components;
using Core.Modules.Lobby.Enums;
using Core.Modules.Lobby.Helpers;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using GameCore;
using Hints;
using InventorySystem.Configs;
using MEC;
using Mirror;
using UnityEngine;
using Log = Exiled.API.Features.Log;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Core.Modules.Lobby
{
    public class LobbySpawner
    {
        private GameObject _lobbyLights;
        private readonly List<GameObject> _dummies = new();
        private readonly List<TeamTrigger> _triggers = new();

        private static Room _lobbyRoom;
        private static Vector3 _spawnPosition;

        private LobbyStatus _status = LobbyStatus.Close;

        private readonly Dictionary<int, RoleType> _spawnQueue = new();

        private CoroutineHandle _hudCoroutine;

        public void OnWaitingForPlayers()
        {
            _spawnQueue.Clear();
            
            _hudCoroutine = Timing.RunCoroutine(ServerHUD());

            _status = LobbyStatus.Open;
            
            GameObject.Find("StartRound").transform.localScale = Vector3.zero;

            _lobbyRoom = Room.Get(RoomType.Hcz106);
            var localPosition = _lobbyRoom.Transform;
            var localRotation = _lobbyRoom.Transform.localRotation;

            _dummies.Add(SpawnDummy(localPosition.TransformPoint(new Vector3(11.35f, -16.4f, -10.65f)), localRotation.eulerAngles + Vector3.up * -135, RoleType.ClassD, "Class-Ds"));
            _dummies.Add(SpawnDummy(localPosition.TransformPoint(new Vector3(3.35f, -16.4f, -10.65f)), localRotation.eulerAngles + Vector3.up * 135, RoleType.FacilityGuard, "Guards"));
            _dummies.Add(SpawnDummy(localPosition.TransformPoint(new Vector3(3.35f, -16.4f, -18.35f)), localRotation.eulerAngles + Vector3.up * 45, RoleType.Scientist, "Scientists"));
            _dummies.Add(SpawnDummy(localPosition.TransformPoint(new Vector3(11.35f, -16.4f, -18.35f)), localRotation.eulerAngles + Vector3.up * -45, RoleType.Scp106, "SCPs"));

            _spawnPosition = localPosition.TransformPoint(new Vector3(7.25f, -17, -14.5f));
            
            _lobbyLights = new GameObject("Lights-Lobby");

            new SimplifiedLight(localPosition.TransformPoint(new Vector3(11.35f, -18.5f, -10.65f)), Color.magenta, 2f, false, 2).Spawn(_lobbyLights.transform);
            new SimplifiedLight(localPosition.TransformPoint(new Vector3(3.35f, -18.5f, -10.65f)), Color.cyan, 2f, false, 2).Spawn(_lobbyLights.transform);
            new SimplifiedLight(localPosition.TransformPoint( new Vector3(3.35f, -18.5f, -18.35f)), Color.yellow, 2f, false, 2).Spawn(_lobbyLights.transform);
            new SimplifiedLight(localPosition.TransformPoint( new Vector3(11.35f, -18.5f, -18.35f)), Color.red, 2f, false, 2).Spawn(_lobbyLights.transform);

            _triggers.Add(SpawnTrigger(Team.CDP, localPosition.TransformPoint(new Vector3(11.35f, -16.4f, -10.65f))));
            _triggers.Add(SpawnTrigger(Team.MTF, localPosition.TransformPoint(new Vector3(3.35f, -16.4f, -10.65f))));
            _triggers.Add(SpawnTrigger(Team.RSC, localPosition.TransformPoint(new Vector3(3.35f, -16.4f, -18.35f))));
            _triggers.Add(SpawnTrigger(Team.SCP, localPosition.TransformPoint(new Vector3(11.35f, -16.4f, -18.35f))));
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            if (_status == LobbyStatus.Open)
            {
                Timing.CallDelayed(0.5f, () =>
                {
                    ev.Player.IsOverwatchEnabled = false;
                    ev.Player.SetRole(RoleType.Tutorial);
                });
                
            }
        }
        
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (_status == LobbyStatus.Open)
                ev.NewRole = RoleType.Tutorial;
            
            else if (_status == LobbyStatus.Starting)
            {
                var role = _spawnQueue.ContainsKey(ev.Player.Id) ? _spawnQueue[ev.Player.Id] : RoleType.ClassD;
                ev.NewRole = role;
                
                ev.Items.Clear();
                ev.Ammo.Clear();

                if (!StartingInventories.DefinedInventories.ContainsKey(role)) 
                    return;
                var inv = StartingInventories.DefinedInventories[role];
                ev.Items.AddRange(inv.Items);
                foreach (var am in inv.Ammo)
                    ev.Ammo.Add(am.Key, am.Value);
            }
        }

        public void OnSpawning(SpawningEventArgs ev)
        {
            if (_status == LobbyStatus.Open)
                ev.Position = _spawnPosition;
        }

        public void OnStarting()
        {
            Timing.KillCoroutines(_hudCoroutine);

            try
            {
                _status = LobbyStatus.Starting;
            
                Map.ClearBroadcasts();
                foreach(var player in Player.List)
                    player.Connection.Send(new HintMessage(new TextHint("", new HintParameter[]{ new StringHintParameter("") }, null, 1)));

                var classElections = new Dictionary<Team, List<int>>() { [Team.CDP] = new(), [Team.RSC] = new(), [Team.MTF] = new(), [Team.SCP] = new(), [Team.TUT] = new()};

                foreach (var player in Player.List)
                    classElections[GetPlayerElection(player)].Add(player.Id);

                ClearDummies();

                var classCounts = new Dictionary<Team, ushort> {[Team.CDP] = 0, [Team.RSC] = 0, [Team.MTF] = 0, [Team.SCP] = 0, [Team.TUT] = 0};
                var queue = ConfigFile.ServerConfig.GetString("team_respawn_queue", "401431403144144");

                for (int i = 0; i < Player.Dictionary.Count; i++)
                {
                    if (queue.Length == i)
                        queue += queue;
                        
                    switch (queue[i])
                    {
                        case '4':
                            classCounts[Team.CDP]++;
                            break;
                        case '3':
                            classCounts[Team.RSC]++;
                            break;
                        case '1':
                            classCounts[Team.MTF]++;
                            break;
                        case '0':
                            classCounts[Team.SCP]++;
                            break;
                    }
                }

                var notChosenIds = Player.List.Select(player => player.Id).ToList();
                var chosenTeams = new Dictionary<Team, List<int>> { [Team.CDP] = new(), [Team.RSC] = new(), [Team.MTF] = new(), [Team.SCP] = new()};

                foreach (var team in chosenTeams.Keys.ToList())
                {
                    var maxAmount = classCounts[team];

                    if (maxAmount == 0) continue;

                    var elections = classElections[team];
                
                    if (elections.Count <= maxAmount)
                    {
                        chosenTeams[team] = elections;
                        foreach (var plyId in elections)
                            notChosenIds.Remove(plyId);
                    }
                    else
                        for (int i = 0; i < maxAmount; i++)
                        {
                            var rndId = elections[Random.Range(0, elections.Count)];
                            chosenTeams[team].Add(rndId);
                            notChosenIds.Remove(rndId);
                            elections.Remove(rndId);
                        }
                }
            
                foreach (var team in chosenTeams.Keys.ToList())
                {
                    var maxAmount = classCounts[team];

                    if (maxAmount == 0) continue;

                    var emptySlots = maxAmount - chosenTeams[team].Count;

                    for (int i = 0; i < emptySlots; i++)
                    {
                        var rndId = notChosenIds[Random.Range(0, notChosenIds.Count)];
                        chosenTeams[team].Add(rndId);
                        notChosenIds.Remove(rndId);
                    }
                }

                foreach (var team in chosenTeams)
                {
                    if (team.Key == Team.SCP)
                        continue;
                
                    RoleType role;
                    switch (team.Key)
                    {
                        case Team.CDP:
                            role = RoleType.ClassD;
                            break;
                        case Team.MTF:
                            role = RoleType.FacilityGuard;
                            break;
                        case Team.RSC:
                            role = RoleType.Scientist;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    foreach (var playerId in team.Value)
                    {
                        if(!_spawnQueue.ContainsKey(playerId))
                            _spawnQueue.Add(playerId, role);
                    }
                }
            
                var scpRoles = new List<RoleType> { RoleType.Scp049, RoleType.Scp096, RoleType.Scp106, RoleType.Scp173, RoleType.Scp93953, RoleType.Scp93989 };

                if(Server.PlayerCount > 15)
                    scpRoles.Add(RoleType.Scp079);
                
                foreach (var scp in chosenTeams[Team.SCP])
                {
                    var rndScp = scpRoles[Random.Range(0, scpRoles.Count)];
                    if (_spawnQueue.ContainsKey(scp))
                        _spawnQueue.Remove(scp);
                
                    _spawnQueue.Add(scp, rndScp);
                    scpRoles.Remove(rndScp);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception catched:\n\n{e}\n\n");
            }

            Timing.CallDelayed(1, () => _status = LobbyStatus.Close);
        }

        private Team GetPlayerElection(Player player)
        {
            foreach (var t in _triggers)
            {
                if (t.ContainsPlayer(player))
                {
                    return t.team;
                }
            }
            
            return Team.TUT;
        }

        private static GameObject SpawnDummy(Vector3 pos, Vector3 rot, RoleType role, string name)
        {
            var gameObject = Object.Instantiate(NetworkManager.singleton.playerPrefab);
            var referenceHub = gameObject.GetComponent<ReferenceHub>();

            gameObject.transform.localScale = Vector3.one * 2; 
            gameObject.transform.position = pos;
            gameObject.transform.eulerAngles = rot;

            referenceHub.queryProcessor.PlayerId = 9999;
            referenceHub.queryProcessor.NetworkPlayerId = 9999;
            referenceHub.queryProcessor._ipAddress = "127.0.0.WAN";

            referenceHub.characterClassManager.CurClass = role;
            referenceHub.characterClassManager.GodMode = true;

            referenceHub.nicknameSync.Network_myNickSync = name;

            NetworkServer.Spawn(gameObject);

            return referenceHub.gameObject;
        }

        private static TeamTrigger SpawnTrigger(Team team, Vector3 pos)
        {
            var trigger = new GameObject($"{team}-trigger");
            trigger.transform.position = pos;
            trigger.transform.localScale = Vector3.one * 5;
            var tt = trigger.AddComponent<TeamTrigger>();
            tt.team = team;
            return tt;
        }

        private void ClearDummies()
        {
            foreach (var dummy in _dummies.ToList())
            {
                Object.Destroy(dummy);
                _dummies.Remove(dummy);
            }

            foreach (var trigger in _triggers.ToList())
            {
                Object.Destroy(trigger);
                _triggers.Remove(trigger);
            }
            
            Object.Destroy(_lobbyLights);
        }

        private IEnumerator<float> ServerHUD()
        {
            for (;;)
            {
                var msg = GetMessage(GetStatus(RoundStart.singleton.NetworkTimer));
                foreach (var player in Player.List)
                    player.Connection.Send(new HintMessage(new TextHint(msg, new HintParameter[]{ new StringHintParameter(msg) }, null, 1.5f)));
                yield return Timing.WaitForSeconds(1);
            }
        }

        private string GetStatus(short timer)
        {
            switch (timer)
            {
                case -2:
                    return LobbyModule.LobbyConfig.ServerPaused;
                case -1:
                case 0:
                    return LobbyModule.LobbyConfig.RoundStarting;
                default:
                    return $"{timer} {LobbyModule.LobbyConfig.SecondsRemain}";
            }
        }

        private string GetMessage(string status) => $"<line-height=95%><voffset=8em><size=50%><alpha=#44><align=left>W<lowercase>olf</lowercase>P<lowercase>ack</lowercase> (Ver {Core.Instance.Version})</align><alpha=#FF></size><align=right>\n\n\n\nW<lowercase>elcome to {LobbyModule.LobbyConfig.ServerName}!</lowercase>\n\n\n </align></voffset><voffset=8em>\n\n\n<size=55>T<lowercase>he game will start soon!</lowercase></size>\n{status}</voffset><voffset=8em>\n\n\n\n\n\n\n\n</voffset><voffset=8em>\n\n\njoin our <color=#5865F2>discord</color>!\n{LobbyModule.LobbyConfig.DiscordLink}\n\n</voffset><voffset=8em>\n<sprite=12>\nT<lowercase>ype <color=#57F287>.level</color> in the console</lowercase></voffset>";
    }
}