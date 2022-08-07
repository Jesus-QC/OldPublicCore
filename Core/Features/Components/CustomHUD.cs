using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    private string _cachedMsg;

    private void Start()
    {
        _player = Player.Get(gameObject);
        _cachedMsg = $"thewolfpack | {_player.Nickname.ToLower()} ({_player.Id})";
    }

    private void Update()
    {
        _counter += Time.deltaTime;

        if (_counter < .5f)
            return;
        
        UpdateNotifications();
        var msg = GetMessage();
        _player.Connection.Send(new HintMessage(new TextHint(msg, new HintParameter[]{ new StringHintParameter(string.Empty) }, null, 1)));
        _counter = 0;
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
        var builder = StringBuilderPool.Shared.Rent(DefaultHUD);
        
        builder = builder.Replace("[9]", $"<color={_player.Role.Color.ToHex()}>{_cachedMsg} | {GetLevelMessage()}  | tps: {Server.Tps}");
        builder = builder.Replace("[0]", FormatStringForHud(_messages[0], MessageLines[0]));

        for (var i = 1; i < _timers.Count; i++)
        {
            if (_timers[i] >= 0)
                _timers[i] -= 0.5f;

            if (_timers[i] < 0)
                _messages[i] = string.Empty;

            var message = _messages[i].TrimEnd('\n');
                
            if (string.IsNullOrEmpty(message))
                message = '\n' + message;
                
            builder = builder.Replace($"[{i}]", FormatStringForHud(message, MessageLines[i]));
        }
        
        return StringBuilderPool.Shared.ToStringReturn(builder);
    }
        
    private static string FormatStringForHud(string text, int linesNeeded)
    {
        var builder = new StringBuilder();
        builder.Append(text);
           
        var textLines = text.Count(x => x == '\n');

        for (var i = 0; i < linesNeeded - textLines; i++)
            builder.Append('\n');

        return builder.ToString();
    }

    private string GetLevelMessage()
    {
        if (_player.DoNotTrack)
            return "do not track";
        
        var exp = _player.GetExp();
        return $"level: {LevelExtensions.GetLevel(exp)} | next: {exp % LevelExtensions.Divider}/{LevelExtensions.Divider}";
    }

    private List<Notification> _notifications = new();

    private void UpdateNotifications()
    {
        if (_player.DoNotTrack)
            return;

        var builder = StringBuilderPool.Shared.Rent();
        
        for (int i = 0; i < (_notifications.Count > 5 ? 6 : _notifications.Count); i++)
        {
            builder.Append(_notifications[i].Message);
            _notifications[i].Duration--;
            
            if(_notifications[i].Duration <= 0)
                _notifications.RemoveAt(0);
        }

        _messages[0] = StringBuilderPool.Shared.ToStringReturn(builder).TrimEnd('\n');
    }
}