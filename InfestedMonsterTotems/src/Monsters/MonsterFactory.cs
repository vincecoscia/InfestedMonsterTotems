using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Monsters;

namespace InfestedMonsterTotems.Monsters
{
    public class MonsterFactory
    {
        private readonly Random random = new();

        public NPC? CreateMonster(string monsterType, Vector2 position, int mineLevel)
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
    }
} 