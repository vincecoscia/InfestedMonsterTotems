using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Objects;
using StardewValley.GameData.Crafting;
using StardewValley.Monsters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using xTile.Dimensions;

namespace InfestedMonsterTotems
{
    public class ModEntry : Mod
    {
        private Dictionary<string, string[]> monsterTotems = new Dictionary<string, string[]>
        {
            { "cakeymat.InfestedMonsterTotems_SlimeTotem", new[] { "Green Slime", "Frost Jelly", "Sludge" } },
            { "cakeymat.InfestedMonsterTotems_BugTotem", new[] { "Bug", "Cave Fly", "Grub" } },
            { "cakeymat.InfestedMonsterTotems_SkeletonTotem", new[] { "Skeleton", "Skeleton" } },
            { "cakeymat.InfestedMonsterTotems_BatTotem", new[] { "Bat", "Frost Bat", "Lava Bat" } }
        };

        private IModHelper _helper;
        private ModConfig Config;
        private Dictionary<string, HashSet<string>> monsterTypeMapping = new()
        {
            { "cakeymat.InfestedMonsterTotems_SlimeTotem", new HashSet<string> { "Green Slime", "Frost Jelly", "Sludge" } },
            { "cakeymat.InfestedMonsterTotems_BugTotem", new HashSet<string> { "Bug", "Cave Fly", "Grub" } },
            { "cakeymat.InfestedMonsterTotems_SkeletonTotem", new HashSet<string> { "Skeleton" } },
            { "cakeymat.InfestedMonsterTotems_BatTotem", new HashSet<string> { "Bat", "Frost Bat", "Lava Bat" } }
        };

        public override void Entry(IModHelper helper)
        {
            _helper = helper;
            
            // Load config
            Config = helper.ReadConfig<ModConfig>();
            
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            Monitor.Log("Monster Totems mod launched successfully!", LogLevel.Info);
            
            // Register Content Patcher token
            var api = _helper.ModRegistry.GetApi<IContentPatcherAPI>("Pathoschild.ContentPatcher");
            if (api == null)
            {
                Monitor.Log("Content Patcher API not found. Some features may not work correctly.", LogLevel.Warn);
                return;
            }

            // Updated token registration with nullable return type
            api.RegisterToken(ModManifest, "TotemTexture", () => 
                new[] { $"Mods/{ModManifest.UniqueID}/Totems" } as IEnumerable<string>);
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady || e.Button != SButton.MouseRight)
                return;

            var player = Game1.player;
            var itemHeld = player.CurrentItem;

            if (itemHeld == null)
                return;

            foreach (var totem in monsterTotems)
            {
                if (itemHeld.Name == totem.Key)
                {
                    UseTotem(player, totem.Key, totem.Value);
                    break;
                }
            }
        }

        private void UseTotem(Farmer player, string totemName, string[] monsters)
        {
            int targetLevel = 1;
            Monitor.Log($"Using {totemName} to warp to mine level {targetLevel}", LogLevel.Debug);

            // Play warp effect
            Game1.playSound("wand");
            Game1.displayFarmer = false;

            // Remove totem item
            if (player.CurrentItem.Stack > 1)
                player.CurrentItem.Stack--;
            else
                player.removeItemFromInventory(player.CurrentItem);

            // Get the mine shaft location name for the target level
            string locationName = StardewValley.Locations.MineShaft.GetLevelName(targetLevel);
            
            // If we're already in the mines, handle it differently
            if (Game1.currentLocation is StardewValley.Locations.MineShaft currentMine && 
                Game1.currentLocation.Name == locationName)
            {
                // We're already in the target level, reload it
                currentMine.loadLevel(targetLevel);
                currentMine.mustKillAllMonstersToAdvance();
                
                // Wait a tick to spawn monsters and restore player visibility
                _helper.Events.GameLoop.UpdateTicked += OnReloadTick;
                
                void OnReloadTick(object? sender, UpdateTickedEventArgs e)
                {
                    _helper.Events.GameLoop.UpdateTicked -= OnReloadTick;
                    Game1.displayFarmer = true;
                    SpawnMonsters(monsters, targetLevel);
                    Monitor.Log($"Reloaded level and respawned monsters on current mine level {targetLevel}", LogLevel.Debug);
                }
                return;
            }

            // Otherwise, handle the warp to a new mine level
            _helper.Events.Player.Warped += OnPlayerWarped;
            Game1.warpFarmer(locationName, 6, 6, false);

            void OnPlayerWarped(object? sender, WarpedEventArgs e)
            {
                _helper.Events.Player.Warped -= OnPlayerWarped;

                if (Game1.currentLocation is StardewValley.Locations.MineShaft mineShaft)
                {
                    mineShaft.loadLevel(targetLevel);
                    mineShaft.mustKillAllMonstersToAdvance();
                    
                    int ticksWaited = 0;
                    _helper.Events.GameLoop.UpdateTicked += WaitForLoad;

                    void WaitForLoad(object? sender, UpdateTickedEventArgs e)
                    {
                        ticksWaited++;
                        if (ticksWaited >= 2)
                        {
                            _helper.Events.GameLoop.UpdateTicked -= WaitForLoad;
                            Game1.displayFarmer = true;
                            SpawnMonsters(monsters, targetLevel);
                            Monitor.Log($"Warped to mine level {targetLevel} and spawned monsters after {ticksWaited} ticks", LogLevel.Debug);
                        }
                    }
                }
            }
        }

        private void SpawnMonsters(string[] monsterTypes, int mineLevel)
        {
            if (Game1.currentLocation is not StardewValley.Locations.MineShaft mineShaft)
                return;

            // Clear existing monsters properly
            mineShaft.characters.Filter(c => !(c is Monster));

            // Spawn 5-8 monsters
            int monstersToSpawn = Game1.random.Next(5, 9);
            int attempts = 0;
            int spawned = 0;

            while (spawned < monstersToSpawn && attempts < 100)
            {
                attempts++;
                string monsterType = monsterTypes[Game1.random.Next(monsterTypes.Length)];
                Vector2 spawnPosition = GetRandomSpawnPosition(mineShaft);

                // Only spawn if position is valid
                if (IsValidSpawnPosition(mineShaft, spawnPosition))
                {
                    var monster = CreateMonster(monsterType, spawnPosition, mineLevel);
                    if (monster != null)
                    {
                        mineShaft.characters.Add(monster);
                        spawned++;
                        Monitor.Log($"Spawned {monsterType} at {spawnPosition}", LogLevel.Trace);
                    }
                }
            }
            
            Monitor.Log($"Spawned {spawned} monsters after {attempts} attempts", LogLevel.Debug);
        }

        private bool IsValidSpawnPosition(GameLocation location, Vector2 position)
        {
            int tileX = (int)(position.X / 64);
            int tileY = (int)(position.Y / 64);

            Vector2 tileLocation = new Vector2(tileX, tileY);

            return location.isTileOnMap(tileX, tileY) &&
                   location.CanSpawnCharacterHere(tileLocation) &&
                   !location.isWaterTile(tileX, tileY);
        }

        private Vector2 GetRandomSpawnPosition(GameLocation location)
        {
            // Get valid tile coordinates within reasonable bounds
            int tileX = Game1.random.Next(2, location.map.GetLayer("Back").LayerWidth - 2);
            int tileY = Game1.random.Next(2, location.map.GetLayer("Back").LayerHeight - 2);

            // Convert to pixel coordinates
            return new Vector2(tileX * 64, tileY * 64);
        }

        private NPC? CreateMonster(string monsterType, Vector2 position, int mineLevel)
        {
            switch (monsterType)
            {
                case "Green Slime":
                    return new GreenSlime(position);
                case "Frost Jelly":
                    var frostJelly = new GreenSlime(position);
                    frostJelly.Name = "Frost Jelly";
                    return frostJelly;
                case "Sludge":
                    var sludge = new GreenSlime(position);
                    sludge.Name = "Sludge";
                    return sludge;
                case "Bat":
                    return new Bat(position);
                case "Frost Bat":
                    var frostBat = new Bat(position);
                    frostBat.Name = "Frost Bat";
                    return frostBat;
                case "Lava Bat":
                    var lavaBat = new Bat(position);
                    lavaBat.Name = "Lava Bat";
                    return lavaBat;
                case "Skeleton":
                    return new Skeleton(position);
                case "Bug":
                    return new Bug(position, mineLevel);
                case "Cave Fly":
                    return new Fly(position);
                case "Grub":
                    return new Grub(position);
                default:
                    return null;
            }
        }

        private void CheckMonsterKillsAndUnlockRecipes()
        {
            if (!Context.IsWorldReady)
                return;

            foreach (var totemEntry in monsterTypeMapping)
            {
                string totemName = totemEntry.Key;
                var monsterTypes = totemEntry.Value;
                
                // Create mail ID without space
                string mailId = $"{totemName}Recipe";
                
                // Skip if recipe is already known or mail was already sent
                if (Game1.player.craftingRecipes.ContainsKey(totemName) || 
                    Game1.player.mailReceived.Contains(mailId) ||
                    Game1.player.mailForTomorrow.Contains(mailId) ||
                    Game1.player.mailbox.Contains(mailId))
                    continue;

                // Get total kills for all related monster types
                int totalKills = 0;
                foreach (var monsterType in monsterTypes)
                {
                    totalKills += Game1.stats.getMonstersKilled(monsterType);
                }

                // Check if kills meet requirement
                if (totalKills >= Config.TotemUnlockRequirements[totemName])
                {
                    // Add mail for tomorrow
                    Game1.addMailForTomorrow(mailId);
                    
                    // Log only once when queuing the mail
                    Monitor.Log($"Queued mail '{mailId}' to be delivered tomorrow after killing {totalKills} monsters", LogLevel.Info);
                }
            }
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            // Check every second (60 ticks)
            if (e.IsMultipleOf(60))
            {
                CheckMonsterKillsAndUnlockRecipes();
            }
        }
    }
}