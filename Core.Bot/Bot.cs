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
        await _client.LoginAsync(TokenType.Bot, "OTA3MzU5NjkxNzIwNTE1NjE1.GKpWMo.882lt7h1SnmwM3safX8l6cWIKsijlinv_wuePs");
        await _client.StartAsync();

        _tcpListener.Start();

        var s = await _tcpListener.AcceptSocketAsync();
        
        byte[] buf = new byte[2];
        while (true)
        {
            s.Receive(buf);
            _status = buf[1];
            _playerCount = buf[0];
            await RefreshStatus();
        }
    }

    private async Task RefreshStatus()
    {
        switch (_status)
        {
            case 1:
                await _client.SetActivityAsync(new Game(_playerCount + "/40"));
                break;
            case 2:
                await _client.SetActivityAsync(new Game("Restarting round..."));
                break;
            case 3:
                await _client.SetActivityAsync(new Game($"Waiting ({_playerCount}/40)"));
                break;
        }
    }
}