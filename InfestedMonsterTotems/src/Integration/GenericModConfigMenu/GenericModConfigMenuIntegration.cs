using StardewModdingAPI;
using InfestedMonsterTotems.Core;
using InfestedMonsterTotems.Totems;

namespace InfestedMonsterTotems.Integration.GenericModConfigMenu
{
    internal class GenericModConfigMenuIntegration
    {
        private readonly IGenericModConfigMenuApi ConfigMenu;
        private readonly IManifest ModManifest;
        private readonly Core.ModConfig DefaultConfig;
        private readonly Func<Core.ModConfig> GetConfig;
        private readonly Action Reset;
        private readonly Action SaveAndApply;

        public GenericModConfigMenuIntegration(
            IGenericModConfigMenuApi configMenu,
            IManifest manifest,
            Func<Core.ModConfig> getConfig,
            Action reset,
            Action saveAndApply)
        {
            ConfigMenu = configMenu;
            ModManifest = manifest;
            DefaultConfig = new Core.ModConfig();
            GetConfig = getConfig;
            Reset = reset;
            SaveAndApply = saveAndApply;
        }

        public void Register()
        {
            ConfigMenu.Register(
                mod: ModManifest,
                reset: Reset,
                save: SaveAndApply
            );

            // General Settings
            ConfigMenu.AddSectionTitle(ModManifest, () => "General Settings");

            ConfigMenu.AddBoolOption(
                mod: ModManifest,
                name: () => "Enable Totem Requirements",
                tooltip: () => "Whether totems need to be unlocked by killing monsters before they can be purchased.",
                getValue: () => GetConfig().EnableTotemUnlockRequirements,
                setValue: value => GetConfig().EnableTotemUnlockRequirements = value
            );

            // Monster Kill Requirements
            ConfigMenu.AddSectionTitle(ModManifest, () => "Monster Kill Requirements");

            foreach (var totem in TotemRegistry.MonsterTotems)
            {
                ConfigMenu.AddNumberOption(
                    mod: ModManifest,
                    name: () => GetTotemDisplayName(totem.Key),
                    tooltip: () => $"Number of {string.Join(", ", totem.Value)} kills needed to unlock this totem",
                    getValue: () => GetConfig().TotemUnlockRequirements[totem.Key],
                    setValue: value => GetConfig().TotemUnlockRequirements[totem.Key] = value,
                    min: 0,
                    max: 100
                );
            }

            // Shop Prices
            ConfigMenu.AddSectionTitle(ModManifest, () => "Shop Prices");

            foreach (var totem in TotemRegistry.MonsterTotems)
            {
                ConfigMenu.AddNumberOption(
                    mod: ModManifest,
                    name: () => GetTotemDisplayName(totem.Key),
                    tooltip: () => $"Purchase price for the {GetTotemDisplayName(totem.Key)}",
                    getValue: () => GetConfig().TotemShopPrices[totem.Key],
                    setValue: value => {
                        GetConfig().TotemShopPrices[totem.Key] = value;
                        SaveAndApply();
                    },
                    min: 0,
                    max: 10000,
                    interval: 100
                );
            }
        }

        private string GetTotemDisplayName(string totemId)
        {
            string name = totemId.Replace("cakeymat.InfestedMonsterTotems_", "").Replace("Totem", "");
            return string.Join(" ", System.Text.RegularExpressions.Regex.Split(name, @"(?<!^)(?=[A-Z])"));
        }
    }
} 