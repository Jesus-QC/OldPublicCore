using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Features.Data.Enums;
using Core.Features.Data.UI;
using Core.Features.Display;
using Core.Features.Extensions;
using Core.Features.Logger;
using Core.Features.Wrappers;
using Core.Modules.Subclasses.Features;
using Exiled.API.Features;
using Hints;
using NorthwoodLib.Pools;
using UnityEngine;

namespace Core.Features.Components;

public class CorePlayerManager : MonoBehaviour
{
    private readonly Dictionary<ScreenZone, float> _timers = new() { [ScreenZone.Top] = -1, [ScreenZone.CenterTop] = -1, [ScreenZone.Center] = -1, [ScreenZone.CenterBottom] = -1, [ScreenZone.Bottom] = -1 , [ScreenZone.SubclassAlert] = -1 , [ScreenZone.InteractionMessage] = -1 , [ScreenZone.KillMessage] = -1 };
    private readonly Dictionary<ScreenZone, string> _messages = new() { [ScreenZone.Top] = string.Empty, [ScreenZone.CenterTop] = string.Empty, [ScreenZone.Center] = string.Empty, [ScreenZone.CenterBottom] = string.Empty, [ScreenZone.Bottom] = string.Empty , [ScreenZone.SubclassAlert] = string.Empty , [ScreenZone.InteractionMessage] = string.Empty , [ScreenZone.KillMessage] = string.Empty };

    private DateTime _startTime;
    private float _counter;
    private Player _player;
    private bool _dnt;
    
    private GameDisplayBuilder _mainDisplay;

    public int GetSeconds => (int)(DateTime.Now - _startTime).TotalSeconds;
    
    private void Start()
    {
        _startTime = DateTime.Now;
        _player = Player.Get(gameObject);
        _dnt = _player.DoNotTrack;

        _mainDisplay = new GameDisplayBuilder(StringBuilderPool.Shared.Rent());
        _mainDisplay.WithName($"{_player.Nickname.ToLower()} ({_player.Id})");
        
        Log.Info($"{LogUtils.GetColor(LogColor.BrightYellow)}[CorePlayerManager] Added to {LogUtils.GetColor(LogColor.Cyan)}{_player.Nickname}");
    }

    private void OnDestroy() => _mainDisplay = null;

    private void Update()
    {
        _counter += Time.deltaTime;

        if (_counter < .5f)
            return;
        
        if(MapCore.IsHudEnabled)
            DrawHud();

        _counter = 0;
    }

    private async void DrawHud()
    {
        string msg = await Task.Run(() =>
        {
            UpdateMessage();
            UpdateNotifications();

            if (Round.IsLobby)
                return _mainDisplay.BuildForLobby(_player);
                
            return _player.IsDead ? _mainDisplay.BuildForSpectator() : _mainDisplay.BuildForHuman();
        });

        _player.Connection.Send(new HintMessage(new TextHint(msg, new HintParameter[] { new StringHintParameter(string.Empty) }, null, 2)));
    }

    public void SetSubclass(Subclass s) => _mainDisplay.WithSubclass(s);

    public void AddMessage(ScreenZone zone, string message, float time = 10f)
    {
        if (zone == ScreenZone.Notifications)
        {
            _notifications.Add(new Notification(message));
            return;
        }
        
        _messages[zone] = message;
        _timers[zone] = time;
    }

    public void ClearZone(ScreenZone zone)
    {
        if (zone == ScreenZone.Notifications)
        {
            _notifications.Clear();
            return;
        }
        
        _messages[zone] = string.Empty;
        _timers[zone] = -1;
    }

    private void UpdateMessage()
    {
        string color = _player.Role.Color.ToHex();
        
        _mainDisplay.Clear();
        _mainDisplay.WithColor(color);
        _mainDisplay.WithLevelMessage(GetLevelMessage());
        _mainDisplay.WithSpectators(_player.ReferenceHub.spectatorManager.ServerCurrentSpectatingPlayers.Count - 1);
        
        for (int i = 0; i < _timers.Count; i++)
        {
            ScreenZone zone = (ScreenZone)i;

            if (_timers[zone] >= 0)
                _timers[zone] -= 0.5f;

            if (_timers[zone] < 0)
                _messages[zone] = string.Empty;

            string message = _messages[zone].TrimEnd('\n');
                
            if (string.IsNullOrEmpty(message))
                message = '\n' + message;
            
            _mainDisplay.WithContent(zone, message);
        }
    }

    private string GetLevelMessage()
    {
        if (_dnt)
            return "do not track";

        int exp = _player.GetExp();
        return $"level: {LevelExtensions.GetLevel(exp)} | next: {exp % LevelExtensions.Divider}/{LevelExtensions.Divider}";
    }

    private readonly List<Notification> _notifications = new();

    private void UpdateNotifications()
    {
        if (_dnt)
            return;

        List<string> queue = new ();

        for (int i = 0; i < (_notifications.Count > 5 ? 6 : _notifications.Count); i++)
        {
            queue.Add(_notifications[i].Message);
            _notifications[i].Duration -= 0.5f;

            if (_notifications[i].Duration <= 0)
                _notifications.Remove(_notifications[i]);
        }

        _mainDisplay.WithNotifications(queue);
    }
}