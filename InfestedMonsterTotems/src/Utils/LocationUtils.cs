using StardewValley;
using StardewValley.Locations;
using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace InfestedMonsterTotems.Utils
{
    public static class LocationUtils
    {
        public static void ClearRocksFromLevel(MineShaft mineShaft)
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

        public static void ClearStaircases(MineShaft mineShaft, IMonitor monitor)
        {
            for (int x = 0; x < mineShaft.map.Layers[0].LayerWidth; x++)
            {
                for (int y = 0; y < mineShaft.map.Layers[0].LayerHeight; y++)
                {
                    var buildingTile = mineShaft.map.GetLayer("Buildings").Tiles[x, y];
                    if (buildingTile?.TileIndex == 173)
                    {
                        mineShaft.map.GetLayer("Buildings").Tiles[x, y] = null;
                        monitor.Log($"Cleared staircase from Buildings layer at {x},{y}", LogLevel.Debug);
                    }
                }
            }
        }
    }
} 