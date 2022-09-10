using System;
using System.Collections.Generic;
using System.Linq;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Core.Features.Wrappers;
using Core.Modules.Lobby.Components;
using Core.Modules.Lobby.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs;
using GameCore;
using InventorySystem;
using InventorySystem.Configs;
using MEC;
using UnityEngine;
using Light = Exiled.API.Features.Toys.Light;
using Log = Exiled.API.Features.Log;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Core.Modules.Lobby;

public class LobbySpawner
{
    private readonly HashSet<GameObject> _map = new();
    private readonly HashSet<TeamTrigger> _triggers = new();
    
    private static readonly Vector3 SpawnPosition = Vector3.forward * 300 + Vector3.up;

    private LobbyStatus _status = LobbyStatus.Close;

    private readonly Dictionary<int, RoleType> _spawnQueue = new();

    private CoroutineHandle _hudCoroutine;

    private readonly HashSet<Player> _overwatch = new ();

    public void OnWaitingForPlayers()
    {
        _spawnQueue.Clear();
            
        _hudCoroutine = Timing.RunCoroutine(ServerHUD());

        _status = LobbyStatus.Open;
            
        GameObject.Find("StartRound").transform.localScale = Vector3.zero;
        
        SpawnMap();
        
        _overwatch.Clear();
    }

    private void SpawnMap()
    {
        GameObject parent = new ("LobbyMap") { transform = { position = SpawnPosition} };
        Transform t = parent.transform;

        CreateToy(t, new Vector3(0,-.05f, 0), new Vector3(50,0.01f, 50), new Color(.47f,0.76f,1));
        CreateToy(t, new Vector3(0,-.03f, 0), new Vector3(30,0.01f, 30), Color.grey);
        
        Transform classD = CreateToy(t, new Vector3(0,0, 7.5f), new Vector3(4,.1f,4), RoleType.ClassD.GetColor());
        Transform scientist = CreateToy(t, new Vector3(0,0, -7.5f), new Vector3(4,.1f,4), RoleType.Scientist.GetColor());
        Transform scp = CreateToy(t, new Vector3(7.5f,0, 0), new Vector3(4,.1f,4), RoleType.Scp049.GetColor());
        Transform mtf = CreateToy(t, new Vector3(-7.5f,0, 0), new Vector3(4,.1f,4), RoleType.NtfSergeant.GetColor());
     
        _triggers.Clear();
        _triggers.Add(SpawnTrigger(Team.CDP, classD.position));
        _triggers.Add(SpawnTrigger(Team.MTF, mtf.position));
        _triggers.Add(SpawnTrigger(Team.RSC, scientist.position));
        _triggers.Add(SpawnTrigger(Team.SCP, scp.position));

        Light.Create(SpawnPosition + Vector3.up * 2).Base.transform.SetParent(t);
        Light.Create(classD.position + Vector3.up * 2).Base.transform.SetParent(t);
        Light.Create(mtf.position + Vector3.up * 2).Base.transform.SetParent(t);
        Light.Create(scientist.position + Vector3.up * 2).Base.transform.SetParent(t);
        Light.Create(scp.position + Vector3.up * 2).Base.transform.SetParent(t);

        GameObject go = new ("jump trigger")
        {
            transform =
            {
                position = SpawnPosition + Vector3.down * 20,
                localScale = new Vector3(100,0.1f,100)
            }
        };
        go.AddComponent<JumpTrigger>();
        
        _map.Add(go);
        _map.Add(parent);
    }

    private Transform CreateToy(Transform parent, Vector3 pos, Vector3 scale, Color color)
    {
        Primitive big = Primitive.Create(PrimitiveType.Cylinder, null, null, scale, false);
        Transform bigT = big.Base.transform;
        bigT.SetParent(parent);
        bigT.localPosition = pos;
        big.Color = color;
        big.Spawn();

        return bigT;
    }

    public void OnTogglingOverwatch(TogglingOverwatchEventArgs ev)
    {
        if (_status != LobbyStatus.Open)
            return;

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
            ev.Player.SendHint(ScreenZone.TopBar, LobbyModule.LobbyConfig.ServerAnnouncement);
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
                ev.Player.IsOverwatchEnabled = true;
        }
    }

    public void OnSpawning(SpawningEventArgs ev)
    {
        if (_status == LobbyStatus.Open)
            ev.Position = SpawnPosition;
    }

    public void OnStarting()
    {
        Timing.KillCoroutines(_hudCoroutine);

        try
        {
            _status = LobbyStatus.Starting;
            
            Map.ClearBroadcasts();
            MapCore.ClearHintZone(ScreenZone.Bottom);
            MapCore.ClearHintZone(ScreenZone.Center);
            MapCore.ClearHintZone(ScreenZone.Top);
            MapCore.ClearHintZone(ScreenZone.CenterTop);
            MapCore.ClearHintZone(ScreenZone.TopBar);
            MapCore.ClearHintZone(ScreenZone.TopBarSecondary);

            Dictionary<Team, List<int>> classElections = new Dictionary<Team, List<int>>() { [Team.CDP] = new(), [Team.RSC] = new(), [Team.MTF] = new(), [Team.SCP] = new(), [Team.TUT] = new()};

            foreach (Player player in Player.List)
                classElections[GetPlayerElection(player)].Add(player.Id);

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

    private Team GetPlayerElection(Player player)
    {
        foreach (TeamTrigger t in _triggers)
        {
            if (t.ContainsPlayer(player))
            {
                return t.team;
            }
        }
            
        return Team.TUT;
    }

    /*private static GameObject SpawnDummy(Vector3 pos, Vector3 rot, RoleType role, string name)
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
    }*/

    private static TeamTrigger SpawnTrigger(Team team, Vector3 pos)
    {
        GameObject trigger = new($"{team}-trigger")
        {
            transform =
            {
                position = pos,
                localScale = Vector3.one * 5
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
        string welcome = $"<u>W<lowercase>elcome to</lowercase></u>\n{LobbyModule.LobbyConfig.ServerName}\n<color=#c09ad8>(</color><color=#b7a8e2>∩</color><color=#aeb6ec>｀</color><color=#a5c4f5>-</color><color=#9cd2ff>´</color><color=#a5d6f7>)</color><color=#aed9ee>⊃</color><color=#b7dde6>━</color><color=#c0e1de>━</color><color=#c9e4d5>☆</color><color=#d2e8cd>ﾟ</color><color=#dbebc4>.</color><color=#e4efbc>*</color><color=#edf3b4>･</color><color=#f6f6ab>｡</color><color=#fffaa3>ﾟ</color>";
        string discord = $"<align=right><color=#5865F2><u></color>J<lowercase>oin our discord!</lowercase></u>\n{LobbyModule.LobbyConfig.DiscordLink}</align>";

        for (;;)
        {
            MapCore.SendHint(ScreenZone.Top, welcome, 1.2f);
            MapCore.SendHint(ScreenZone.Bottom, discord, 1.2f);
            MapCore.SendHint(ScreenZone.CenterTop, GetMessage(GetStatus(RoundStart.singleton.NetworkTimer)), 1.2f);
            yield return Timing.WaitForSeconds(0.95f);
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

    private string GetMessage(string status) => $"<align=right>T<lowercase>he game will start soon!</lowercase>\n{status}</align>";
}