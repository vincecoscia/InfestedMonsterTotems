using Microsoft.Xna.Framework;
using StardewValley;

namespace InfestedMonsterTotems.Utils
{
    public static class SpawnUtils
    {
        public static bool IsValidSpawnPosition(GameLocation location, Vector2 position)
        {
            int tileX = (int)(position.X / 64);
            int tileY = (int)(position.Y / 64);

            Vector2 tileLocation = new Vector2(tileX, tileY);

            return location.isTileOnMap(tileX, tileY) &&
                   !location.isWaterTile(tileX, tileY) &&
                   location.isTilePassable(new xTile.Dimensions.Location(tileX, tileY), Game1.viewport);
        }

        public static Vector2 GetRandomSpawnPosition(GameLocation location)
        {
            int padding = 1;
            int tileX = Game1.random.Next(padding, location.map.GetLayer("Back").LayerWidth - padding);
            int tileY = Game1.random.Next(padding, location.map.GetLayer("Back").LayerHeight - padding);

            float offsetX = Game1.random.Next(-16, 16);
            float offsetY = Game1.random.Next(-16, 16);

            return new Vector2(tileX * 64 + offsetX, tileY * 64 + offsetY);
        }
    }
} 