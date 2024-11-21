using System.Collections.Generic;

namespace InfestedMonsterTotems.Core
{
    public static class TotemRegistry
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

        public static Dictionary<string, HashSet<string>> MonsterTypeMapping { get; } = new()
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
    }
} 