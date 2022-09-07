using System.Collections.Generic;

namespace Hi3Helper.Preset
{
    public static class GameConfigurationTemplate
    {
        public static List<PresetConfigClasses> GameConfigTemplate = new List<PresetConfigClasses>
        {
            
            new PresetConfigClasses
            {
                ProfileName = "BPSEA",
                ZoneName = "Blue Protocol",
                InstallRegistryLocation = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Blue Protocol",
                ConfigRegistryLocation = "Software\\miHoYo\\Blue Protocol",
                DefaultGameLocation = "C:\\Program Files\\Blue Protocol",
                LanguageAvailable = new List<string>{ "en" },
                FallbackLanguage = "en",
                UseRightSideProgress = true,
                GameDirectoryName = "Blue Protocol game",
                GameDispatchURL = "https://{0}.yuanshen.com/query_cur_region?version={1}&platform=3&channel_id=1&dispatchSeed={2}",
                GameExecutableName = "BlueProtocol.exe",
                ProtoDispatchKey = "caffbdd6d7460dff",
                IsHideSocMedDesc = false,
                LauncherSpriteURLMultiLang = true,
                LauncherSpriteURL = "https://ardisahputra.me/launcher/content.json",
                LauncherResourceURL = "https://sdk-os-static.mihoyo.com/hk4e_global/mdk/launcher/api/resource?channel_id=1&key=gcStgarh&launcher_id=10&sub_channel_id=0"
            }

           

        };
    }
}
