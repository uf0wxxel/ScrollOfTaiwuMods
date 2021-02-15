using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using Harmony12;
using UnityEngine;
using UnityModManagerNet;

namespace BossGongfaFixEnhance
{
    public class Settings : UnityModManager.ModSettings
    {
    }
    
    public static class Main
    {
        public static UnityModManager.ModEntry Mod { get; private set; }
        public static Settings settings;
        public static UnityModManager.ModEntry.ModLogger Logger => Mod?.Logger;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Mod = modEntry;
            HarmonyInstance.Create(Mod.Info.Id).PatchAll(Assembly.GetExecutingAssembly());

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
            GUILayout.EndVertical();
        }

        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            return true;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
        }
    }

    [HarmonyPatch(typeof(BattleSystem), "SetDamage")]
    public static class BattleSystem_SetDamage_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Main.Logger.Log("BattleSystem_SetDamage_Patch start");
            var codes = new List<CodeInstruction>(instructions);
            var i = 0;
            var startIndex = -1;
            var endIndex = -1;
            Main.Logger.Log(codes.Count.ToString());
            for (; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldc_I4) {
                    int val = (int)codes[i].operand;
                    if (val == 0x7534) {
                        startIndex = i - 1;
                        break;
                    }
                }
            }
            for (; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldc_I4) {
                    int val = (int)codes[i].operand;
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
            object inst1 = null;
            object inst2 = null;
            for (i = 0; i < modified.Count; i++) {
                if (modified[i].opcode == OpCodes.Ldfld) {
                    inst1 = modified[i].operand;
                    i++;
                    break;
                }
            }
            for (; i < modified.Count; i++) {
                if (modified[i].opcode == OpCodes.Ldfld) {
                    inst2 = modified[i].operand;
                    break;
                }
            }
            if (inst1 == null || inst2 == null) return instructions;
            var j = 0;
            for (i = 0; i < modified.Count && j < 8; i++) {
                if (modified[i].opcode == OpCodes.Ldfld) {
                    if (j % 2 == 0) {
                        modified[i].operand = inst2;
                    } else {
                        modified[i].operand = inst1;
                    }
                    j++;
                }
            }
            for (i = 0; i < modified.Count; i++) {
                if (modified[i].opcode == OpCodes.Ldc_I4) {
                    int val = (int)modified[i].operand;
                    if (val == 0x7534) {
                        modified[i].operand = 0x9c44;
                    }
                }
            }
            codes.InsertRange(endIndex, modified);
            var newEnd = endIndex + modified.Count;
            codes[endIndex].labels = new List<Label>(codes[newEnd].labels);
            codes[newEnd].labels.Clear();
            Label newLabel = generator.DefineLabel();
            codes[newEnd].labels.Add(newLabel);
            for (i = endIndex; i < newEnd; i++) {
                if (codes[i].opcode == OpCodes.Brfalse) {
                    codes[i].operand = newLabel;
                    break;
                }
            }
            Main.Logger.Log("BattleSystem_SetDamage_Patch success");
            return codes.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(BattleSystem), "AutoSetDefGongFa")]
    class BattleSystem_AutoSetDefGongFa_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Main.Logger.Log("BattleSystem_AutoSetDefGongFa_Patch start");
            var codes = new List<CodeInstruction>(instructions);
            var i = 0;
            var index = -1;
            Main.Logger.Log(codes.Count.ToString());
            for (; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldc_I4) {
                    int val = (int)codes[i].operand;
                    if (val == 0x9c44) {
                        index = i;
                        break;
                    }
                }
            }
            if (index < 0) return instructions;
            for (index = -1; i < codes.Count; i++) {
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
            Main.Logger.Log("BattleSystem_AutoSetDefGongFa_Patch success");
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
