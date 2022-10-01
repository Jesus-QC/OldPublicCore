using System;
using System.Collections.Generic;
using System.Linq;
using Core.Modules.Lobby.Components;
using Core.Modules.Lobby.Enums;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using GameCore;
using InventorySystem;
using InventorySystem.Configs;
using MEC;
using Mirror;
using UnityEngine;
using Log = Exiled.API.Features.Log;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Core.Modules.Lobby;

public class LobbySpawner
{
    private readonly HashSet<TeamTrigger> _triggers = new();
    private readonly HashSet<GameObject> _map = new();
    
    private static Room _lobbyRoom;
    private static Vector3 _spawnPosition;

    private LobbyStatus _status = LobbyStatus.Close;

    private readonly Dictionary<int, RoleType> _spawnQueue = new();

    private CoroutineHandle _hudCoroutine;

    private readonly HashSet<Player> _overwatch = new ();

    public void OnWaitingForPlayers()
    {
        _spawnQueue.Clear();

        _status = LobbyStatus.Open;
            
        _hudCoroutine = Timing.RunCoroutine(ServerHUD());
        
        GameObject.Find("StartRound").transform.localScale = Vector3.zero;
        
        SpawnMap();
        
        _overwatch.Clear();
    }

    private void SpawnMap()
    {
        GameObject.Find("StartRound").transform.localScale = Vector3.zero;
        
         _lobbyRoom = Room.Get(RoomType.Hcz106);
        Transform localPosition = _lobbyRoom.Transform;
        Quaternion localRotation = _lobbyRoom.Transform.localRotation;

        _map.Add(SpawnDummy(localPosition.TransformPoint(new Vector3(11.35f, -16.4f, -10.65f)), localRotation.eulerAngles + Vector3.up * -135, RoleType.ClassD, "Class-Ds"));
        _map.Add(SpawnDummy(localPosition.TransformPoint(new Vector3(3.35f, -16.4f, -10.65f)), localRotation.eulerAngles + Vector3.up * 135, RoleType.FacilityGuard, "Guards"));
        _map.Add(SpawnDummy(localPosition.TransformPoint(new Vector3(3.35f, -16.4f, -18.35f)), localRotation.eulerAngles + Vector3.up * 45, RoleType.Scientist, "Scientists"));
        _map.Add(SpawnDummy(localPosition.TransformPoint(new Vector3(11.35f, -16.4f, -18.35f)), localRotation.eulerAngles + Vector3.up * -45, RoleType.Scp106, "SCPs"));

        _spawnPosition = localPosition.TransformPoint(new Vector3(7.25f, -17, -14.5f));

        _triggers.Add(SpawnTrigger(Team.CDP, localPosition.TransformPoint(new Vector3(11.35f, -16.4f, -10.65f))));
        _triggers.Add(SpawnTrigger(Team.MTF, localPosition.TransformPoint(new Vector3(3.35f, -16.4f, -10.65f))));
        _triggers.Add(SpawnTrigger(Team.RSC, localPosition.TransformPoint(new Vector3(3.35f, -16.4f, -18.35f))));
        _triggers.Add(SpawnTrigger(Team.SCP, localPosition.TransformPoint(new Vector3(11.35f, -16.4f, -18.35f))));
    }

    public void OnTogglingOverwatch(TogglingOverwatchEventArgs ev)
    {
        if (_status != LobbyStatus.Open)
            return;

        if (_overwatch.Contains(ev.Player))
        {
            _overwatch.Remove(ev.Player);
            return;
        }
        
        ev.IsAllowed = false;
        _overwatch.Add(ev.Player);
    }
    
    public void OnVerified(VerifiedEventArgs ev)
    {
        if (_status != LobbyStatus.Open)
            return;
        
        Timing.CallDelayed(0.5f, () =>
        {
            ev.Player.SetRole(RoleType.Tutorial);
        });
    }
        
    public void OnChangingRole(ChangingRoleEventArgs ev)
    {
        if (_status == LobbyStatus.Open)
            ev.NewRole = RoleType.Tutorial;
            
        else if (_status == LobbyStatus.Starting)
        {
            RoleType role = _spawnQueue.ContainsKey(ev.Player.Id) ? _spawnQueue[ev.Player.Id] : RoleType.ClassD;
            ev.NewRole = role;
                
            ev.Items.Clear();
            ev.Ammo.Clear();

            if (!StartingInventories.DefinedInventories.ContainsKey(role)) 
                return;
            InventoryRoleInfo inv = StartingInventories.DefinedInventories[role];
            ev.Items.AddRange(inv.Items);
            foreach (KeyValuePair<ItemType, ushort> am in inv.Ammo)
                ev.Ammo.Add(am.Key, am.Value);

            if (_overwatch.Contains(ev.Player))
            {
                ev.NewRole = RoleType.Spectator;
                Timing.CallDelayed(2, () => ev.Player.IsOverwatchEnabled = true);
            }
                
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

            Dictionary<Team, List<int>> classElections = new () { [Team.CDP] = new(), [Team.RSC] = new(), [Team.MTF] = new(), [Team.SCP] = new(), [Team.TUT] = new()};

            foreach (Player player in Player.List)
                classElections[GetPlayerElection(player,false)].Add(player.Id);

            ClearDummies();

            Dictionary<Team, ushort> classCounts = new Dictionary<Team, ushort> {[Team.CDP] = 0, [Team.RSC] = 0, [Team.MTF] = 0, [Team.SCP] = 0, [Team.TUT] = 0};
            string queue = ConfigFile.ServerConfig.GetString("team_respawn_queue", "401431403144144");

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

            List<int> notChosenIds = Player.List.Select(player => player.Id).ToList();
            Dictionary<Team, List<int>> chosenTeams = new Dictionary<Team, List<int>> { [Team.CDP] = new(), [Team.RSC] = new(), [Team.MTF] = new(), [Team.SCP] = new()};

            foreach (Team team in chosenTeams.Keys.ToList())
            {
                ushort maxAmount = classCounts[team];

                if (maxAmount == 0) continue;

                List<int> elections = classElections[team];
                
                if (elections.Count <= maxAmount)
                {
                    chosenTeams[team] = elections;
                    foreach (int plyId in elections)
                        notChosenIds.Remove(plyId);
                }
                else
                    for (int i = 0; i < maxAmount; i++)
                    {
                        int rndId = elections[Random.Range(0, elections.Count)];
                        chosenTeams[team].Add(rndId);
                        notChosenIds.Remove(rndId);
                        elections.Remove(rndId);
                    }
            }
            
            foreach (Team team in chosenTeams.Keys.ToList())
            {
                ushort maxAmount = classCounts[team];

                if (maxAmount == 0) continue;

                int emptySlots = maxAmount - chosenTeams[team].Count;

                for (int i = 0; i < emptySlots; i++)
                {
                    int rndId = notChosenIds[Random.Range(0, notChosenIds.Count)];
                    chosenTeams[team].Add(rndId);
                    notChosenIds.Remove(rndId);
                }
            }

            foreach (KeyValuePair<Team, List<int>> team in chosenTeams)
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

                foreach (int playerId in team.Value)
                {
                    if(!_spawnQueue.ContainsKey(playerId))
                        _spawnQueue.Add(playerId, role);
                }
            }
            
            List<RoleType> scpRoles = new List<RoleType> { RoleType.Scp049, RoleType.Scp096, RoleType.Scp106, RoleType.Scp173, RoleType.Scp93953, RoleType.Scp93989 };

            if(Server.PlayerCount > 15)
                scpRoles.Add(RoleType.Scp079);
                
            foreach (int scp in chosenTeams[Team.SCP])
            {
                RoleType rndScp = scpRoles[Random.Range(0, scpRoles.Count)];
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

    public Team GetPlayerElection(Player player, bool overwatch = true)
    {
        if (overwatch)
        {
            if (_overwatch.Contains(player))
                return Team.RIP;
        }

        foreach (TeamTrigger t in _triggers.Where(t => t.ContainsPlayer(player)))
        {
            return t.team;
        }
            
        return Team.TUT;
    }

    private static GameObject SpawnDummy(Vector3 pos, Vector3 rot, RoleType role, string name)
    {
        GameObject gameObject = Object.Instantiate(NetworkManager.singleton.playerPrefab);
        ReferenceHub referenceHub = gameObject.GetComponent<ReferenceHub>();

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
        GameObject trigger = new($"{team}-trigger")
        {
            transform =
            {
                position = pos,
                localScale = Vector3.one * 5.25f
            }
        };
        TeamTrigger tt = trigger.AddComponent<TeamTrigger>();
        tt.team = team;
        return tt;
    }

    private void ClearDummies()
    {
        foreach (var obj in _map.ToList())
        {
            Object.Destroy(obj);
            _map.Remove(obj);
        }

        foreach (TeamTrigger trigger in _triggers.ToList())
        {
            Object.Destroy(trigger);
            _triggers.Remove(trigger);
        }
    }
    
    private IEnumerator<float> ServerHUD()
    {
        for (;;)
        {
            LobbyModule.Status = GetStatus(RoundStart.singleton.NetworkTimer);
            yield return Timing.WaitForSeconds(1);
        }
    }

    private string GetStatus(short timer)
    {
        switch (timer)
        {
            case -2:
                return "⛔ server paused ⛔";
            case -1:
            case 0:
                return "<color=#5cc8f2>starting round</color>";
            default:
                return $"{timer} seconds remain";
        }
    }
}