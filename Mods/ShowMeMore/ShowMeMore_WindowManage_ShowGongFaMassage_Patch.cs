using System;
using Harmony12;
using UnityEngine.UI;

namespace ShowMeMore
{
    // Token: 0x02000006 RID: 6
    [HarmonyPatch(typeof(WindowManage), "ShowGongFaMassage")]
    public static class ShowMeMore_WindowManage_ShowGongFaMassage_Patch
    {
        // Token: 0x0600000B RID: 11 RVA: 0x00002C34 File Offset: 0x00000E34
        private static void Postfix(WindowManage __instance, int skillId, int skillTyp, int levelTyp, int actorId, Toggle toggle, ref Text ___informationMassage, ref string ___baseGongFaMassage)
        {
            bool flag = Main.enabled && Main.settings.ShowGongFaMassage && skillTyp != 0 && skillTyp == 1 && int.Parse(DateFile.instance.gongFaDate[skillId][10]) != 0;
            if (flag)
            {
                int.Parse(DateFile.instance.gongFaDate[skillId][103]);
                int actorId2 = (actorId == -1) ? ((!ActorMenu.instance.actorMenu.activeInHierarchy) ? DateFile.instance.MianActorID() : ActorMenu.instance.actorId) : actorId;
                if (levelTyp == -1 || levelTyp == 0)
                {
                    int num = (skillId != 0) ? DateFile.instance.GetGongFaFLevel(actorId2, skillId, false) : 0;
                }
                string text = ___baseGongFaMassage;
                string text2 = "";
                text2 += DateFile.instance.SetColoer(10002, "\n【隐藏属性】\n", false);
                float num2 = float.Parse(DateFile.instance.gongFaDate[skillId][604]);
                float num3 = float.Parse(DateFile.instance.gongFaDate[skillId][614]);
                float num4 = float.Parse(DateFile.instance.gongFaDate[skillId][615]);
                float num5 = float.Parse(DateFile.instance.gongFaDate[skillId][601]);
                float num6 = float.Parse(DateFile.instance.gongFaDate[skillId][602]);
                float num7 = float.Parse(DateFile.instance.gongFaDate[skillId][603]);
                int num8 = int.Parse(DateFile.instance.gongFaDate[skillId][2]);
                bool flag2 = num2 > 0f;
                if (flag2)
                {
                    text2 = text2 + WindowManage.instance.Dit() + "基础伤害：";
                    bool flag3 = num8 > 7;
                    int num9;
                    if (flag3)
                    {
                        num2 *= 10f;
                        num9 = 100;
                    }
                    else
                    {
                        num2 *= 8f;
                        num9 = 80;
                    }
                    text2 += DateFile.instance.SetColoer(20003, num2.ToString() + "%\n", false);
                    bool showHitRatio = Main.settings.ShowHitRatio;
                    if (showHitRatio)
                    {
                        num5 *= (float)num9;
                        num6 *= (float)num9;
                        num7 *= (float)num9;
                        text2 = text2 + WindowManage.instance.Dit() + "基础命中：";
                        bool flag4 = num7 < 0f;
                        if (flag4)
                        {
                            text2 = text2 + "迅疾" + DateFile.instance.SetColoer(20005, "无懈", false);
                        }
                        else
                        {
                            text2 = text2 + "迅疾" + DateFile.instance.SetColoer(20005, num7.ToString(), false);
                        }
                        bool flag5 = num6 < 0f;
                        if (flag5)
                        {
                            text2 = text2 + DateFile.instance.massageDate[10][4] + "精妙" + DateFile.instance.SetColoer(20005, "无懈", false);
                        }
                        else
                        {
                            text2 = text2 + DateFile.instance.massageDate[10][4] + "精妙" + DateFile.instance.SetColoer(20005, num6.ToString(), false);
                        }
                        bool flag6 = num5 < 0f;
                        if (flag6)
                        {
                            string[] values = new string[]
                            {
                                text2,
                                DateFile.instance.massageDate[10][4],
                                "力道",
                                DateFile.instance.SetColoer(20005, "无懈", false),
                                "\n"
                            };
                            text2 = string.Concat(values);
                        }
                        else
                        {
                            string[] values2 = new string[]
                            {
                                text2,
                                DateFile.instance.massageDate[10][4],
                                "力道",
                                DateFile.instance.SetColoer(20005, num5.ToString(), false),
                                "\n"
                            };
                            text2 = string.Concat(values2);
                        }
                        num3 *= (float)num9;
                        num4 *= (float)num9;
                        text2 = string.Concat(new string[]
                        {
                            text2,
                            WindowManage.instance.Dit(),
                            "基础穿透：",
                            DateFile.instance.SetColoer(20003, "破体", false),
                            DateFile.instance.SetColoer(20006, num3.ToString(), false),
                            DateFile.instance.massageDate[10][4],
                            DateFile.instance.SetColoer(20003, "破气", false),
                            DateFile.instance.SetColoer(20006, num4.ToString(), false),
                            "\n"
                        });
                    }
                }
                text2 = text2 + WindowManage.instance.Dit() + "释放时间：";
                bool flag7 = Main.settings.ShowRealTime && DateFile.instance.ActorIsInBattle(DateFile.instance.MianActorID()) != 0;
                if (flag7)
                {
                    text2 += DateFile.instance.SetColoer(20005, BattleVaule.instance.GetGongFaMaxUseTime(true, skillId).ToString(), false);
                }
                else
                {
                    float num10 = float.Parse(DateFile.instance.gongFaDate[skillId][10]) / 100f;
                    text2 += DateFile.instance.SetColoer(20005, num10.ToString(), false);
                }
                text2 += "\n";
                string text3 = "胸背|腰腹|头颈|左臂|右臂|左腿|右腿|心神|毒质|全身";
                string text4 = "";
                for (int i = 0; i < 10; i++)
                {
                    int key = 21 + i;
                    string text5 = DateFile.instance.gongFaDate[skillId][key];
                    bool flag8 = int.Parse(DateFile.instance.gongFaDate[skillId][key]) == 0;
                    if (!flag8)
                    {
                        char[] separator = new char[]
                        {
                            '|'
                        };
                        string text6 = text3.Split(separator)[i];
                        int num11 = i;
                        bool flag9 = num11 > 1;
                        if (flag9)
                        {
                            bool flag10 = num11 == 2;
                            if (flag10)
                            {
                                text4 += DateFile.instance.SetColoer(20008, text6, false);
                            }
                            else
                            {
                                text4 += DateFile.instance.SetColoer(20006, text6, false);
                            }
                        }
                        else
                        {
                            text4 += DateFile.instance.SetColoer(20009, text6, false);
                        }
                        text4 = text4 + DateFile.instance.gongFaDate[skillId][key] + " ";
                    }
                }
                bool flag11 = num2 > 0f;
                if (flag11)
                {
                    string[] values3 = new string[]
                    {
                        text2,
                        WindowManage.instance.Dit(),
                        "伤害部位及概率参数：\n  ",
                        text4,
                        "\n"
                    };
                    text2 = string.Concat(values3);
                }
                text2 += "\n";
                text += text2;
                ___baseGongFaMassage = text;
                ___informationMassage.text = text;
            }
        }
    }
}
