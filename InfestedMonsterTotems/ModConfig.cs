public class ModConfig
{
    public Dictionary<string, int> TotemUnlockRequirements { get; set; } = new()
    {
        { "cakeymat.InfestedMonsterTotems_SlimeTotem", 10 },    // Kill 10 slimes to unlock
        { "cakeymat.InfestedMonsterTotems_BugTotem", 10 },      // Kill 10 bugs to unlock
        { "cakeymat.InfestedMonsterTotems_SkeletonTotem", 10 }, // Kill 10 skeletons to unlock
        { "cakeymat.InfestedMonsterTotems_BatTotem", 10 }       // Kill 10 bats to unlock
    };
} 