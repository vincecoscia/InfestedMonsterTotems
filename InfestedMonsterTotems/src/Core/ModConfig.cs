using System.Collections.Generic;

namespace InfestedMonsterTotems.Core
{
    public class ModConfig
    {
        /*********
        ** Accessors
        *********/
        /****
        ** Totem Requirements
        ****/
        /// <summary>The number of monsters that need to be killed to unlock each totem type.</summary>
        public Dictionary<string, int> TotemUnlockRequirements { get; set; } = new()
        {
            { "cakeymat.InfestedMonsterTotems_SlimeTotem", 10 },
            { "cakeymat.InfestedMonsterTotems_BugTotem", 10 },
            { "cakeymat.InfestedMonsterTotems_SkeletonTotem", 10 },
            { "cakeymat.InfestedMonsterTotems_BatTotem", 10 },
            { "cakeymat.InfestedMonsterTotems_SpiritTotem", 10 },
            { "cakeymat.InfestedMonsterTotems_DuggyTotem", 10 },
            { "cakeymat.InfestedMonsterTotems_DustSpriteTotem", 10 },
            { "cakeymat.InfestedMonsterTotems_CrabTotem", 10 },
            { "cakeymat.InfestedMonsterTotems_MummyTotem", 10 },
            { "cakeymat.InfestedMonsterTotems_DinosaurTotem", 10 },
            { "cakeymat.InfestedMonsterTotems_SerpentTotem", 10 },
            { "cakeymat.InfestedMonsterTotems_MagmaSpriteTotem", 10 }
        };

        /****
        ** Shop Configuration
        ****/
        /// <summary>The purchase price for each totem type in the shop.</summary>
        public Dictionary<string, int> TotemShopPrices { get; set; } = new()
        {
            { "cakeymat.InfestedMonsterTotems_SlimeTotem", 1000 },
            { "cakeymat.InfestedMonsterTotems_BugTotem", 1000 },
            { "cakeymat.InfestedMonsterTotems_SkeletonTotem", 1500 },
            { "cakeymat.InfestedMonsterTotems_BatTotem", 1000 },
            { "cakeymat.InfestedMonsterTotems_SpiritTotem", 2000 },
            { "cakeymat.InfestedMonsterTotems_DuggyTotem", 1000 },
            { "cakeymat.InfestedMonsterTotems_DustSpriteTotem", 1000 },
            { "cakeymat.InfestedMonsterTotems_CrabTotem", 1000 },
            { "cakeymat.InfestedMonsterTotems_MummyTotem", 2500 },
            { "cakeymat.InfestedMonsterTotems_DinosaurTotem", 2500 },
            { "cakeymat.InfestedMonsterTotems_SerpentTotem", 2000 },
            { "cakeymat.InfestedMonsterTotems_MagmaSpriteTotem", 1500 }
        };

        /****
        ** General Settings
        ****/
        /// <summary>Whether totems need to be unlocked by killing monsters before they can be purchased.</summary>
        public bool EnableTotemUnlockRequirements { get; set; } = true;
    }
} 