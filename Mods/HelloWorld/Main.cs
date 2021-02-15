﻿using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Harmony12;
using UnityEngine;
using UnityModManagerNet;

namespace BossGongfaFixEnhance
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
            GUILayout.Label($"已测试的游戏版本:0.2.8.4, 当前游戏版本:{gameVersionText}");
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

    [HarmonyPatch(typeof(BattleSystem), "SetDamage")]
    public static class BattleSystem_SetDamage_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Main.Enabled) return instructions;
            var codes = new List<CodeInstruction>(instructions);
            var i = 0;
            var startIndex = -1;
            var endIndex = -1;
            for (; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldc_I4) {
                    var val = codes[i].operand as int;
                    if (val == 0x7534) {
                        startIndex = i - 1;
                        break;
                    }
                }
            }
            for (; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldc_I4) {
                    var val = codes[i].operand as int;
                    if (val == 0x7534) {
                        break;
                    }
                }
            }
            for (; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Starg_S) {
                    break;
                }
            }
            for (; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldarg_1) {
                    endIndex = i;
                    break;
                }
            }
            if (startIndex < 0 || endIndex < startIndex) {
                return instructions;
            }
            var modified = codes.GetRange(startIndex, endIndex - startIndex);
            var inst1, inst2;
            i = 0;
            for (; i < modified.Count; i++) {
                if (modified[i].opcode == OpCodes.Ldfld) {
                    inst1 = modified[i];
                    i++;
                    break;
                }
            }
            for (; i < modified.Count; i++) {
                if (modified[i].opcode == OpCodes.Ldfld) {
                    inst2 = modified[i];
                    break;
                }
            }
            i = 0;
            var j = 0;
            for (; i < modified.Count && j < 8; i++) {
                if (modified[i].opcode == OpCodes.Ldfld) {
                    if (j % 2 == 0) {
                        modified[i] = inst2;
                    } else {
                        modified[i] = inst1;
                    }
                    j++;
                }
            }
            i = 0;
            for (; i < modified.Count; i++) {
                if (modified[i].opcode == OpCodes.Ldc_I4) {
                    var val = modified[i].operand as int;
                    if (val == 0x7534) {
                        modified[i].operand = 0x9c44;
                    }
                }
            }
            codes[i].InsertRange(endIndex, modified);
            return codes.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(BattleSystem), "AutoSetDefGongFa")]
    class BattleSystem_AutoSetDefGongFa_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Main.Enabled) return instructions;
            var codes = new List<CodeInstruction>(instructions);
            var i = 0;
            var index = -1;
            for (; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldc_I4) {
                    var val = codes[i].operand as int;
                    if (val == 0x7534) {
                        index = i;
                        break;
                    }
                }
            }
            if (index < 0) return instructions;
            index = -1;
            for (; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldc_I4_M1) {
                    index = i;
                    break;
                }
            }
            if (index < 0) return instructions;
            for (; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldc_I4_1) {
                    codes[i].opcode = OpCodes.Ldc_I4_2;
                }
            }
            return codes.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(BattleSystem), "UseGongFa")]
    class BattleSystem_UseGongFa_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Todo
            return instructions;
        }
    }
}
