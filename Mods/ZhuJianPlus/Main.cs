using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony12;
using UnityEngine;
using UnityModManagerNet;

namespace ZhuJianPlus
{
    // Token: 0x02000002 RID: 2
    public static class Main
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            HarmonyInstance.Create(modEntry.Info.Id).PatchAll(Assembly.GetExecutingAssembly());
            Main.Logger = modEntry.Logger;
            Main.settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            modEntry.OnGUI = new Action<UnityModManager.ModEntry>(Main.OnGUI);
            modEntry.OnToggle = new Func<UnityModManager.ModEntry, bool, bool>(Main.OnToggle);
            modEntry.OnSaveGUI = new Action<UnityModManager.ModEntry>(Main.OnSaveGUI);
            return true;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000020CC File Offset: 0x000002CC
        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginVertical("Box", new GUILayoutOption[0]);
            GUILayout.BeginHorizontal("Box", new GUILayoutOption[0]);
            GUILayout.Label("使用哪种强效精制", new GUILayoutOption[0]);
            Main.settings.notEnchant = GUILayout.Toggle(Main.settings.notEnchant, "不使用额外强效精制", new GUILayoutOption[0]);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box", new GUILayoutOption[0]);
            Main.settings.extraEnchant = GUILayout.SelectionGrid(Main.settings.extraEnchant, new string[]
            {
                "十二路鱼肠刺剑",
                "工布独一剑",
                "胜邪残剑",
                "巨阙千钧剑",
                "龙源七星剑法",
                "太阿无量剑",
                "纯钧剑气",
                "湛卢剑法"
            }, 8, new GUILayoutOption[0]);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal("Box", new GUILayoutOption[0]);
            GUILayout.Label("最大强效精制次数：", new GUILayoutOption[]
            {
                GUILayout.Width(120f)
            });
            int.TryParse(GUILayout.TextField(Main.settings.maxEnchantTimes.ToString(), 3, new GUILayoutOption[]
            {
                GUILayout.Width(30f)
            }), out Main.settings.maxEnchantTimes);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        // Token: 0x06000003 RID: 3 RVA: 0x0000223C File Offset: 0x0000043C
        public static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            bool flag = !value;
            bool flag2 = flag;
            bool result;
            if (flag2)
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

        // Token: 0x06000004 RID: 4 RVA: 0x00002269 File Offset: 0x00000469
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Main.settings.Save(modEntry);
        }

        // Token: 0x04000001 RID: 1
        public static bool enabled;

        // Token: 0x04000002 RID: 2
        public static Settings settings;

        // Token: 0x04000003 RID: 3
        public static UnityModManager.ModEntry.ModLogger Logger;

        // Token: 0x04000004 RID: 4
        public static UnityEngine.Random rd = new UnityEngine.Random();

        // Token: 0x02000005 RID: 5
        [HarmonyPatch(typeof(BattleEndWindow), "BattleEnd")]
        public static class BattleEndWindow_BattleEnd_Patch
        {
            // Token: 0x06000009 RID: 9 RVA: 0x000022B8 File Offset: 0x000004B8
            public static void Prefix()
            {
                bool flag = !Main.enabled;
                bool flag2 = !flag;
                if (flag2)
                {
                    bool notEnchant = Main.settings.notEnchant;
                    bool flag3 = !notEnchant;
                    if (flag3)
                    {
                        Dictionary<int, int> itemExtraChangeTimesData = DateFile.instance.itemExtraChangeTimesData;
                        int usingWeaponId = BattleSystem.GetUsingWeaponId(true);
                        bool flag4 = int.Parse(DateFile.instance.GetItemDate(usingWeaponId, 4, true, -1)) == 4 && (!itemExtraChangeTimesData.ContainsKey(usingWeaponId) || itemExtraChangeTimesData[usingWeaponId] < Main.settings.maxEnchantTimes);
                        bool flag5 = flag4;
                        if (flag5)
                        {
                            foreach (int key in new Dictionary<int, int[]>
                            {
                                {
                                    1,
                                    new int[]
                                    {
                                        108
                                    }
                                },
                                {
                                    2,
                                    new int[]
                                    {
                                        109
                                    }
                                },
                                {
                                    3,
                                    new int[]
                                    {
                                        110
                                    }
                                },
                                {
                                    4,
                                    new int[]
                                    {
                                        103
                                    }
                                },
                                {
                                    5,
                                    new int[]
                                    {
                                        111
                                    }
                                },
                                {
                                    6,
                                    new int[]
                                    {
                                        112,
                                        113
                                    }
                                },
                                {
                                    7,
                                    new int[]
                                    {
                                        105,
                                        106
                                    }
                                },
                                {
                                    8,
                                    new int[]
                                    {
                                        101,
                                        102,
                                        107
                                    }
                                }
                            }[Main.settings.extraEnchant + 1])
                            {
                                Dictionary<int, string> dictionary = DateFile.instance.changeEquipDate[key];
                                DateFile.instance.ChangItemDate(usingWeaponId, int.Parse(dictionary[2]), int.Parse(dictionary[3]) * 10, false);
                            }
                            bool flag6 = itemExtraChangeTimesData.ContainsKey(usingWeaponId);
                            bool flag7 = flag6;
                            if (flag7)
                            {
                                Dictionary<int, int> dictionary2 = itemExtraChangeTimesData;
                                int key2 = usingWeaponId;
                                int num = dictionary2[key2];
                                dictionary2[key2] = num + 1;
                            }
                            else
                            {
                                itemExtraChangeTimesData.Add(usingWeaponId, 1);
                            }
                        }
                    }
                }
            }
        }
    }
}
