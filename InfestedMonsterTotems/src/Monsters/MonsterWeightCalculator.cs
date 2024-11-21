using StardewValley;

namespace InfestedMonsterTotems.Monsters
{
    public class MonsterWeightCalculator
    {
        public Dictionary<string, float> GetMonsterWeights(string[] possibleMonsters)
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
    }
} 