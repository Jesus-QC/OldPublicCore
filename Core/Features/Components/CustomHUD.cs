using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Features.Data.Enums;
using Core.Features.Data.UI;
using Core.Features.Extensions;
using Exiled.API.Features;
using Hints;
using NorthwoodLib.Pools;
using UnityEngine;

namespace Core.Features.Components;

public class CustomHUD : MonoBehaviour
{
    private const string DefaultHUD = "<size=90%><line-height=90%><voffset=9.25em><size=60%>[5]</size><align=right>[0]</align>[1][2][3][4]<size=60%>[9]";
    private static readonly Dictionary<int, int> MessageLines = new() { [0] = 6, [1] = 6, [2] = 6, [3] = 6, [4] = 6, [5] = 1 };
        
    private readonly Dictionary<int, float> _timers = new() { [0] = -1, [1] = -1, [2] = -1, [3] = -1, [4] = -1, [5] = -1 };
    private readonly Dictionary<int, string> _messages = new() { [0] = string.Empty, [1] = string.Empty, [2] = string.Empty, [3] = string.Empty, [4] = string.Empty, [5] = string.Empty };

    private float _counter;
    private Player _player;
    private bool _dnt;
    private string _cachedMsg;

    private StringBuilder _builder;
    private StringBuilder _secondaryBuilder;

    private void Start()
    {
        _builder = StringBuilderPool.Shared.Rent();
        _secondaryBuilder = StringBuilderPool.Shared.Rent();
        _player = Player.Get(gameObject);
        _dnt = _player.DoNotTrack;
        _cachedMsg = $"thewolfpack | {Core.GlobalVersion} | {_player.Nickname.ToLower()} ({_player.Id})";
    }

    private void OnDestroy()
    {
        StringBuilderPool.Shared.Return(_builder);
        StringBuilderPool.Shared.Return(_secondaryBuilder);
    }

    private void Update()
    {
        _counter += Time.deltaTime;

        if (_counter < .5f)
            return;

        DrawHud();
        
        _counter = 0;
    }

    private async void DrawHud()
    {
        var msg = await Task.Run(() =>
        {
            UpdateNotifications();
            return GetMessage();
        });
        
        _player.Connection.Send(new HintMessage(new TextHint(msg, new HintParameter[] { new StringHintParameter(string.Empty) }, null, 2)));
    }

    public void AddMessage(ScreenZone zone, string message, float time = 10f)
    {
        if (zone == ScreenZone.Notifications)
        {
            _notifications.Add(new Notification(message));
            return;
        }
        
        var i = (int) zone;
        _messages[i] = message;
        _timers[i] = time;
    }

    public void ClearZone(ScreenZone zone)
    {
        if (zone == ScreenZone.Notifications)
        {
            _notifications.Clear();
            return;
        }
        
        var i = (int) zone;
        _messages[i] = string.Empty;
        _timers[i] = -1;
    }

    private string GetMessage()
    {
        _builder.Clear();
        _builder.Append(DefaultHUD);

        _builder = _builder.Replace("[9]", $"<color={_player.Role.Color.ToHex()}>{_cachedMsg} | {GetLevelMessage()}  | tps: {Server.Tps}");
        _builder = _builder.Replace("[0]", FormatStringForHud(_messages[0], MessageLines[0]));

        for (var i = 1; i < _timers.Count; i++)
        {
            if (_timers[i] >= 0)
                _timers[i] -= 0.5f;

            if (_timers[i] < 0)
                _messages[i] = string.Empty;

            var message = _messages[i].TrimEnd('\n');
                
            if (string.IsNullOrEmpty(message))
                message = '\n' + message;
                
            _builder = _builder.Replace($"[{i}]", FormatStringForHud(message, MessageLines[i]));
        }

        return _builder.ToString();
    }
        
    private string FormatStringForHud(string text, int linesNeeded)
    {
        _secondaryBuilder.Clear();
        _secondaryBuilder.Append(text);
           
        var textLines = text.Count(x => x == '\n');

        for (var i = 0; i < linesNeeded - textLines; i++)
            _secondaryBuilder.Append('\n');

        return _secondaryBuilder.ToString();
    }

    private string GetLevelMessage()
    {
        if (_dnt)
            return "do not track";
        
        var exp = _player.GetExp();
        return $"level: {LevelExtensions.GetLevel(exp)} | next: {exp % LevelExtensions.Divider}/{LevelExtensions.Divider}";
    }

    private readonly List<Notification> _notifications = new();

    private void UpdateNotifications()
    {
        if (_dnt)
            return;

        _builder.Clear();
        for (int i = 0; i < (_notifications.Count > 5 ? 6 : _notifications.Count); i++)
        {
            _builder.Append(_notifications[i].Message + "\n");
            _notifications[i].Duration -= 0.5f;
            
            if(_notifications[i].Duration <= 0)
                _notifications.RemoveAt(0);
        }

        _messages[0] = _builder.ToString().TrimEnd('\n');
    }
}