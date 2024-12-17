using StardewModdingAPI;
using StardewValley;
using InfestedMonsterTotems.Core;

namespace InfestedMonsterTotems.Totems
{
    public class TotemUnlockManager
    {
        private readonly IModHelper _helper;
        private readonly IMonitor _monitor;
        private readonly Core.ModConfig _config;

        public TotemUnlockManager(IModHelper helper, IMonitor monitor, Core.ModConfig config)
        {
            _helper = helper;
            _monitor = monitor;
            _config = config;
        }

        public void CheckMonsterKillsAndUnlockRecipes()
        {
            if (!Context.IsWorldReady)
                return;

            var player = Game1.player;
            _monitor.Log("Checking monster kills for recipe unlocks...", LogLevel.Debug);

            foreach (var entry in TotemRegistry.MonsterTypeMapping)
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
                if (!_config.EnableTotemUnlockRequirements)
                {
                    _monitor.Log($"Requirements disabled - automatically unlocking recipe for {totemName}", LogLevel.Info);
                    if (!player.mailbox.Contains(mailId))
                    {
                        player.mailbox.Add(mailId);
                        _monitor.Log($"Added mail '{mailId}' to mailbox for tomorrow morning", LogLevel.Info);
                    }
                    continue;
                }

                // Get total kills for all related monster types
                int totalKills = 0;
                foreach (var monsterType in monsterTypes)
                {
                    int kills = Game1.stats.getMonstersKilled(monsterType);
                    totalKills += kills;
                    _monitor.Log($"{monsterType}: {kills} kills", LogLevel.Trace);
                }

                _monitor.Log($"Total kills for {totemName}: {totalKills}", LogLevel.Debug);

                // Check if kills meet requirement
                if (totalKills >= _config.TotemUnlockRequirements[totemName])
                {
                    _monitor.Log($"Unlocking recipe for {totemName} ({totalKills} kills)", LogLevel.Info);
                    if (!player.mailbox.Contains(mailId))
                    {
                        player.mailbox.Add(mailId);
                        _monitor.Log($"Added mail '{mailId}' to mailbox for tomorrow morning", LogLevel.Info);
                    }
                }
            }
        }
    }
} 