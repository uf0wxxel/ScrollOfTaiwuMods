using System.Collections.Generic;
using Harmony12;
using UnityEngine.UI;
using UnityModManagerNet;

namespace ShowMeMore
{
    // Token: 0x02000007 RID: 7
    [HarmonyPatch(typeof(WindowManage), "ShowItemMassage")]
    public class ShowMeMore_WindowManage_ShowItemMassage_Patch
    {
        public enum TaiwuItemProperty
        {
            weaponDamage = 1,
            weaponAttackTime = 2,
            ActorBasicMAO = 3,
            MoreAttackObbs = 4,
            MoreAttackObbs2 = 5,
        }

        private static IDictionary<int, Dictionary<TaiwuItemProperty, int>> _itemsInfoCache = new Dictionary<int, Dictionary<TaiwuItemProperty, int>>();

        // Token: 0x0600000C RID: 12 RVA: 0x000033A8 File Offset: 0x000015A8
        public static int getItemMassage(int itemId, TaiwuItemProperty taiwuItemProperty)
        {
            bool flag = _itemsInfoCache.ContainsKey(itemId);
            if (_itemsInfoCache.TryGetValue(itemId, out var itemInfoCache))
            {
                if (itemInfoCache.TryGetValue(taiwuItemProperty, out var property))
                {
                    return property;
                }

                itemInfoCache.Add(taiwuItemProperty, 0);
            }
            else
            {
                itemInfoCache = new Dictionary<TaiwuItemProperty, int>();
                _itemsInfoCache.Add(itemId, itemInfoCache);
            }

            var properties = new Dictionary<TaiwuItemProperty, int>();

            //int num = 50;
            properties[TaiwuItemProperty.weaponDamage] = int.Parse(DateFile.instance.GetItemDate(itemId, 602, true, -1)) * 50 / 100;
            //dictionary2["weaponDamage"] = value;
            properties[TaiwuItemProperty.weaponAttackTime] = BattleVaule.instance.GetAttackNeedTime(itemId) * 100 / 25;
            //dictionary2["weaponAttackTime"] = value2;
            int num2 = 0;
            var num3 = properties[TaiwuItemProperty.ActorBasicMAO] = Main.settings.ShowMoreAttackTimeAtEffect ? 250 : 150;
            //dictionary2["ActorBasicMAO"] = num3;
            int value3 = int.Parse(DateFile.instance.GetItemDate(itemId, 14, true, -1)) * (num3 - num2 * 20 - num2 * num2 * 5) / 100;
            num2 = 1;
            int value4 = int.Parse(DateFile.instance.GetItemDate(itemId, 14, true, -1)) * (num3 - num2 * 20 - num2 * num2 * 5) / 100;
            properties[TaiwuItemProperty.MoreAttackObbs] = value3;
            properties[TaiwuItemProperty.MoreAttackObbs2] = value4;
            foreach (var pair in properties)
            {
                itemInfoCache[pair.Key] = pair.Value;
            }

            if (itemInfoCache.TryGetValue(taiwuItemProperty, out var result))
            {
                return result;
            }
            else
            {
                return -1;
            }
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00003644 File Offset: 0x00001844
        public static void Postfix(WindowManage __instance, int itemId, ref string ___baseWeaponMassage, ref Text ___informationMassage, ref Text ___informationName)
        {
            bool flag = Main.enabled && Main.settings.ShowItemMassage;
            if (flag)
            {
                string text = ___baseWeaponMassage;

                bool flag2 = int.Parse(DateFile.instance.GetItemDate(itemId, 1, true, -1)) == 1 && Main.settings.ShowWeaponMassage;
                if (flag2)
                {
                    int num = getItemMassage(itemId, TaiwuItemProperty.weaponDamage);
                    float num2 = 0.1f * (float)getItemMassage(itemId, TaiwuItemProperty.weaponAttackTime);
                    float num3 = (float)num / 10f;
                    int num4 = getItemMassage(itemId, TaiwuItemProperty.ActorBasicMAO);
                    int num5 = getItemMassage(itemId, TaiwuItemProperty.MoreAttackObbs);
                    string itemDate = DateFile.instance.GetItemDate(itemId, 14, true, -1);
                    text = string.Concat(new string[]
                    {
                        text,
                        "\n【隐藏属性】\n".SetTaiwuColor(TaiwuTextColor.DeepBrown),
                        //DateFile.instance.SetColoer(10002, "\n【隐藏属性】\n", false),
                        WindowManage.instance.Dit(),
                        "基础伤害：",
                        $"{num3}%".SetTaiwuColor(TaiwuTextColor.LightYellow),
                        //DateFile.instance.SetColoer(20003, num3.ToString() + "%", false),
                        "\n",
                        WindowManage.instance.Dit(),
                        Main.settings.ShowMoreAttackTimeAtEffect ? "醉酒后" : "",
                        "连击概率：",
                        $"{num5}%".SetTaiwuColor(TaiwuTextColor.Cyan),
                        //DateFile.instance.SetColoer(20006, num5.ToString() + "%", false),
                        "  "
                    });
                    num5 = getItemMassage(itemId, TaiwuItemProperty.MoreAttackObbs2);
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
