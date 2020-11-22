using System;
using Harmony12;
using UnityEngine;

namespace ShowMeMore
{
    // Token: 0x02000004 RID: 4
    [HarmonyPatch(typeof(UIDate), "UpdateResourceText")]
    public static class ShowMeMore_UIDate_UpdateResourceText_Patch
    {
        // Token: 0x06000009 RID: 9 RVA: 0x00002590 File Offset: 0x00000790
        public static void Postfix(GameObject resource)
        {
            bool flag = Main.enabled && Main.settings.ShowHomeMassageAtTop && DateFile.instance.gameLine >= 10 && ActorMenu.instance != null && resource != null;
            if (flag)
            {
                int num = DateFile.instance.MianActorID();
                string text = WindowManage.instance.informationName.text;
                char[] separator = new char[]
                {
                    ','
                };
                string[] array = resource.name.Split(separator);
                int num2 = (array.Length <= 1) ? -1 : int.Parse(array[1]);
                bool flag2 = num2 != -1;
                if (flag2)
                {
                    float[] taiwuIncome = ShowMeMore_WindowManage_WindowSwitch_Patch.GetTaiwuIncome();
                    float[] array2 = new float[2];
                    bool flag3 = taiwuIncome[0] != array2[0];
                    if (flag3)
                    {
                        int num3 = num2;
                        int num4 = num3;
                        if (num4 != 5)
                        {
                            if (num4 == 6)
                            {
                                string text2 = "";
                                int taiwuCiTangPrestige = ShowMeMore_WindowManage_WindowSwitch_Patch.GetTaiwuCiTangPrestige();
                                int allQuquPrestige = DateFile.instance.GetAllQuquPrestige();
                                string[] values = new string[]
                                {
                                    text2,
                                    "\n",
                                    DateFile.instance.SetColoer(20007, "+ " + string.Format("{0:F1}", taiwuIncome[1]).ToString(), false),
                                    DateFile.instance.SetColoer(20011, " / 年（期望）", false),
                                    "来自",
                                    DateFile.instance.SetColoer(10002, "太吾村经营建筑", false),
                                    "\n"
                                };
                                text2 = string.Concat(values);
                                bool flag4 = taiwuCiTangPrestige != 0;
                                if (flag4)
                                {
                                    string[] values2 = new string[]
                                    {
                                        text2,
                                        DateFile.instance.SetColoer(20007, "+ " + taiwuCiTangPrestige.ToString() + " 威望", false),
                                        " / ",
                                        DateFile.instance.SetColoer(20008, "「立春」", false),
                                        "来自",
                                        DateFile.instance.SetColoer(20009, "太吾氏祠堂", false),
                                        "\n"
                                    };
                                    text2 = string.Concat(values2);
                                }
                                bool flag5 = allQuquPrestige > 0;
                                if (flag5)
                                {
                                    string[] values3 = new string[]
                                    {
                                        text2,
                                        DateFile.instance.SetColoer(20007, "+ " + allQuquPrestige.ToString() + " 威望", false),
                                        " / ",
                                        DateFile.instance.SetColoer(20008, "「立秋」", false),
                                        "来自",
                                        DateFile.instance.SetColoer(20009, "太吾村", false),
                                        "陈列的促织\n"
                                    };
                                    text2 = string.Concat(values3);
                                }
                                WindowManage.instance.informationMassage.text = WindowManage.instance.informationMassage.text + text2;
                            }
                        }
                        else
                        {
                            string text3 = "";
                            string[] values4 = new string[]
                            {
                                text3,
                                "\n",
                                DateFile.instance.SetColoer(20008, "+ " + string.Format("{0:F1}", taiwuIncome[0]).ToString(), false),
                                DateFile.instance.SetColoer(20011, " / 年（期望）", false),
                                "来自",
                                DateFile.instance.SetColoer(10002, "太吾村建筑", false),
                                "\n"
                            };
                            text3 = string.Concat(values4);
                            WindowManage.instance.informationMassage.text = WindowManage.instance.informationMassage.text + text3;
                        }
                    }
                }
            }
        }
    }
}
