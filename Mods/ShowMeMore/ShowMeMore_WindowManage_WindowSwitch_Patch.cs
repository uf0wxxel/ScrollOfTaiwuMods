using System;
using System.Collections.Generic;
using Harmony12;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;

namespace ShowMeMore
{
    // Token: 0x02000008 RID: 8
    [HarmonyPatch(typeof(WindowManage), "WindowSwitch")]
    public static class ShowMeMore_WindowManage_WindowSwitch_Patch
    {
        // Token: 0x06000010 RID: 16 RVA: 0x00003930 File Offset: 0x00001B30
        private static int[] GetActorAllTrueQi(int id, bool Most)
        {
            int[] array = new int[6];
            foreach (int num in DateFile.instance.actorGongFas[id].Keys)
            {
                Dictionary<int, string> dictionary = DateFile.instance.gongFaDate[num];
                bool flag = int.Parse(dictionary[1]) <= 0;
                if (flag)
                {
                    int gongFaLevel = DateFile.instance.GetGongFaLevel(id, num, 0);
                    int num2 = (int.Parse(dictionary[4]) != 0) ? gongFaLevel : (Main.settings.AddHunYuan ? (gongFaLevel * 2) : 0);
                    int num3 = (int.Parse(dictionary[4]) != 0) ? 0 : (gongFaLevel * 2);
                    bool flag2 = !Most;
                    if (flag2)
                    {
                        array[0] += Convert.ToInt32(float.Parse(dictionary[701]) * (float)num3);
                    }
                    else
                    {
                        array[0] += Convert.ToInt32(float.Parse(dictionary[701]) * ((int.Parse(dictionary[4]) != 0) ? 0f : 200f));
                    }
                    for (int i = 0; i < array.Length - 1; i++)
                    {
                        bool flag3 = !Most;
                        if (flag3)
                        {
                            array[i + 1] += Convert.ToInt32(float.Parse(dictionary[i + 701]) * (float)num2);
                        }
                        else
                        {
                            array[i + 1] += Convert.ToInt32(float.Parse(dictionary[i + 701]) * ((int.Parse(dictionary[4]) != 0) ? 100f : (Main.settings.AddHunYuan ? 200f : 0f)));
                        }
                    }
                }
            }
            return array;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00003B50 File Offset: 0x00001D50
        private static int GetPlaceMark(int buildingId)
        {
            int num = 0;
            int num2 = 0;
            int num3 = int.Parse(DateFile.instance.basehomePlaceDate[buildingId][92]);
            int num4 = int.Parse(DateFile.instance.basehomePlaceDate[buildingId][93]);
            int num5 = int.Parse(DateFile.instance.basehomePlaceDate[buildingId][94]);
            int num6 = int.Parse(DateFile.instance.basehomePlaceDate[buildingId][95]);
            bool flag = num5 != 0;
            int result;
            if (flag)
            {
                result = -1;
            }
            else
            {
                foreach (int id in DateFile.instance.gangDate.Keys)
                {
                    int num7 = int.Parse(DateFile.instance.GetGangDate(id, 3));
                    bool flag2 = DateFile.instance.baseWorldDate[int.Parse(DateFile.instance.GetGangDate(id, 11))][num7][0] > 0;
                    if (flag2)
                    {
                        int placeId = int.Parse(DateFile.instance.GetGangDate(id, 4));
                        int num8 = int.Parse(DateFile.instance.GetNewMapDate(num7, placeId, 7));
                        int num9 = int.Parse(DateFile.instance.GetNewMapDate(num7, placeId, 8));
                        int num10 = (num5 <= 0) ? 0 : int.Parse(DateFile.instance.GetNewMapDate(num7, placeId, num5));
                        bool flag3 = (num3 == 0 || num8 != 0) && (num4 == 0 || num9 != 0) && (num6 == 0 || num10 != 0);
                        if (flag3)
                        {
                            int[] placeResource = DateFile.instance.GetPlaceResource(num7, placeId);
                            int num11 = 0;
                            int num12 = -1;
                            bool flag4 = num3 != 0;
                            if (flag4)
                            {
                                num11++;
                                num12 = ((num3 <= 0) ? ((placeResource[6] - 100) * 100 / num3) : ((placeResource[6] + 25) * 100 / num3));
                            }
                            int num13 = -1;
                            bool flag5 = num4 != 0;
                            if (flag5)
                            {
                                num11++;
                                num13 = ((num4 <= 0) ? ((placeResource[7] - 100) * 100 / num4) : ((placeResource[7] + 25) * 100 / num4));
                            }
                            int num14 = -1;
                            bool flag6 = num6 != 0;
                            if (flag6)
                            {
                                num11++;
                                num14 = ((num6 <= 0) ? ((placeResource[num5 - 1] - 100) * 100 / num6) : ((placeResource[num5 - 1] + 25) * 100 / num6));
                            }
                            int num15 = (num11 != 0) ? ((num12 + num13 + num14) / num11) : 100;
                            bool flag7 = num15 >= 50;
                            if (flag7)
                            {
                                num2++;
                                num += num15;
                            }
                        }
                    }
                }
                result = ((num2 != 0) ? (num / num2) : -1);
            }
            return result;
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00003E1C File Offset: 0x0000201C
        public static int GetTaiwuCiTangPrestige()
        {
            int key = int.Parse(DateFile.instance.GetGangDate(16, 3));
            int key2 = int.Parse(DateFile.instance.GetGangDate(16, 4));
            foreach (int key3 in DateFile.instance.homeBuildingsDate[key][key2].Keys)
            {
                int[] array = DateFile.instance.homeBuildingsDate[key][key2][key3];
                bool flag = array[0] == 1005;
                if (flag)
                {
                    return DateFile.instance.StudyGetPrestige(array[1]);
                }
            }
            return 0;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00003EF4 File Offset: 0x000020F4
        public static float[] GetTaiwuIncome()
        {
            float[] array = new float[2];
            int num = int.Parse(DateFile.instance.GetGangDate(16, 3));
            int num2 = int.Parse(DateFile.instance.GetGangDate(16, 4));
            bool showDebug = Main.settings.ShowDebug;
            if (showDebug)
            {
                UnityModManager.Logger.Log(string.Concat(new object[]
                {
                    "太吾村partid：",
                    num,
                    "，placeId：",
                    num2
                }));
            }
            bool flag = false;
            foreach (int key in DateFile.instance.homeBuildingsDate[num][num2].Keys)
            {
                int[] array2 = DateFile.instance.homeBuildingsDate[num][num2][key];
                bool flag2 = array2[0] == 1006;
                if (flag2)
                {
                    flag = true;
                }
            }
            bool flag3 = flag;
            if (flag3)
            {
                int[] array3 = new int[]
                {
                    ShowMeMore_WindowManage_WindowSwitch_Patch.GetPlaceMark(3605),
                    ShowMeMore_WindowManage_WindowSwitch_Patch.GetPlaceMark(3602),
                    ShowMeMore_WindowManage_WindowSwitch_Patch.GetPlaceMark(2030)
                };
                bool flag4 = ShowMeMore_WindowManage_WindowSwitch_Patch.GetPlaceMark(3605) == -1;
                if (flag4)
                {
                    return array;
                }
                foreach (int num3 in DateFile.instance.homeBuildingsDate[num][num2].Keys)
                {
                    bool flag5 = false;
                    bool flag6 = DateFile.instance.actorsWorkingDate.ContainsKey(num) && DateFile.instance.actorsWorkingDate[num].ContainsKey(num2) && DateFile.instance.actorsWorkingDate[num][num2].ContainsKey(num3);
                    if (flag6)
                    {
                        flag5 = true;
                    }
                    bool flag7 = flag5;
                    if (flag7)
                    {
                        int[] array4 = DateFile.instance.homeBuildingsDate[num][num2][num3];
                        bool flag8 = array4[0] != 0;
                        if (flag8)
                        {
                            int key2 = array4[0];
                            int num4 = array4[1];
                            string text = DateFile.instance.basehomePlaceDate[array4[0]][0];
                            int num5 = int.Parse(DateFile.instance.basehomePlaceDate[array4[0]][33]);
                            int num6 = int.Parse(DateFile.instance.basehomePlaceDate[array4[0]][96]);
                            int num7 = int.Parse(DateFile.instance.basehomePlaceDate[array4[0]][91]);
                            int buildingLevelPct = DateFile.instance.GetBuildingLevelPct(num, num2, num3);
                            float[] array5 = new float[4];
                            int[] array6 = new int[]
                            {
                                buildingLevelPct,
                                buildingLevelPct * 30 / 100,
                                buildingLevelPct * 15 / 100
                            };
                            bool flag9 = buildingLevelPct >= 100;
                            if (flag9)
                            {
                                array5[2] = 0f;
                                array5[3] = 0f;
                                array5[1] = (float)array6[2];
                                array5[0] = 100f - array5[1];
                            }
                            else
                            {
                                array5[3] = (float)(array6[0] * array6[1]) / 100f;
                                array5[2] = (float)(array6[0] * (100 - array6[1])) / 100f;
                                array5[1] = (float)((100 - array6[0]) * array6[2]) / 100f;
                                array5[0] = 100f - array5[1] - array5[2] - array5[3];
                            }
                            float num8 = (float)num7 / (float)buildingLevelPct;
                            float num9 = 12f / num8;
                            bool flag10 = num6 != 0;
                            if (flag10)
                            {
                                bool flag11 = int.Parse(DateFile.instance.basehomePlaceDate[key2][92]) > 0;
                                bool flag12 = int.Parse(DateFile.instance.basehomePlaceDate[key2][93]) > 0;
                                bool flag13 = int.Parse(DateFile.instance.basehomePlaceDate[key2][93]) < 0;
                                for (int i = 0; i < 4; i++)
                                {
                                    bool showDebug2 = Main.settings.ShowDebug;
                                    if (showDebug2)
                                    {
                                        UnityModManager.Logger.Log(string.Concat(new object[]
                                        {
                                            "现在在计算评价",
                                            i,
                                            "，该评价出现概率为",
                                            array5[i],
                                            "%"
                                        }));
                                    }
                                    System.Random random = new System.Random();
                                    char[] separator = new char[]
                                    {
                                        '|'
                                    };
                                    string[] array7 = DateFile.instance.homeShopEventTypDate[num6][i + 1].Split(separator);
                                    int key3 = int.Parse(array7[random.Next(0, array7.Length)]);
                                    char[] separator2 = new char[]
                                    {
                                        '|'
                                    };
                                    string[] array8 = DateFile.instance.homeShopEventDate[key3][11].Split(separator2);
                                    char[] separator3 = new char[]
                                    {
                                        '|'
                                    };
                                    string[] array9 = DateFile.instance.homeShopEventDate[key3][12].Split(separator3);
                                    char[] separator4 = new char[]
                                    {
                                        '|'
                                    };
                                    string[] array10 = DateFile.instance.homeShopEventDate[key3][13].Split(separator4);
                                    for (int j = 0; j < array8.Length; j++)
                                    {
                                        bool flag14 = int.Parse(array8[j]) == 1 && (flag11 || flag12 || flag13);
                                        if (flag14)
                                        {
                                            int num10 = array3[flag12 ? 1 : (flag13 ? 2 : 0)];
                                            int num11 = int.Parse(array9[j]) - 6;
                                            int num12 = Mathf.Max(1, int.Parse(array10[j]) * (80 + num4 * 8) / 100 * Mathf.Min(num10, 200) / 100);
                                            array[num11] += num9 * (float)num12 * array5[i] / 100f;
                                            bool showDebug3 = Main.settings.ShowDebug;
                                            if (showDebug3)
                                            {
                                                UnityModManager.Logger.Log(string.Concat(new object[]
                                                {
                                                    "现在正在计算编号为",
                                                    num3,
                                                    "的建筑",
                                                    text,
                                                    "，其",
                                                    (num11 == 0) ? "银钱" : "威望",
                                                    "单次收入为",
                                                    num12,
                                                    "，一年收入",
                                                    num9,
                                                    "次，平均地区资源影响率为",
                                                    num10
                                                }));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return array;
        }

        // Token: 0x06000014 RID: 20 RVA: 0x000045E4 File Offset: 0x000027E4
        public static void Postfix(bool on, GameObject tips, ref Text ___itemMoneyText, ref Text ___itemLevelText, ref Text ___informationMassage, ref Text ___informationName, ref bool ___anTips, ref int ___tipsW, ref int ___tipsH)
        {
            bool flag = Main.enabled && (on && Main.enabled && ActorMenu.instance != null) && tips != null;
            if (flag)
            {
                int id = DateFile.instance.MianActorID();
                string text = ___informationMassage.text;
                char[] separator = new char[]
                {
                    ','
                };
                string[] array = tips.name.Split(separator);
                int num = (array.Length <= 1) ? 0 : int.Parse(array[1]);
                bool flag2 = num == 713 && Main.settings.ShowQiRate;
                if (flag2)
                {
                    int[] array2 = new int[]
                    {
                        20003,
                        20008,
                        20007,
                        20006,
                        20010,
                        20004
                    };
                    int[] actorAllTrueQi = ShowMeMore_WindowManage_WindowSwitch_Patch.GetActorAllTrueQi(id, false);
                    int[] actorAllTrueQi2 = ShowMeMore_WindowManage_WindowSwitch_Patch.GetActorAllTrueQi(id, true);
                    text += DateFile.instance.SetColoer(10002, "\n【修习进度】\n", false);
                    for (int i = 0; i < actorAllTrueQi.Length; i++)
                    {
                        object[] array3 = new object[4];
                        array3[0] = WindowManage.instance.Dit();
                        char[] separator2 = new char[]
                        {
                            '|'
                        };
                        array3[1] = DateFile.instance.massageDate[2004][0].Split(separator2)[i];
                        array3[2] = DateFile.instance.SetColoer(array2[i], actorAllTrueQi[i].ToString(), false);
                        array3[3] = DateFile.instance.SetColoer(array2[i], actorAllTrueQi2[i].ToString(), false);
                        text += string.Format("{0}{1}: {2} / {3}\n", array3);
                    }
                    ___informationMassage.text = text;
                }
                else
                {
                    bool flag3 = tips.tag == "PlaceMassage" && Main.settings.ShowHomeMassage;
                    if (flag3)
                    {
                        int choosePartId = WorldMapSystem.instance.choosePartId;
                        int choosePlaceId = WorldMapSystem.instance.choosePlaceId;
                        bool flag4 = choosePartId == int.Parse(DateFile.instance.GetGangDate(16, 3)) && choosePlaceId == int.Parse(DateFile.instance.GetGangDate(16, 4));
                        if (flag4)
                        {
                            string text2 = "";
                            float[] taiwuIncome = ShowMeMore_WindowManage_WindowSwitch_Patch.GetTaiwuIncome();
                            int[] array4 = new int[]
                            {
                                ShowMeMore_WindowManage_WindowSwitch_Patch.GetPlaceMark(3605),
                                ShowMeMore_WindowManage_WindowSwitch_Patch.GetPlaceMark(3602),
                                ShowMeMore_WindowManage_WindowSwitch_Patch.GetPlaceMark(2030)
                            };
                            text2 = string.Concat(new string[]
                            {
                                text2,
                                "\n年均期望银钱收入：",
                                DateFile.instance.SetColoer(20008, string.Format("{0:F1}", taiwuIncome[0]).ToString(), false),
                                "\n年均期望威望收入：",
                                DateFile.instance.SetColoer(20007, string.Format("{0:F1}", taiwuIncome[1]).ToString(), false),
                                "\n\n当前已开通驿站地区平均影响率（不计低于50%的）：\n文化正相关：",
                                DateFile.instance.SetColoer(20005, array4[0].ToString(), false),
                                "%\n安定正相关：",
                                DateFile.instance.SetColoer(20004, array4[1].ToString(), false),
                                "%\n安定负相关：",
                                DateFile.instance.SetColoer(20010, array4[2].ToString(), false),
                                "%\n"
                            });
                            WindowManage.instance.informationMassage.text = WindowManage.instance.informationMassage.text + text2;
                        }
                    }
                }
            }
        }
    }
}
