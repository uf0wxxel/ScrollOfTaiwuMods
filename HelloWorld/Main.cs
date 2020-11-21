using System;
using System.Reflection;
using Harmony12;
using UnityEngine;
using UnityModManagerNet;

namespace HelloWorld
{
    public static class Main
    {
        private static Settings _settings;
        private static UnityModManager.ModEntry.ModLogger _logger;

        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            HarmonyInstance.Create(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());
            _logger = modEntry.Logger;
            _settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            modEntry.OnGUI = OnGUI;
            modEntry.OnToggle = OnToggle;
            modEntry.OnSaveGUI = OnSaveGUI;
            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginVertical("Box", (GUILayoutOption[])(object)new GUILayoutOption[0]);
            GUILayout.Label($"测试MOD, 读取当前游戏版本:{MainMenu.instance.gameVersionText.text}");
            GUILayout.EndVertical();
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (!value)
            {
                return false;
            }
            _settings.Enabled = value;
            return true;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            _settings.Save(modEntry);
        }
    }
}
