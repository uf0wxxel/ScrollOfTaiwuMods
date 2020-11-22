using System;
using System.IO;
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
            GUILayout.Label("内置文字颜色:");
            GUILayout.BeginHorizontal("Box");
            var colorIndex = 0;
            foreach (var colorText in Enum.GetNames(typeof(TaiwuTextColor)))
            {
                var newLine = (colorIndex++) % 10 == 0;
                if (newLine)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal("Box");
                }

                var colorEnum = (TaiwuTextColor)Enum.Parse(typeof(TaiwuTextColor), colorText);
                GUILayout.Label(colorText.SetTaiwuColor(colorEnum));
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

#if DEBUG
            GUILayout.BeginVertical("Box");
            GUILayout.BeginHorizontal("Box");
            if (GUILayout.Button("Dump DateFile"))
            {
                var msgData = DateFile.instance;
                if (DateFile.instance != null)
                {
                    var publicProperties = typeof(DateFile).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    if (publicProperties?.Length > 0)
                    {
                        foreach (var publicProperty in publicProperties)
                        {
                            try
                            {
                                var value = publicProperty.GetValue(DateFile.instance);
                                Logger.DebugFileWriteJson(publicProperty.Name, value);
                            }
                            catch
                            {
                            }
                        }
                    }

                    var publicFields = typeof(DateFile).GetFields(BindingFlags.Public | BindingFlags.Instance);
                    if (publicFields?.Length > 0)
                    {
                        foreach (var publicField in publicFields)
                        {
                            try
                            {
                                var value = publicField.GetValue(DateFile.instance);
                                Logger.DebugFileWriteJson(publicField.Name, value);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
#endif
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
