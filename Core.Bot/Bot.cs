using System.Net.Sockets;
using Discord;
using Discord.WebSocket;

namespace Core.Bot;

public class Bot
{
    private readonly TcpListener _tcpListener;
    private readonly DiscordSocketClient _client;

    private byte _playerCount;
    private byte _status;
    
    public Bot(int port)
    {
        _tcpListener = TcpListener.Create(port);
        _client = new DiscordSocketClient();
        _client.Ready += async () => await _client.SetStatusAsync(UserStatus.Idle);
    }
    
    public async Task Run()
    {
        await _client.LoginAsync(TokenType.Bot, "ODYzMDEzNjA0MTI1OTY2MzU2.GQ0khu.b9sFDkM7QAq8ewwmiNwEdIvIZ8f-HTH3RsLgIg");
        await _client.StartAsync();

        _tcpListener.Start();

        Socket? s = await _tcpListener.AcceptSocketAsync();
        
        byte[] buf = new byte[2];
        while (true)
        {
            s.Receive(buf);
            _status = buf[1];
            _playerCount = buf[0];
            await RefreshStatus();
        }
    }

    private byte _oldStatus = 0;

    private async Task RefreshStatus()
    {
        _oldStatus = _playerCount;
        
        switch (_status)
        {
            case 1:
                await _client.SetActivityAsync(new Game(_playerCount + "/35"));
                break;
            case 2:
                await _client.SetActivityAsync(new Game("Restarting round..."));
                break;
            case 3:
                
                
                await _client.SetActivityAsync(new Game($"Waiting ({(_playerCount == 0 && _oldStatus != 0 ? _oldStatus : _playerCount)}/35)"));
                break;
        }
    }
}