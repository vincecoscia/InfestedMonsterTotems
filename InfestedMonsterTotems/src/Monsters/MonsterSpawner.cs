using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Monsters;
using InfestedMonsterTotems.Utils;
using System.Linq;

namespace InfestedMonsterTotems.Monsters
{
    public class MonsterSpawner
    {
        private readonly IModHelper _helper;
        private readonly IMonitor _monitor;
        private readonly MonsterFactory _monsterFactory;
        private readonly MonsterWeightCalculator _weightCalculator;

        public MonsterSpawner(IModHelper helper, IMonitor monitor)
        {
            _helper = helper;
            _monitor = monitor;
            _monsterFactory = new MonsterFactory();
            _weightCalculator = new MonsterWeightCalculator();
        }

        public void SpawnMonsters(string[] monsterTypes, int mineLevel)
        {
            if (Game1.currentLocation is not MineShaft mineShaft)
                return;

            // Clear existing monsters
            mineShaft.characters.Filter(c => !(c is Monster));

            // Clear any staircases
            LocationUtils.ClearStaircases(mineShaft, _monitor);

            // Get weighted monster probabilities
            var monsterWeights = _weightCalculator.GetMonsterWeights(monsterTypes);
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

                Vector2 spawnPosition = SpawnUtils.GetRandomSpawnPosition(mineShaft);

                if (SpawnUtils.IsValidSpawnPosition(mineShaft, spawnPosition, monsterType))
                {
                    var monster = _monsterFactory.CreateMonster(monsterType, spawnPosition, mineLevel);
                    if (monster != null)
                    {
                        mineShaft.characters.Add(monster);
                        spawned++;
                        _monitor.Log($"Spawned {monsterType} at {spawnPosition}", LogLevel.Trace);
                    }
                }
            }
            
            _monitor.Log($"Spawned {spawned} monsters after {attempts} attempts", LogLevel.Debug);
        }
    }
} 