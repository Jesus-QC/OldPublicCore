﻿using HarmonyLib;
using Exiled.API.Enums;
using Core.Features.Data;
using Exiled.API.Features;
using Core.Features.Handlers;
using Core.Features.Data.Configs;
using Core.Loader;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;
using CorePlayer = Core.Features.Events.Core;

namespace Core
{
    public class Core : Plugin<CoreConfig>
    {
        public override string Name { get; } = "Core";
        public override string Prefix { get; } = "core";
        public override string Author { get; } = "Jesus-QC";
        public override PluginPriority Priority { get; } = PluginPriority.Higher;

        public static Core Instance;
        public static Database Database;

        private PlayerHandler _playerHandler;
        private ServerHandler _serverHandler;
        
        private static Harmony _harmony;
        
        public override void OnEnabled()
        {
            Instance = this;
            
            _harmony = new Harmony("core.patches.all");
            _harmony.PatchAll();

            Database = new Database();
            
            RegisterEvents();
            
            ModuleLoader.Load();
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            ModuleLoader.UnLoad();
            
            UnregisterEvents();
            
            Database = null;
            
            _harmony.UnpatchAll(_harmony.Id);
            _harmony = null;

            Instance = null;
            
            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            _playerHandler = new PlayerHandler();
            _serverHandler = new ServerHandler();

            Player.Verified += _playerHandler.OnVerified;
            Player.Left += _playerHandler.OnLeft;
            Player.Banned += _playerHandler.OnBanned;
            Player.Kicking += _playerHandler.OnKicking;
            CorePlayer.Warned += _playerHandler.OnWarned;

            Server.RestartingRound += _serverHandler.OnRestartingRound;
        }
        
        private void UnregisterEvents()
        {
            Player.Verified -= _playerHandler.OnVerified;
            Player.Left -= _playerHandler.OnLeft;
            Player.Banned -= _playerHandler.OnBanned;
            Player.Kicking -= _playerHandler.OnKicking;
            CorePlayer.Warned -= _playerHandler.OnWarned;

            Server.RestartingRound -= _serverHandler.OnRestartingRound;

            _serverHandler = null;
            _playerHandler = null;
        }
    }
}