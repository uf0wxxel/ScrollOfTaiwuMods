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

    /*[HarmonyPatch(typeof(BattleSystem), "SetDamage")]
    public static class BattleSystem_SetDamage_Patch
    {
        static readonly HashSet<OpCode> branchCodes = new HashSet<OpCode>
        {
            OpCodes.Br_S, OpCodes.Brfalse_S, OpCodes.Brtrue_S, OpCodes.Beq_S, OpCodes.Bge_S, OpCodes.Bgt_S,
            OpCodes.Ble_S, OpCodes.Blt_S, OpCodes.Bne_Un_S, OpCodes.Bge_Un_S, OpCodes.Bgt_Un_S, OpCodes.Ble_Un_S,
            OpCodes.Blt_Un_S, OpCodes.Br, OpCodes.Brfalse, OpCodes.Brtrue, OpCodes.Beq, OpCodes.Bge, OpCodes.Bgt,
            OpCodes.Ble, OpCodes.Blt, OpCodes.Bne_Un, OpCodes.Bge_Un, OpCodes.Bgt_Un, OpCodes.Ble_Un, OpCodes.Blt_Un
        };
        
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Main.Logger.Log("BattleSystem_SetDamage_Patch start");
            var codes = new List<CodeInstruction>(instructions);
            var i = 0;
            var startIndex = -1;
            var endIndex = -1;
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
            if (startIndex < 0 || endIndex <= startIndex) {
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
            modified[0].labels.Clear();
            var rpLabels = new Dictionary<Label, Label>();
            for (i = 0; i < modified.Count; i++) {
                if (modified[i].labels.Any()) {
                    for (j = 0; j < modified[i].labels.Count; j++) {
                        var oldLabel = modified[i].labels[j];
                        modified[i].labels[j] = generator.DefineLabel();
                        rpLabels[oldLabel] = modified[i].labels[j];
                    }
                }
            }
            for (i = 0; i < modified.Count; i++) {
                if (branchCodes.Contains(modified[i].opcode)) {
                    Label tempLabel;
                    if (rpLabels.TryGetValue((Label)modified[i].operand, out tempLabel)) {
                        modified[i].operand = tempLabel;
                        break;
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
    }*/

    [HarmonyPatch(typeof(BattleSystem), "AutoSetDefGongFa")]
    class BattleSystem_AutoSetDefGongFa_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Main.Logger.Log("BattleSystem_AutoSetDefGongFa_Patch start");
            var codes = new List<CodeInstruction>(instructions);
            var i = 0;
            var index = -1;
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
            CodeInstruction copy = null;
            for (index = -1; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldc_I4_1) {
                    index = i;
                    break;
                }
                if (codes[i].opcode == OpCodes.Call) {
                    copy = codes[i].Clone();
                }
            }
            if (index < 0 || copy == null) return instructions;
            var toInsert = new List<CodeInstruction>(2);
            toInsert.Add(new CodeInstruction(OpCodes.Ldc_I4_4));
            toInsert.Add(copy);
            codes.InsertRange(index + 1, toInsert);
            Main.Logger.Log("BattleSystem_AutoSetDefGongFa_Patch success");
            return codes.AsEnumerable();
        }
    }

    [HarmonyPatch(typeof(BattleSystem), "UseGongFa")]
    class BattleSystem_UseGongFa_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            Main.Logger.Log("BattleSystem_UseGongFa_Patch start");
            var codes = new List<CodeInstruction>(instructions);
            var i = 0;
            var index = -1;
            var found = false;
            for (; i + 2 < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldarg_0 && codes[i + 1].opcode == OpCodes.Ldfld && codes[i + 2].opcode == OpCodes.Ldloc_2) {
                    if (found) {
                        index = i + 1;
                        break;
                    } else {
                        found = true;
                    }
                }
            }
            if (index < 3) {
                return instructions;
            }
            CodeInstruction copy1 = codes[index].Clone();
            CodeInstruction copy3 = codes[index - 3].Clone();
            CodeInstruction copy2 = null;
            for (i = index + 2; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldfld) {
                    copy2 = codes[i].Clone();
                }
            }
            if (copy2 == null) {
                return instructions;
            }
            for (index = -1, i = 0; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldc_I4 && (int)codes[i].operand == 0x4e29) {
                    index = i;
                    break;
                }
            }
            if (index < 0) {
                return instructions;
            }
            for (index = -1; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldfld && codes[i].operand == copy2.operand) {
                    index = i;
                    break;
                }
            }
            if (index < 0) {
                return instructions;
            }
            Label l1 = generator.DefineLabel();
            Label l2 = generator.DefineLabel();
            codes[index].labels.Add(l1);
            codes[index + 1].labels.Add(l2);
            var toInsert = new List<CodeInstruction>(5);
            toInsert.Add(new CodeInstruction(OpCodes.Ldloc_0));
            toInsert.Add(copy3);
            toInsert.Add(new CodeInstruction(OpCodes.Brfalse_S, l1));
            toInsert.Add(copy1);
            toInsert.Add(new CodeInstruction(OpCodes.Br_S, l2));
            codes.InsertRange(index, toInsert);
            for (index = -1, i += 7; i < codes.Count; i++) {
                if (codes[i].opcode == OpCodes.Ldfld && codes[i].operand == copy2.operand) {
                    index = i;
                    break;
                }
            }
            if (index < 0) {
                return instructions;
            }
            l1 = generator.DefineLabel();
            l2 = generator.DefineLabel();
            codes[index].labels.Add(l1);
            codes[index + 1].labels.Add(l2);
            toInsert[2] = new CodeInstruction(OpCodes.Brfalse_S, l1);
            toInsert[4] = new CodeInstruction(OpCodes.Br_S, l2);
            codes.InsertRange(index, toInsert);
            Main.Logger.Log("BattleSystem_UseGongFa_Patch success");
            return codes.AsEnumerable();
        }
    }
}
