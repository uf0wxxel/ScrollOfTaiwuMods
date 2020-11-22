using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony12;
using UnityEngine;
using UnityModManagerNet;

namespace ShowMeMore
{
    // Token: 0x02000002 RID: 2
    public class Main
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Main.Logger = modEntry.Logger;
            Main.settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            HarmonyInstance.Create(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());
            modEntry.OnToggle = new Func<UnityModManager.ModEntry, bool, bool>(Main.OnToggle);
            modEntry.OnGUI = new Action<UnityModManager.ModEntry>(Main.OnGUI);
            modEntry.OnSaveGUI = new Action<UnityModManager.ModEntry>(Main.OnSaveGUI);
            return true;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000020CC File Offset: 0x000002CC
        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayout.Label("<color=#8E8E8EFF>作者jmswzyk</color>，<color=#E3C66DFF>有什么别的隐藏信息想要显示的，欢迎来NGA提议</color>", new GUILayoutOption[0]);
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical("Box", new GUILayoutOption[0]);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayoutOption[] options = new GUILayoutOption[]
            {
                GUILayout.Width(150f)
            };
            Main.settings.ShowGongFaMassage = GUILayout.Toggle(Main.settings.ShowGongFaMassage, "显示功法隐藏属性", options);
            GUILayoutOption[] options2 = new GUILayoutOption[]
            {
                GUILayout.Width(150f)
            };
            Main.settings.ShowHitRatio = (GUILayout.Toggle(Main.settings.ShowHitRatio, "显示功法穿透命中", options2) && Main.settings.ShowGongFaMassage);
            GUILayoutOption[] options3 = new GUILayoutOption[]
            {
                GUILayout.Width(180f)
            };
            Main.settings.ShowRealTime = (GUILayout.Toggle(Main.settings.ShowRealTime, "<color=#8E8E8EFF>战斗中显示真实释放时间*</color>", options3) && Main.settings.ShowGongFaMassage);
            GUILayout.EndHorizontal();
            GUILayout.Label("<color=#8E8E8EFF>*不选真实释放时间则显示的释放时间是原始数据/100。真实释放时间基础值是40（伤害功法）或20（其他） + 原始数据 * 25 / 100</color>", new GUILayoutOption[0]);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayoutOption[] options4 = new GUILayoutOption[]
            {
                GUILayout.Width(150f)
            };
            Main.settings.ShowQiRate = GUILayout.Toggle(Main.settings.ShowQiRate, "显示内力修习进度", options4);
            GUILayoutOption[] options5 = new GUILayoutOption[]
            {
                GUILayout.Width(150f)
            };
            Main.settings.AddHunYuan = (GUILayout.Toggle(Main.settings.AddHunYuan, "混元内力加在各项*", options5) && Main.settings.ShowQiRate);
            GUILayout.EndHorizontal();
            GUILayout.Label("<color=#8E8E8EFF>在内力属性悬浮窗增加显示</color><color=#FBFBFBFF>各属性真实内力/学到的内功修满后的内力</color><color=#8E8E8EFF>，方便练混元。*默认各属性显示数值不加混元内力。</color>", new GUILayoutOption[0]);
            Main.settings.ShowQuquMassage = GUILayout.Toggle(Main.settings.ShowQuquMassage, "显示蛐蛐隐藏属性", new GUILayoutOption[0]);
            GUILayout.Label("<color=#8E8E8EFF>*显示的</color><color=#AE5AC8FF>“提供威望”</color><color=#8E8E8EFF>是指放在</color><color=#E4504DFF>神一品蛐蛐罐</color><color=#8E8E8EFF>能提供的威望（低品蛐蛐罐会造成提供的威望打折）</color>", new GUILayoutOption[0]);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayoutOption[] options6 = new GUILayoutOption[]
            {
                GUILayout.Width(150f)
            };
            Main.settings.ShowItemMassage = GUILayout.Toggle(Main.settings.ShowItemMassage, "显示物品隐藏信息", options6);
            GUILayoutOption[] options7 = new GUILayoutOption[]
            {
                GUILayout.Width(150f)
            };
            Main.settings.ShowWeaponMassage = (GUILayout.Toggle(Main.settings.ShowWeaponMassage, "显示武器隐藏信息", options7) && Main.settings.ShowItemMassage);
            GUILayoutOption[] options8 = new GUILayoutOption[]
            {
                GUILayout.Width(180f)
            };
            Main.settings.ShowMoreAttackTimeAtEffect = (GUILayout.Toggle(Main.settings.ShowMoreAttackTimeAtEffect, "<color=#8E8E8EFF>连击率按千年醉状态计算</color>", options8) && Main.settings.ShowWeaponMassage);
            GUILayoutOption[] options9 = new GUILayoutOption[]
            {
                GUILayout.Width(150f)
            };
            Main.settings.ShowItemFavor = (GUILayout.Toggle(Main.settings.ShowItemFavor, "显示物品赠送好感", options9) && Main.settings.ShowItemMassage);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayoutOption[] options10 = new GUILayoutOption[]
            {
                GUILayout.Width(500f)
            };
            Main.settings.ShowHomeMassage = GUILayout.Toggle(Main.settings.ShowHomeMassage, "显示太吾村年均收益<color=#8E8E8EFF>——位于太吾村地块资源面板图标浮动信息</color>", options10);
            GUILayoutOption[] options11 = new GUILayoutOption[]
            {
                GUILayout.Width(150f)
            };
            Main.settings.ShowHomeMassageAtTop = (GUILayout.Toggle(Main.settings.ShowHomeMassageAtTop, "在顶部资源栏也显示", options11) && Main.settings.ShowHomeMassage);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002460 File Offset: 0x00000660
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Main.settings.Save(modEntry);
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002470 File Offset: 0x00000670
        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            bool flag = !value;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                Main.enabled = value;
                result = true;
            }
            return result;
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002498 File Offset: 0x00000698
        private static void SetGUIToToggle(int index, string name, ref List<int> field)
        {
            bool flag = GUILayout.Toggle(field.Contains(index), name, new GUILayoutOption[0]);
            bool changed = GUI.changed;
            if (changed)
            {
                bool flag2 = flag;
                if (flag2)
                {
                    bool flag3 = !field.Contains(index);
                    if (flag3)
                    {
                        field.Add(index);
                    }
                }
                else
                {
                    bool flag4 = field.Contains(index);
                    if (flag4)
                    {
                        field.Remove(index);
                    }
                }
            }
        }

        // Token: 0x04000001 RID: 1
        public static Settings settings;

        // Token: 0x04000002 RID: 2
        public static bool enabled;

        // Token: 0x04000003 RID: 3
        public static UnityModManager.ModEntry.ModLogger Logger;
    }
}
