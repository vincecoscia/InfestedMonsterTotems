using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Monsters;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using InfestedMonsterTotems.Framework;

namespace InfestedMonsterTotems
{
    public class ModEntry : Mod
    {
        public static Dictionary<string, string[]> MonsterTotems = new Dictionary<string, string[]>
        {
            { "cakeymat.InfestedMonsterTotems_SlimeTotem", new[] { 
                "Green Slime", "Frost Jelly", "Red Sludge", "Purple Sludge", 
                "Yellow Slime", "Gray Sludge", "Tiger Slime" 
            }},

            { "cakeymat.InfestedMonsterTotems_BugTotem", new[] { 
                "Bug", "Cave Fly", "Grub", "Mutant Fly", "Mutant Grub" 
            }},

            { "cakeymat.InfestedMonsterTotems_SkeletonTotem", new[] { 
                "Skeleton", "Skeleton Mage" 
            }},

            { "cakeymat.InfestedMonsterTotems_BatTotem", new[] { 
                "Bat", "Frost Bat", "Lava Bat", "Iridium Bat" 
            }},

            { "cakeymat.InfestedMonsterTotems_SpiritTotem", new[] { 
                "Shadow Brute", "Shadow Shaman", "Shadow Sniper" 
            }},

            { "cakeymat.InfestedMonsterTotems_DuggyTotem", new[] { 
                "Duggy", "Magma Duggy" 
            }},

            { "cakeymat.InfestedMonsterTotems_DustSpriteTotem", new[] { 
                "Dust Spirit", "Carbon Ghost" 
            }},

            { "cakeymat.InfestedMonsterTotems_CrabTotem", new[] { 
                "Rock Crab", "Lava Crab", "Iridium Crab" 
            }},

            { "cakeymat.InfestedMonsterTotems_MummyTotem", new[] { 
                "Mummy" 
            }},

            { "cakeymat.InfestedMonsterTotems_DinosaurTotem", new[] { 
                "Pepper Rex" 
            }},

            { "cakeymat.InfestedMonsterTotems_SerpentTotem", new[] { 
                "Serpent", "Royal Serpent" 
            }},

            { "cakeymat.InfestedMonsterTotems_MagmaSpriteTotem", new[] { 
                "Magma Sprite", "Magma Sparker" 
            }}
        };

        private IModHelper _helper;
        private ModConfig Config;
        private Dictionary<string, HashSet<string>> monsterTypeMapping = new()
        {
            { "cakeymat.InfestedMonsterTotems_SlimeTotem", new HashSet<string> { 
                "Green Slime", "Frost Jelly", "Red Sludge", "Purple Sludge", 
                "Yellow Slime", "Gray Sludge", "Tiger Slime" 
            }},

            { "cakeymat.InfestedMonsterTotems_BugTotem", new HashSet<string> { 
                "Bug", "Cave Fly", "Grub", "Mutant Fly", "Mutant Grub" 
            }},

            { "cakeymat.InfestedMonsterTotems_SkeletonTotem", new HashSet<string> { 
                "Skeleton", "Skeleton Mage" 
            }},

            { "cakeymat.InfestedMonsterTotems_BatTotem", new HashSet<string> { 
                "Bat", "Frost Bat", "Lava Bat", "Iridium Bat" 
            }},

            { "cakeymat.InfestedMonsterTotems_SpiritTotem", new HashSet<string> { 
                "Shadow Brute", "Shadow Shaman", "Shadow Sniper" 
            }},

            { "cakeymat.InfestedMonsterTotems_DuggyTotem", new HashSet<string> { 
                "Duggy", "Magma Duggy" 
            }},

            { "cakeymat.InfestedMonsterTotems_DustSpriteTotem", new HashSet<string> { 
                "Dust Spirit", "Carbon Ghost" 
            }},

            { "cakeymat.InfestedMonsterTotems_CrabTotem", new HashSet<string> { 
                "Rock Crab", "Lava Crab", "Iridium Crab" 
            }},

            { "cakeymat.InfestedMonsterTotems_MummyTotem", new HashSet<string> { 
                "Mummy" 
            }},

            { "cakeymat.InfestedMonsterTotems_DinosaurTotem", new HashSet<string> { 
                "Pepper Rex" 
            }},

            { "cakeymat.InfestedMonsterTotems_SerpentTotem", new HashSet<string> { 
                "Serpent", "Royal Serpent" 
            }},

            { "cakeymat.InfestedMonsterTotems_MagmaSpriteTotem", new HashSet<string> { 
                "Magma Sprite", "Magma Sparker" 
            }}
        };

        private IGameContentHelper? gameContentHelper;

        public override void Entry(IModHelper helper)
        {
            _helper = helper;
            gameContentHelper = helper.GameContent;
            
            // Load config
            Config = helper.ReadConfig<ModConfig>();
            
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
            var configMenu = _helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
            {
                Monitor.Log("Generic Mod Config Menu API not found.", LogLevel.Warn);
                return;
            }

            // Register config menu
            new GenericModConfigMenuIntegration(
                configMenu: configMenu,
                manifest: ModManifest,
                getConfig: () => Config,
                reset: () => 
                {
                    Config = new ModConfig();
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

            foreach (var totem in MonsterTotems)
            {
                if (itemHeld.Name == totem.Key)
                {
                    UseTotem(player, totem.Key, totem.Value);
                    break;
                }
            }
        }

        private void ClearRocksFromLevel(StardewValley.Locations.MineShaft mineShaft)
        {
            // Clear all resource clumps (large rocks/stumps/logs)
            mineShaft.resourceClumps.Clear();
            
            // Clear all debris (small rocks, twigs, weeds)
            for (int x = 0; x < mineShaft.map.Layers[0].LayerWidth; x++)
            {
                for (int y = 0; y < mineShaft.map.Layers[0].LayerHeight; y++)
                {
                    Vector2 tile = new Vector2(x, y);
                    if (mineShaft.Objects.ContainsKey(tile))
                    {
                        var obj = mineShaft.Objects[tile];
                        // Check if it's a rock, twig, or weed (category -9 is resource clumps)
                        if (obj.Category == -9 || obj.Name.Contains("Stone") || obj.Name.Contains("Rock") || 
                            obj.Name.Contains("Twig") || obj.Name.Contains("Weed"))
                        {
                            mineShaft.Objects.Remove(tile);
                        }
                    }
                }
            }
        }

        private void UseTotem(Farmer player, string totemName, string[] monsters)
        {
            // Determine target level based on totem type
            int targetLevel = totemName switch
            {
                "cakeymat.InfestedMonsterTotems_DuggyTotem" => 13,  // Specific level for Duggies
                _ => 1  // Default level for other monsters
            };

            // Monitor.Log($"=== Using Totem ===", LogLevel.Info);
            // Monitor.Log($"Totem Name: {totemName}", LogLevel.Info);
            // Monitor.Log($"Target Level: {targetLevel}", LogLevel.Info);
            // Monitor.Log($"Monster Types: {string.Join(", ", monsters)}", LogLevel.Info);
            // Monitor.Log($"Current Location: {Game1.currentLocation?.Name}", LogLevel.Info);

            // Play warp effect
            Game1.playSound("wand");
            Game1.displayFarmer = false;

            // Remove totem item
            if (player.CurrentItem.Stack > 1)
            {
                // Monitor.Log("Reducing totem stack by 1", LogLevel.Debug);
                player.CurrentItem.Stack--;
            }
            else
            {
                // Monitor.Log("Removing totem from inventory", LogLevel.Debug);
                player.removeItemFromInventory(player.CurrentItem);
            }

            // Get the mine shaft location name for the target level
            string locationName = StardewValley.Locations.MineShaft.GetLevelName(targetLevel);
            
            // If we're already in the mines, handle it differently
            if (Game1.currentLocation is StardewValley.Locations.MineShaft currentMine && 
                Game1.currentLocation.Name == locationName)
            {
                // We're already in the target level, reload it
                currentMine.loadLevel(targetLevel);
                currentMine.mustKillAllMonstersToAdvance();
                ClearRocksFromLevel(currentMine);
                
                // Wait a tick to spawn monsters and restore player visibility
                _helper.Events.GameLoop.UpdateTicked += OnReloadTick;
                
                void OnReloadTick(object? sender, UpdateTickedEventArgs e)
                {
                    _helper.Events.GameLoop.UpdateTicked -= OnReloadTick;
                    Game1.displayFarmer = true;
                    SpawnMonsters(monsters, targetLevel);
                    // Monitor.Log($"Reloaded level and respawned monsters on current mine level {targetLevel}", LogLevel.Debug);
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
                    ClearRocksFromLevel(mineShaft);
                    
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
                            // Monitor.Log($"Warped to mine level {targetLevel} and spawned monsters after {ticksWaited} ticks", LogLevel.Debug);
                        }
                    }
                }
            }
        }

        private Dictionary<string, float> GetMonsterWeights(string[] possibleMonsters)
        {
            var weights = new Dictionary<string, float>();
            var player = Game1.player;
            
            // Check various progression flags
            bool hasSkullKey = player.hasSkullKey;
            bool hasGingerIsland = Game1.MasterPlayer.mailReceived.Contains("Island_Unlock");
            bool hasQuarryUnlocked = Game1.MasterPlayer.mailReceived.Contains("ccCraftsRoom");
            int mineLevel = player.deepestMineLevel;
            
            foreach (string monster in possibleMonsters)
            {
                // Default weight
                float weight = 1f;
                
                // First check if monster should be completely unavailable
                bool isAvailable = monster switch
                {
                    // Skull Cavern exclusive monsters
                    "Royal Serpent" or "Pepper Rex" or "Mummy" => hasSkullKey,
                    
                    // Ginger Island exclusive monsters
                    "Tiger Slime" => hasGingerIsland,
                    
                    // Quarry exclusive monsters (if any)
                    "Carbon Ghost" => hasQuarryUnlocked,
                    
                    // All other monsters are always available
                    _ => true
                };
                
                if (!isAvailable)
                {
                    weights[monster] = 0f;
                    continue;
                }
                
                // Adjust weights based on progression
                weight = monster switch
                {
                    // Stronger variants have lower weights early game
                    "Purple Sludge" or "Red Sludge" or "Black Slime" => mineLevel >= 80 ? 1.2f : 0.3f,
                    "Frost Jelly" => mineLevel >= 40 ? 1.0f : 0.4f,
                    "Skeleton Mage" or "Iridium Bat" or "Iridium Crab" => hasSkullKey ? 1.5f : 0.3f,
                    "Magma Duggy" or "Lava Bat" or "Lava Crab" => mineLevel >= 80 ? 1.2f : 0.4f,
                    "Armored Bug" or "Mutant Fly" or "Mutant Grub" => mineLevel >= 70 ? 1.0f : 0.3f,
                    
                    // Basic variants have higher weights early game
                    "Green Slime" or "Bat" or "Bug" or "Rock Crab" or "Duggy" => 
                        hasSkullKey ? 0.7f : mineLevel >= 40 ? 1.0f : 1.5f,
                    
                    // Default weight for everything else
                    _ => 1.0f
                };
                
                weights[monster] = weight;
            }
            
            return weights;
        }

        private void SpawnMonsters(string[] monsterTypes, int mineLevel)
        {
            if (Game1.currentLocation is not StardewValley.Locations.MineShaft mineShaft)
                return;

            // Clear existing monsters
            mineShaft.characters.Filter(c => !(c is Monster));

            // Get weighted monster probabilities
            var monsterWeights = GetMonsterWeights(monsterTypes);
            float totalWeight = monsterWeights.Values.Sum();

            // Spawn 5-8 monsters
            int monstersToSpawn = Game1.random.Next(5, 9);
            int attempts = 0;
            int spawned = 0;

            while (spawned < monstersToSpawn && attempts < 100)
            {
                attempts++;
                
                // Select monster type based on weights
                float roll = (float)(Game1.random.NextDouble() * totalWeight);
                string monsterType = monsterTypes[0]; // Default
                float currentWeight = 0;
                
                foreach (var weight in monsterWeights)
                {
                    currentWeight += weight.Value;
                    if (roll <= currentWeight)
                    {
                        monsterType = weight.Key;
                        break;
                    }
                }

                Vector2 spawnPosition = GetRandomSpawnPosition(mineShaft);

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

        private readonly Random random = new Random();

        private NPC? CreateMonster(string monsterType, Vector2 position, int mineLevel)
        {
            return monsterType switch
            {
                // Slimes
                "Green Slime" => new GreenSlime(position) { Name = "Green Slime" },
                "Frost Jelly" => new GreenSlime(position, 40) { Name = "Frost Jelly" },
                "Red Sludge" => new GreenSlime(position, 80) { Name = "Red Sludge" },
                "Purple Sludge" => new GreenSlime(position, 121) { Name = "Purple Sludge" },
                "Yellow Slime" => new GreenSlime(position, new Color(255, 255, 50)) { Name = "Yellow Slime" },
                // "Gray Sludge" => new GreenSlime(position, 77377) { Name = "Gray Sludge"  },
                "Black Slime" => new GreenSlime(position, new Color(40 + random.Next(10), 40 + random.Next(10), 40 + random.Next(10))) { Name = "Black Slime" },
                "Tiger Slime" => new GreenSlime(position) { Name = "Tiger Slime" },
                // "Prismatic Slime" => new GreenSlime(position) { Name = "Prismatic Slime" },
                // "Big Slime" => new BigSlime(position, 0) { Name = "Big Slime" },

                // Bats
                "Bat" => new Bat(position),
                "Frost Bat" => new Bat(position, 40) { Name = "Frost Bat" },
                "Lava Bat" => new Bat(position, 80) { Name = "Lava Bat" },
                "Iridium Bat" => new Bat(position, 171) { Name = "Iridium Bat" },
                "Haunted Skull" => new Bat(position, 77377) { Name = "Haunted Skull" },
                "Cursed Doll" => new Bat(position, -666) { Name = "Cursed Doll" },
                "Magma Sprite" => new Bat(position, -555) { Name = "Magma Sprite" },
                "Magma Sparker" => new Bat(position, -556) { Name = "Magma Sparker" },

                // Bugs
                "Bug" => new Bug(position, mineLevel),
                "Armored Bug" => new Bug(position, 121) { Name = "Armored Bug" },
                "Cave Fly" => new Fly(position),
                "Mutant Fly" => new Fly(position, true) { Name = "Mutant Fly" },
                "Grub" => new Grub(position),
                "Mutant Grub" => new Grub(position, true) { Name = "Mutant Grub" },

                // Ghosts and Spirits
                "Ghost" => new Ghost(position),
                "Carbon Ghost" => new Ghost(position) { Name = "Carbon Ghost" },
                "Putrid Ghost" => new Ghost(position) { Name = "Putrid Ghost" },
                "Dust Spirit" => new DustSpirit(position),

                // Crabs and Rock Creatures
                "Rock Crab" => new RockCrab(position),
                "Lava Crab" => new RockCrab(position) { Name = "Lava Crab" },
                "Iridium Crab" => new RockCrab(position) { Name = "Iridium Crab" },
                "False Magma Cap" => new RockCrab(position) { Name = "False Magma Cap" },
                "Stick Bug" => new RockCrab(position) { Name = "Stick Bug" },
                "Stone Golem" => new RockGolem(position),
                "Wilderness Golem" => new RockGolem(position, 5) { Name = "Wilderness Golem" },

                // Shadows and Skeletons
                "Shadow Brute" => new ShadowBrute(position),
                "Shadow Shaman" => new ShadowShaman(position),
                "Shadow Sniper" => new Shooter(position),
                "Skeleton" => new Skeleton(position, false),
                "Skeleton Mage" => new Skeleton(position, true),

                // Duggies
                "Duggy" => new Duggy(position),
                "Magma Duggy" => new Duggy(position) { Name = "Magma Duggy" },

                // Other Unique Monsters
                "Metal Head" => new MetalHead(position, 80),
                "Mummy" => new Mummy(position),
                "Pepper Rex" => new DinoMonster(position),
                "Serpent" => new Serpent(position),
                "Royal Serpent" => new Serpent(position) { Name = "Royal Serpent" },
                "Squid Kid" => new SquidKid(position),
                "Blue Squid" => new BlueSquid(position),
                "Dwarvish Sentry" => new DwarvishSentry(position),
                "Hot Head" => new HotHead(position),
                "Lava Lurk" => new LavaLurk(position),
                "Spiker" => new Spiker(position, 0),
                "Spider" => new Leaper(position) { Name = "Spider" },

                _ => null
            };
        }

        private void CheckMonsterKillsAndUnlockRecipes()
        {
            if (!Context.IsWorldReady)
                return;

            var player = Game1.player;
            Monitor.Log("Checking monster kills for recipe unlocks...", LogLevel.Debug);

            foreach (var entry in monsterTypeMapping)
            {
                string totemName = entry.Key;
                var monsterTypes = entry.Value;
                string mailId = $"{totemName}Recipe";

                // Skip if recipe is already known
                if (player.craftingRecipes.ContainsKey(totemName) || 
                    player.mailReceived.Contains(mailId) ||
                    player.mailForTomorrow.Contains(mailId) ||
                    player.mailbox.Contains(mailId))
                {
                    continue;
                }

                // If requirements are disabled, automatically unlock the recipe
                if (!Config.EnableTotemUnlockRequirements)
                {
                    Monitor.Log($"Requirements disabled - automatically unlocking recipe for {totemName}", LogLevel.Info);
                    if (!player.mailbox.Contains(mailId))
                    {
                        Game1.addMailForTomorrow(mailId);
                        Monitor.Log($"Added mail '{mailId}' to be delivered tomorrow", LogLevel.Info);
                    }
                    continue;
                }

                // Get total kills for all related monster types
                int totalKills = 0;
                foreach (var monsterType in monsterTypes)
                {
                    int kills = Game1.stats.getMonstersKilled(monsterType);
                    totalKills += kills;
                    Monitor.Log($"{monsterType}: {kills} kills", LogLevel.Trace);
                }

                Monitor.Log($"Total kills for {totemName}: {totalKills}", LogLevel.Debug);

                // Check if kills meet requirement
                if (totalKills >= Config.TotemUnlockRequirements[totemName])
                {
                    Monitor.Log($"Unlocking recipe for {totemName} ({totalKills} kills)", LogLevel.Info);
                    // Add mail for tomorrow if not already in mailbox
                    if (!player.mailbox.Contains(mailId))
                    {
                        Game1.addMailForTomorrow(mailId);
                        Monitor.Log($"Added mail '{mailId}' to be delivered tomorrow", LogLevel.Info);
                    }
                }
            }
        }

        private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            // Check for recipes when save is loaded
            CheckMonsterKillsAndUnlockRecipes();
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            // Check for any recipes that should be unlocked based on kills
            CheckMonsterKillsAndUnlockRecipes();
        }
    }
}