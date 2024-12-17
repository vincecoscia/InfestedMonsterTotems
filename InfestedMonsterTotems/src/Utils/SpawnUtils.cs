using Microsoft.Xna.Framework;
using StardewValley;
using System;

namespace InfestedMonsterTotems.Utils
{
    public static class SpawnUtils
    {
        public static bool IsValidSpawnPosition(GameLocation location, Vector2 position, string? monsterType = null)
        {
            int tileX = (int)(position.X / 64);
            int tileY = (int)(position.Y / 64);
            Vector2 tileLocation = new Vector2(tileX, tileY);

            // Basic checks for all monsters
            if (!location.isTileOnMap(tileX, tileY) ||
                location.isWaterTile(tileX, tileY) ||
                !location.isTilePassable(new xTile.Dimensions.Location(tileX, tileY), Game1.viewport))
            {
                return false;
            }

            // Special check for Duggies - they need tillable ground
            if (monsterType != null &&
                (monsterType == "Duggy" || monsterType == "Magma Duggy"))
            {
                return location.doesTileHaveProperty(tileX, tileY, "Diggable", "Back") != null;

            }

            return true;

        }

        public static Vector2 GetRandomSpawnPosition(GameLocation location)
        {
            // Get map dimensions
            int mapWidth = location.map.GetLayer("Back").LayerWidth;
            int mapHeight = location.map.GetLayer("Back").LayerHeight;

            // Calculate spawn boundaries (20% inset from edges)
            int minX = (int)(mapWidth * 0.2);
            int maxX = (int)(mapWidth * 0.8);
            int minY = (int)(mapHeight * 0.2);
            int maxY = (int)(mapHeight * 0.8);

            // Ensure we have valid spawn range
            minX = Math.Max(1, minX);
            minY = Math.Max(1, minY);
            maxX = Math.Min(mapWidth - 2, maxX);
            maxY = Math.Min(mapHeight - 2, maxY);

            // Generate random position within boundaries
            int tileX = Game1.random.Next(minX, maxX);
            int tileY = Game1.random.Next(minY, maxY);

            // Add small random offset within tile for more natural placement
            float offsetX = Game1.random.Next(-8, 8);
            float offsetY = Game1.random.Next(-8, 8);

            return new Vector2(tileX * 64 + offsetX, tileY * 64 + offsetY);
        }
    }
} 