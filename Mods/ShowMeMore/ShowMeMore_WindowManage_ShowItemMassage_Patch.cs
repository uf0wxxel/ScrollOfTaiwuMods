using System.Collections.Generic;
using GameData;
using Harmony12;
using UnityEngine.UI;
using UnityModManagerNet;

namespace ShowMeMore
{
    // Token: 0x02000007 RID: 7
    [HarmonyPatch(typeof(WindowManage), "ShowItemMassage")]
    public class ShowMeMore_WindowManage_ShowItemMassage_Patch
    {
        // Token: 0x0600000C RID: 12 RVA: 0x000033A8 File Offset: 0x000015A8
        public static int getItemMassage(int itemId, string index)
        {
            Dictionary<int, Dictionary<string, int>> dictionary = new Dictionary<int, Dictionary<string, int>>();
            UnityModManager.Logger.Log("物品ID是" + itemId.ToString());
            bool flag = dictionary.ContainsKey(itemId);
            if (flag)
            {
                bool flag2 = dictionary[itemId].ContainsKey(index);
                if (flag2)
                {
                    UnityModManager.Logger.Log("已有该物品数据");
                    return dictionary[itemId][index];
                }
                UnityModManager.Logger.Log("已有该物品，但无属性" + index);
                dictionary[itemId].Add(index, 0);
            }
            else
            {
                UnityModManager.Logger.Log("无该物品数据");
                dictionary.Add(itemId, new Dictionary<string, int>());
            }
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>
            {
                {
                    "weaponDamage",
                    1
                },
                {
                    "weaponAttackTime",
                    2
                },
                {
                    "ActorBasicMAO",
                    3
                },
                {
                    "MoreAttackObbs",
                    4
                },
                {
                    "MoreAttackObbs2",
                    5
                }
            };
            int num = 50;
            int value = int.Parse(DateFile.instance.GetItemDate(itemId, 602, true, -1)) * num / 100;
            dictionary2["weaponDamage"] = value;
            int value2 = BattleVaule.instance.GetAttackNeedTime(itemId) * 100 / 25;
            dictionary2["weaponAttackTime"] = value2;
            int num2 = 0;
            int num3 = Main.settings.ShowMoreAttackTimeAtEffect ? 250 : 150;
            dictionary2["ActorBasicMAO"] = num3;
            int value3 = int.Parse(DateFile.instance.GetItemDate(itemId, 14, true, -1)) * (num3 - num2 * 20 - num2 * num2 * 5) / 100;
            num2 = 1;
            int value4 = int.Parse(DateFile.instance.GetItemDate(itemId, 14, true, -1)) * (num3 - num2 * 20 - num2 * num2 * 5) / 100;
            dictionary2["MoreAttackObbs"] = value3;
            dictionary2["MoreAttackObbs2"] = value4;
            foreach (string key in dictionary2.Keys)
            {
                bool flag3 = !dictionary[itemId].ContainsKey(key);
                if (flag3)
                {
                    dictionary[itemId].Add(key, 0);
                }
                dictionary[itemId][key] = dictionary2[key];
            }
            bool flag4 = dictionary.ContainsKey(itemId) && dictionary[itemId].ContainsKey(index);
            int result;
            if (flag4)
            {
                result = dictionary[itemId][index];
            }
            else
            {
                result = -1;
            }
            return result;
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00003644 File Offset: 0x00001844
        public static void Postfix(WindowManage __instance, int itemId, ref string ___baseWeaponMassage, ref Text ___informationMassage, ref Text ___informationName)
        {
            bool flag = Main.enabled && Main.settings.ShowItemMassage;
            if (flag)
            {
                string text = ___baseWeaponMassage;

                var item = Items.GetItem(itemId);
                if (item?.Count > 0)
                {
                    Main.Logger.DebugFileWriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(item));
                }

                if (DateFile.instance.presetitemDate.TryGetValue(itemId, out var dict2) && dict2?.Count > 0)
                {
                    Main.Logger.DebugFileWriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(dict2));
                }

                bool flag2 = int.Parse(DateFile.instance.GetItemDate(itemId, 1, true, -1)) == 1 && Main.settings.ShowWeaponMassage;
                if (flag2)
                {
                    int num = getItemMassage(itemId, "weaponDamage");
                    float num2 = 0.1f * (float)getItemMassage(itemId, "weaponAttackTime");
                    float num3 = (float)num / 10f;
                    int num4 = getItemMassage(itemId, "ActorBasicMAO");
                    int num5 = getItemMassage(itemId, "MoreAttackObbs");
                    string itemDate = DateFile.instance.GetItemDate(itemId, 14, true, -1);
                    text = string.Concat(new string[]
                    {
                        text,
                        DateFile.instance.SetColoer(10002, "\n【隐藏属性】\n", false),
                        WindowManage.instance.Dit(),
                        "基础伤害：",
                        DateFile.instance.SetColoer(20003, num3.ToString() + "%", false),
                        "\n",
                        WindowManage.instance.Dit(),
                        Main.settings.ShowMoreAttackTimeAtEffect ? "醉酒后" : "",
                        "连击概率：",
                        DateFile.instance.SetColoer(20006, num5.ToString() + "%", false),
                        "  "
                    });
                    num5 = ShowMeMore_WindowManage_ShowItemMassage_Patch.getItemMassage(itemId, "MoreAttackObbs2");
                    text = string.Concat(new string[]
                    {
                        text,
                        "(再次连击概率",
                        DateFile.instance.SetColoer(20006, num5.ToString() + "%", false),
                        ")\n",
                        WindowManage.instance.Dit(),
                        "连招常数：",
                        DateFile.instance.SetColoer(20001, itemDate, false),
                        "\n",
                        WindowManage.instance.Dit(),
                        "攻击时间：",
                        DateFile.instance.SetColoer(20006, num2.ToString(), false),
                        "\n"
                    });
                    ___baseWeaponMassage = text;
                    ___informationMassage.text = text;
                }
                string text2 = ___informationMassage.text;
                bool flag3 = Main.settings.ShowItemFavor && !BattleSystem.instance.battleWindow.activeSelf;
                if (flag3)
                {
                    int num6 = int.Parse(DateFile.instance.GetItemDate(itemId, 102, true, -1)) * 150 / 100;
                    object[] args = new object[]
                    {
                        text2,
                        "\n赠送增加好感：",
                        num6,
                        "\n"
                    };
                    text2 = string.Concat(args);
                }
                ___informationMassage.text = text2;
                ___baseWeaponMassage = text2;
            }
        }

        // Token: 0x04000012 RID: 18
        public static bool flag = false;

        // Token: 0x04000013 RID: 19
        public static ShowMeMore_WindowManage_ShowItemMassage_Patch instance;

        // Token: 0x04000014 RID: 20
        public Dictionary<int, Dictionary<string, int>> itemMassage = new Dictionary<int, Dictionary<string, int>>();
    }
}
