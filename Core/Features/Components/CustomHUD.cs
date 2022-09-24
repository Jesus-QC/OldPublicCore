using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Features.Data.Enums;
using Core.Features.Data.UI;
using Core.Features.Extensions;
using Core.Features.Handlers;
using Core.Features.Wrappers;
using Exiled.API.Features;
using Hints;
using NorthwoodLib.Pools;
using UnityEngine;

namespace Core.Features.Components;

public class CustomHUD : MonoBehaviour
{
    private const string DefaultHUD = $"<size=86%><line-height=90%><voffset=9.75em><size=60%>[5][6]</size><align=right>[0]</align>[1][2][3][4]<size=60%>[8]<b><size=55%><color=#c862ff>C</color><color=#c684ff>u</color><color=#c4a7ff>r</color><color=#c1c9ff>s</color><color=#bfebff>e</color><color=#cbf0eb>d</color> <color=#e3fac4>S</color><color=#efffb0>L</color> - {Core.GlobalVersion}</size>\n[9]";
    private static readonly Dictionary<int, int> MessageLines = new() { [0] = 6, [1] = 6, [2] = 6, [3] = 6, [4] = 6 };
        
    private readonly Dictionary<int, float> _timers = new() { [0] = -1, [1] = -1, [2] = -1, [3] = -1, [4] = -1 };
    private readonly Dictionary<int, string> _messages = new() { [0] = string.Empty, [1] = string.Empty, [2] = string.Empty, [3] = string.Empty, [4] = string.Empty };

    private float _counter;
    private Player _player;
    private bool _dnt;
    private string _cachedMsg;

    private string _topBar = string.Empty;
    private string _secondaryTopBar = string.Empty;
    
    private StringBuilder _builder;
    private StringBuilder _secondaryBuilder;

    private void Start()
    {
        _builder = StringBuilderPool.Shared.Rent();
        _secondaryBuilder = StringBuilderPool.Shared.Rent();
        _player = Player.Get(gameObject);
        _dnt = _player.DoNotTrack;
        _cachedMsg = $"{_player.Nickname.ToLower()} ({_player.Id})";
        
        if (LevelExtensions.ExpMultiplier != 1)
            _cachedMsg += " | <color=#ffe669>2x XP</color>";
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
        string msg = await Task.Run(() =>
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

        if (zone is ScreenZone.TopBar)
        {
            _topBar = message;
            return;
        }

        if (zone is ScreenZone.TopBarSecondary)
        {
            _secondaryTopBar = message;
            return;
        }

        int i = (int) zone;
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
        
        if (zone is ScreenZone.TopBar)
        {
            _topBar = string.Empty;
            return;
        }

        if (zone is ScreenZone.TopBarSecondary)
        {
            _secondaryTopBar = string.Empty;
            return;
        }
        
        int i = (int) zone;
        _messages[i] = string.Empty;
        _timers[i] = -1;
    }

    private string GetMessage()
    {
        _builder.Clear();
        _builder.Append(DefaultHUD);

        string color = _player.Role.Color.ToHex();
        
        _builder = _builder.Replace("[8]", $"<color={color}>");
        _builder = _builder.Replace("[9]", $"{_cachedMsg} | {GetLevelMessage()} | tps: {ServerCore.Tps}");
        _builder = _builder.Replace("[0]", FormatStringForHud(_messages[0], MessageLines[0]));
        _builder = _builder.Replace("[5]", $"<color={color}>" + _topBar + '\n');
        _builder = _builder.Replace("[6]", _secondaryTopBar + "</color>\n");

        int i = 1;

        if (PollHandler.Enabled)
        {
            i++;
            _builder = _builder.Replace("[1]", FormatStringForHud($"<align=right><size=75%><u>poll by <color=#299>{PollHandler.PollAuthor}</color></u>\n{PollHandler.PollName}\n\n<color=green>.vote yes - {PollHandler.YesVotes}</color>\n<color=red>.vote no - {PollHandler.NoVotes}</color>\n<size=60%>time left: {PollHandler.TimeLeft}</size></size></align>", MessageLines[i]));
        }
        
        for (; i < _timers.Count; i++)
        {
            if (_timers[i] >= 0)
                _timers[i] -= 0.5f;

            if (_timers[i] < 0)
                _messages[i] = string.Empty;

            string message = _messages[i].TrimEnd('\n');
                
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
           
        int textLines = text.Count(x => x == '\n');

        for (int i = 0; i < linesNeeded - textLines; i++)
            _secondaryBuilder.Append('\n');

        return _secondaryBuilder.ToString();
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