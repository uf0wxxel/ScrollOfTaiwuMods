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

        public static bool Enabled { get; private set; }

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
            var gameVersionText = MainMenu.instance?.gameVersionText?.text?.Trim();

            GUILayout.BeginVertical("Box", (GUILayoutOption[])(object)new GUILayoutOption[0]);
            GUILayout.Label($"测试MOD, 读取当前游戏版本:{gameVersionText}");
            GUILayout.Label($"当前MOD已激活:{(Enabled ? "是" : "否")}");
            GUILayout.EndVertical();
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.Save(modEntry);
        }
    }
}
