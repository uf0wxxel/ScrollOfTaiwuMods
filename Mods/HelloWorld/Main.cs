using System.Reflection;
using Harmony12;
using UnityEngine;
using UnityModManagerNet;

namespace HelloWorld
{
    public static class Main
    {
        public static UnityModManager.ModEntry Mod { get; private set; }
        public static Settings Settings { get; private set; }
        public static UnityModManager.ModEntry.ModLogger Logger => Mod?.Logger;
        public static string GameVersionText { get; private set; }

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Mod = modEntry;
            HarmonyInstance.Create(Mod.Info.Id).PatchAll(Assembly.GetExecutingAssembly());
            Settings = UnityModManager.ModSettings.Load<Settings>(Mod);

            Mod.OnGUI = OnGUI;
            Mod.OnToggle = OnToggle;
            Mod.OnSaveGUI = OnSaveGUI;

            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            if (GameVersionText.IsNullOrEmpty())
            {
                GameVersionText = MainMenu.instance?.gameVersionText?.text?.Trim();
                Logger.Log($"GameVersion: {GameVersionText}");
            }

            GUILayout.BeginVertical("Box", (GUILayoutOption[])(object)new GUILayoutOption[0]);
            GUILayout.Label($"测试MOD, 读取当前游戏版本:{GameVersionText}");
            GUILayout.EndVertical();
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (!value)
            {
                return false;
            }

            return true;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }
    }
}
