using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using InfestedMonsterTotems.Monsters;
using InfestedMonsterTotems.Utils;

namespace InfestedMonsterTotems.Totems
{
    public class TotemManager
    {
        private readonly IModHelper _helper;
        private readonly IMonitor _monitor;
        private readonly MonsterSpawner _monsterSpawner;

        public TotemManager(IModHelper helper, IMonitor monitor)
        {
            _helper = helper;
            _monitor = monitor;
            _monsterSpawner = new MonsterSpawner(helper, monitor);
        }

        public void UseTotem(Farmer player, string totemName, string[] monsters)
        {
            // Determine target level based on totem type
            int targetLevel = totemName switch
            {
                "cakeymat.InfestedMonsterTotems_DuggyTotem" => 13,  // Specific level for Duggies
                _ => 1  // Default level for other monsters
            };

            // Play warp effect
            Game1.playSound("wand");
            Game1.displayFarmer = false;

            // Remove totem item
            if (player.CurrentItem.Stack > 1)
            {
                player.CurrentItem.Stack--;
            }
            else
            {
                player.removeItemFromInventory(player.CurrentItem);
            }

            // Get the mine shaft location name for the target level
            string locationName = MineShaft.GetLevelName(targetLevel);
            
            // If we're already in the mines, handle it differently
            if (Game1.currentLocation is MineShaft currentMine && 
                Game1.currentLocation.Name == locationName)
            {
                HandleSameLocationWarp(currentMine, targetLevel, monsters);
                return;
            }

            // Otherwise, handle the warp to a new mine level
            HandleNewLocationWarp(locationName, targetLevel, monsters);
        }

        private void HandleSameLocationWarp(MineShaft currentMine, int targetLevel, string[] monsters)
        {
            currentMine.loadLevel(targetLevel);
            currentMine.mustKillAllMonstersToAdvance();
            LocationUtils.ClearRocksFromLevel(currentMine);
            
            // Wait a tick to spawn monsters and restore player visibility
            _helper.Events.GameLoop.UpdateTicked += OnReloadTick;
            
            void OnReloadTick(object? sender, UpdateTickedEventArgs e)
            {
                _helper.Events.GameLoop.UpdateTicked -= OnReloadTick;
                Game1.displayFarmer = true;
                _monsterSpawner.SpawnMonsters(monsters, targetLevel);
            }
        }

        private void HandleNewLocationWarp(string locationName, int targetLevel, string[] monsters)
        {
            _helper.Events.Player.Warped += OnPlayerWarped;
            Game1.warpFarmer(locationName, 6, 6, false);

            void OnPlayerWarped(object? sender, WarpedEventArgs e)
            {
                _helper.Events.Player.Warped -= OnPlayerWarped;

                if (Game1.currentLocation is MineShaft mineShaft)
                {
                    mineShaft.loadLevel(targetLevel);
                    mineShaft.mustKillAllMonstersToAdvance();
                    LocationUtils.ClearRocksFromLevel(mineShaft);
                    
                    int ticksWaited = 0;
                    _helper.Events.GameLoop.UpdateTicked += WaitForLoad;

                    void WaitForLoad(object? sender, UpdateTickedEventArgs e)
                    {
                        ticksWaited++;
                        if (ticksWaited >= 2)
                        {
                            _helper.Events.GameLoop.UpdateTicked -= WaitForLoad;
                            Game1.displayFarmer = true;
                            _monsterSpawner.SpawnMonsters(monsters, targetLevel);
                        }
                    }
                }
            }
        }
    }
} 