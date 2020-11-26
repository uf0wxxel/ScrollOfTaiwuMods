using System.Reflection;
using Harmony12;
using UnityEngine;
using UnityModManagerNet;

namespace PrettifyActors
{
    public static class Main
    {
        public static UnityModManager.ModEntry Mod { get; private set; }
        public static UnityModManager.ModEntry.ModLogger Logger => Mod?.Logger;

        public static bool Enabled { get; private set; }

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Mod = modEntry;
            SharedSpritePatchHelper.Logger = modEntry.Logger;
            HarmonyInstance.Create(Mod.Info.Id).PatchAll(Assembly.GetExecutingAssembly());

            Mod.OnGUI = OnGUI;
            Mod.OnToggle = OnToggle;
            Mod.OnSaveGUI = OnSaveGUI;

            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"已加载{SharedSpritePatchHelper.TextureKey2InfoMappings.Count}个贴图文件");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label($"男性角色美化贴图来自：https://bbs.nga.cn/read.php?tid=19552335&_fp=3");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label($"女性角色美化贴图来自：https://bbs.3dmgame.com/thread-5795262-1-1.html");
            GUILayout.EndHorizontal();
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
        }
    }
}
