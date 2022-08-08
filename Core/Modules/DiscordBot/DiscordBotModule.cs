using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Core.Features.Data.Configs;
using Core.Loader.Features;
using Exiled.API.Features;

namespace Core.Modules.DiscordBot;

public class DiscordBotModule : CoreModule<DiscordBotConfig>
{
    public override string Name { get; } = "DiscordBot";

    private Task _botTask;
    
    public override void OnEnabled()
    {
        try
        {
            if (!string.IsNullOrEmpty(Config.Path))
            {
                ProcessStartInfo p = new ProcessStartInfo(Config.Path, (Server.Port + 2000).ToString());
                Process.Start(p);
                Log.Warn("Bot started!");
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
        
        _botTask = Task.Run(() => BotTask(Server.Port + 2000, Config.Ip));
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        _botTask.Dispose();
        
        base.OnDisabled();
    }

    private static async Task BotTask(int port, string ip)
    {
        TcpClient c = new TcpClient();
        await c.ConnectAsync(ip, port);

        while (true)
        {
            await c.GetStream().WriteAsync(new [] {(byte)Player.Dictionary.Count, GetStatus()}, 0, 2);
            await Task.Delay(20000);
        }
    }

    private static byte GetStatus()
    {
        try
        {
            if (Round.IsLobby)
                return 3;
            if (Round.IsEnded)
                return 2;
            return 1;
        }
        catch
        {
            return 1;
        }
    }
}