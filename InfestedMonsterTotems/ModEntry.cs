using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using InfestedMonsterTotems.Core;
using InfestedMonsterTotems.Integration.ContentPatcher;
using InfestedMonsterTotems.Integration.GenericModConfigMenu;
using InfestedMonsterTotems.Totems;

namespace InfestedMonsterTotems
{
    public class ModEntry : Mod
    {
        private IModHelper _helper;
        private Core.ModConfig Config;
        private TotemManager _totemManager;
        private TotemUnlockManager _totemUnlockManager;

        public override void Entry(IModHelper helper)
        {
            _helper = helper;
            
            // Load config
            Config = helper.ReadConfig<Core.ModConfig>();
            
            // Initialize managers
            _totemManager = new TotemManager(helper, Monitor);
            _totemUnlockManager = new TotemUnlockManager(helper, Monitor, Config);
            
            // Register event handlers
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Monitor.Log("Monster Totems mod launched successfully!", LogLevel.Info);
            
            // Register Content Patcher token
            var cpApi = _helper.ModRegistry.GetApi<IContentPatcherAPI>("Pathoschild.ContentPatcher");
            if (cpApi == null)
            {
                Monitor.Log("Content Patcher API not found. Some features may not work correctly.", LogLevel.Warn);
            }
            else 
            {
                cpApi.RegisterToken(ModManifest, "TotemTexture", () => 
                    new[] { $"Mods/{ModManifest.UniqueID}/Totems" } as IEnumerable<string>);
            }

            // Get Generic Mod Config Menu's API (if it's installed)
            var configMenu = _helper.ModRegistry.GetApi<Integration.GenericModConfigMenu.IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
            {
                Monitor.Log("Generic Mod Config Menu API not found.", LogLevel.Warn);
                return;
            }

            // Register config menu
            new Integration.GenericModConfigMenu.GenericModConfigMenuIntegration(
                configMenu: configMenu,
                manifest: ModManifest,
                getConfig: () => Config,
                reset: () => 
                {
                    Config = new Core.ModConfig();
                    _helper.WriteConfig(Config);
                },
                saveAndApply: () => _helper.WriteConfig(Config)
            ).Register();
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady || e.Button != SButton.MouseRight)
                return;

            var player = Game1.player;
            var itemHeld = player.CurrentItem;

            foreach (var totem in TotemRegistry.MonsterTotems)
            {
                if (itemHeld?.Name == totem.Key)
                {
                    _totemManager.UseTotem(player, totem.Key, totem.Value);
                    break;
                }
            }
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            _totemUnlockManager.CheckMonsterKillsAndUnlockRecipes();
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            _totemUnlockManager.CheckMonsterKillsAndUnlockRecipes();
        }
    }
}