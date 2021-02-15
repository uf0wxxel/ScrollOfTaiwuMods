using System;
using System.IO;
using System.Reflection;
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
    class BattleSystem_SetDamage_Patch
    {
        static bool Prefix(BattleSystem __instance, bool isActor, int damageTyp, int injuryId, int injuryPower, int deferKey, int attackerKey, float damageMassageSize, bool canDefttack = true, bool getWin = true, bool canTransfer = false, bool directDamage = true)
        {
            if (!Main.enabled) return true;
            bool gongFaFEffect = __instance.GetGongFaFEffect(40004, isActor, deferKey, 0);
            if (gongFaFEffect && isActor)
            {
                bool flag = __instance.actorGongFaSp[deferKey][0] >= __instance.enemyGongFaSp[attackerKey][0] * 2 && __instance.actorGongFaSp[deferKey][1] >= __instance.enemyGongFaSp[attackerKey][1] * 2 && __instance.actorGongFaSp[deferKey][2] >= __instance.enemyGongFaSp[attackerKey][2] * 2 && __instance.actorGongFaSp[deferKey][3] >= __instance.enemyGongFaSp[attackerKey][3] * 2;
                if (flag)
                {
                    __instance.ShowBattleState(40004, isActor, 0);
                    injuryPower = 0;
                    base.StartCoroutine(__instance.UpdateDamageText(isActor, 0, DateFile.instance.SetColoer(20002, "0", false), __instance.smallSize, true));
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(BattleSystem), "AutoSetDefGongFa")]
    class BattleSystem_AutoSetDefGongFa_Patch
    {
        static bool Prefix(BattleSystem __instance, bool isActor)
        {
            int num2 = __instance.ActorId(!isActor, false);
            bool gongFaFEffect = __instance.GetGongFaFEffect(30004, isActor, num, 0);
            if (gongFaFEffect)
            {
                __instance.ShowBattleState(30004, isActor, 0);
                List<int> list = new List<int>();
                for (int i = 0; i < 4; i++)
                {
                    bool flag = __instance.enemyGongFaSp[num][i] < __instance.actorGongFaSp[num2][i] * 2;
                    if (flag)
                    {
                        list.Add(i);
                    }
                }
                __instance.ChangeActorSp(isActor, (list.Count > 0) ? list[Random.Range(0, list.Count)] : -1, Random.Range(1, 4));
            }
            bool gongFaFEffect2 = __instance.GetGongFaFEffect(40004, isActor, num, 0);
            if (gongFaFEffect2)
            {
                __instance.ShowBattleState(40004, isActor, 0);
                List<int> list2 = new List<int>();
                for (int j = 0; j < 4; j++)
                {
                    bool flag2 = __instance.actorGongFaSp[num][j] < __instance.enemyGongFaSp[num2][j] * 2;
                    if (flag2)
                    {
                        list2.Add(j);
                    }
                }
                __instance.ChangeActorSp(isActor, (list2.Count > 0) ? list2[Random.Range(0, list2.Count)] : -1, Random.Range(1, 4));
            }
            int num3 = __instance.NowDefEffectId(isActor, num);
            bool flag3 = num3 != 0;
            if (flag3)
            {
                int num4 = num3;
                if (num4 <= 318)
                {
                    if (num4 != 278)
                    {
                        if (num4 == 318)
                        {
                            bool gongFaDefEffect = __instance.GetGongFaDefEffect(num3, isActor, num, 0);
                            if (gongFaDefEffect)
                            {
                                int num5 = isActor ? __instance.actorGongFaSp[num][0] : __instance.enemyGongFaSp[num][0];
                                __instance.SetPoisonDamage(!isActor, num2, 3, Random.Range(10, 40) + num5 * 3, true);
                            }
                        }
                    }
                    else
                    {
                        bool gongFaDefEffect2 = __instance.GetGongFaDefEffect(num3, isActor, num, 0);
                        if (gongFaDefEffect2)
                        {
                            __instance.DoHeal(isActor, 50, false, true, -1);
                        }
                    }
                }
                else if (num4 != 5278)
                {
                    if (num4 == 5318)
                    {
                        bool gongFaDefEffect3 = __instance.GetGongFaDefEffect(num3, isActor, num, 0);
                        if (gongFaDefEffect3)
                        {
                            int num6 = isActor ? __instance.actorGongFaSp[num][0] : __instance.enemyGongFaSp[num][0];
                            __instance.SetPoisonDamage(!isActor, num2, 1, Random.Range(10, 40) + num6 * 3, true);
                        }
                    }
                }
                else
                {
                    bool gongFaDefEffect4 = __instance.GetGongFaDefEffect(num3, isActor, num, 0);
                    if (gongFaDefEffect4)
                    {
                        List<int> list3 = new List<int>(DateFile.instance.battleActorsInjurys[num2].Keys);
                        foreach (int injuryId in list3)
                        {
                            __instance.WorsenInjury(num2, injuryId, 50, false, true);
                        }
                    }
                }
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(BattleSystem), "UseGongFa")]
    class BattleSystem_UseGongFa_Patch
    {
        static bool Prefix(BattleSystem __instance, bool isActor, int gongFaId, bool noPrepare = false)
        {
            __instance.StopGongFaTime(true);
            int num = 0;
            int num2 = __instance.ActorId(isActor, false);
            int num3 = __instance.ActorId(!isActor, true);
            int num4 = int.Parse(DateFile.instance.gongFaDate[gongFaId][2]);
            int combatSkillDataInt = DateFile.GetCombatSkillDataInt(gongFaId, 7, num2, true);
            if (noPrepare)
            {
                bool flag = !(isActor ? __instance._actorNextImmediateGongfaNotShowName : __instance._enemyNextImmediateGongfaNotShowName);
                if (flag)
                {
                    Image[] array = isActor ? BattleSystem.instance.actorUseGongFaTimeBar : BattleSystem.instance.enemyUseGongFaTimeBar;
                    BattleSystem.instance.ShowGongFaName(isActor, gongFaId);
                    array[0].fillAmount = 1f;
                    array[1].fillAmount = 1f;
                }
                else
                {
                    bool isActor2 = isActor;
                    if (isActor2)
                    {
                        __instance._actorNextImmediateGongfaNotShowName = false;
                    }
                    else
                    {
                        __instance._enemyNextImmediateGongfaNotShowName = false;
                    }
                }
            }
            GEvent.OnEvent(eEvents.Combat_PerformSkillBegin, new object[]
            {
                num2,
                isActor,
                gongFaId,
                num3,
                noPrepare
            });
            int actorQiTyp = DateFile.instance.GetActorQiTyp(num2);
            bool flag2 = int.Parse(DateFile.instance.qiValueStateDate[actorQiTyp][5]) == int.Parse(DateFile.instance.gongFaDate[gongFaId][4]);
            if (flag2)
            {
                string[] array2 = DateFile.instance.gongFaDate[gongFaId][69].Split(new char[]
                {
                    '|'
                });
                int key = int.Parse(array2[Random.Range(0, array2.Length)]);
                int num5 = int.Parse(DateFile.instance.qiValueStateDate[actorQiTyp][103]);
                List<string> list = new List<string>(DateFile.instance.bodyInjuryDate[key][10 + num5].Split(new char[]
                {
                    '|'
                }));
                int injuryId = int.Parse(list[Random.Range(0, list.Count)]);
                int num6 = combatSkillDataInt * Random.Range(100, 201);
                num += num6;
                __instance.SetRealDamage(isActor, 1, injuryId, num6, num2, (num5 == 2) ? __instance.mediumSize : ((num5 == 3) ? __instance.largeSize : __instance.smallSize), false, num2, false);
                __instance.ShowBattleState(10009, isActor, 0);
            }
            bool flag3 = false;
            Dictionary<int, int[]> dictionary = new Dictionary<int, int[]>(DateFile.instance.GetActorEquipGongFa(num2));
            for (int i = 0; i < dictionary[0].Length; i++)
            {
                int num7 = dictionary[0][i];
                bool flag4 = num7 > 0;
                if (flag4)
                {
                    int gongFaFTyp = DateFile.instance.GetGongFaFTyp(num2, num7);
                    bool flag5 = gongFaFTyp == 1;
                    if (flag5)
                    {
                        int key2 = int.Parse(DateFile.instance.gongFaDate[num7][103 + gongFaFTyp]);
                        int num8 = int.Parse(DateFile.instance.gongFaFPowerDate[key2][301]);
                        bool flag6 = num8 != 0 && int.Parse(DateFile.instance.gongFaDate[gongFaId][3]) == num8;
                        if (flag6)
                        {
                            flag3 = true;
                            num += combatSkillDataInt * Random.Range(50, 101);
                        }
                        int num9 = int.Parse(DateFile.instance.gongFaFPowerDate[key2][302]);
                        bool flag7 = num9 != -1 && (int.Parse(DateFile.instance.gongFaDate[gongFaId][4]) == num9 || num9 == 6);
                        if (flag7)
                        {
                            flag3 = true;
                            num += combatSkillDataInt * Random.Range(50, 101);
                        }
                    }
                }
            }
            bool flag8 = num > 0;
            if (flag8)
            {
                bool flag9 = flag3;
                if (flag9)
                {
                    __instance.ShowBattleState(10010, isActor, 0);
                }
                DateFile.instance.ChangeMianQi(num2, num, 5);
            }
            bool flag10 = __instance.battleEnd;
            if (!flag10)
            {
                bool isActor3 = isActor;
                if (isActor3)
                {
                    bool flag11 = num4 > __instance.actorUseGongFaMaxLevel;
                    if (flag11)
                    {
                        __instance.actorUseGongFaMaxLevel = num4;
                    }
                }
                else
                {
                    bool flag12 = num4 > __instance.enemyUseGongFaMaxLevel;
                    if (flag12)
                    {
                        __instance.enemyUseGongFaMaxLevel = num4;
                    }
                }
                bool gongFaFEffect = __instance.GetGongFaFEffect(5235, isActor, num2, 0);
                if (gongFaFEffect)
                {
                    List<int> list2 = new List<int>();
                    for (int j = 0; j < 7; j++)
                    {
                        bool flag13 = __instance.battlerQiDamagePart[num2][j] > BattleSystem.GetNotHealBlockedAcupointCount(num2, j);
                        if (flag13)
                        {
                            list2.Add(j);
                        }
                    }
                    bool flag14 = list2.Count > 0;
                    if (flag14)
                    {
                        __instance.ShowBattleState(5235, isActor, 0);
                        __instance.ChangeQiDamagePart(isActor, num2, num2, list2[Random.Range(0, list2.Count)], -3, true, true);
                    }
                }
                switch (int.Parse(DateFile.instance.gongFaDate[gongFaId][6]))
                {
                case 1:
                {
                    __instance.useGongFaMask.CrossFadeAlpha(0.6f, 1f, true);
                    bool flag15 = !isActor && __instance.maxBattleDefAttack > 0;
                    if (flag15)
                    {
                        int num10 = (__instance.battleDefAttack + 1 == __instance.maxBattleDefAttack) ? 9 : __instance.battleDefAttack;
                        __instance.ShowBattleState(10201 + num10, false, 0);
                    }
                    int num11 = __instance.NowDefEffectId(!isActor, num3);
                    bool flag16 = num11 != 0;
                    if (flag16)
                    {
                        int num12 = num11;
                        if (num12 != 287)
                        {
                            if (num12 == 5287)
                            {
                                bool gongFaDefEffect = __instance.GetGongFaDefEffect(num11, !isActor, num3, 0);
                                if (gongFaDefEffect)
                                {
                                    __instance.AddBattleState(num3, 1, 505287, 100 + DateFile.instance.GetActorValue(num3, 514, true) / 5, 1);
                                }
                            }
                        }
                        else
                        {
                            bool gongFaDefEffect2 = __instance.GetGongFaDefEffect(num11, !isActor, num3, 0);
                            if (gongFaDefEffect2)
                            {
                                __instance.ChangeActorSp(!isActor, -1, Mathf.Max(DateFile.instance.GetActorValue(num3, 514, true) / 50, 1));
                            }
                        }
                    }
                    bool gongFaFEffect2 = __instance.GetGongFaFEffect(276, isActor, num2, 0);
                    if (gongFaFEffect2)
                    {
                        List<int> list3 = new List<int>();
                        for (int k = 0; k < 6; k++)
                        {
                            bool flag17 = DateFile.instance.Poison(num3, k, true) >= DateFile.instance.MaxPoison(num3, k, true);
                            if (flag17)
                            {
                                list3.Add(k);
                            }
                        }
                        bool flag18 = list3.Count > 0;
                        if (flag18)
                        {
                            __instance.ShowBattleState(276, isActor, 0);
                            __instance.SetPoisonEffect(!isActor, num3, list3[Random.Range(0, list3.Count)], 0, true, -1);
                        }
                    }
                    bool flag19 = int.Parse(DateFile.instance.GetActorDate(num2, 15, true)) > int.Parse(DateFile.instance.GetActorDate(num3, 15, true)) && __instance.GetGongFaFEffect(261, isActor, num2, 0);
                    if (flag19)
                    {
                        int combatSkillDataInt2 = DateFile.GetCombatSkillDataInt(gongFaId, 7, num2, true);
                        __instance.SetRealDamage(!isActor, 1, 45 + combatSkillDataInt2, Random.Range(150, 301), num3, (combatSkillDataInt2 == 2) ? __instance.mediumSize : ((combatSkillDataInt2 == 3) ? __instance.largeSize : __instance.smallSize), false, num2, true);
                    }
                    bool flag20 = int.Parse(DateFile.instance.GetActorDate(num3, 15, true)) > int.Parse(DateFile.instance.GetActorDate(num2, 15, true)) && __instance.GetGongFaFEffect(5261, !isActor, num3, 0);
                    if (flag20)
                    {
                        int combatSkillDataInt3 = DateFile.GetCombatSkillDataInt(gongFaId, 7, num2, true);
                        __instance.SetRealDamage(isActor, 1, 45 + combatSkillDataInt3, Random.Range(150, 301), num2, (combatSkillDataInt3 == 2) ? __instance.mediumSize : ((combatSkillDataInt3 == 3) ? __instance.largeSize : __instance.smallSize), false, num3, true);
                    }
                    bool gongFaFEffect3 = __instance.GetGongFaFEffect(30010, isActor, num2, 0);
                    if (gongFaFEffect3)
                    {
                        int count = DateFile.instance.GetLifeDateList(num2, 801, false).Count;
                        int count2 = DateFile.instance.GetLifeDateList(num3, 801, false).Count;
                        int num13 = count - count2;
                        bool flag21 = num13 > 0;
                        if (flag21)
                        {
                            __instance.ShowBattleState(30010, isActor, 0);
                            for (int l = 0; l < num13; l++)
                            {
                                int num14 = (Random.Range(0, 100) < 50) ? 0 : 1;
                                __instance.SetRealDamage(!isActor, num14, (num14 == 0) ? 57 : 60, Random.Range(300, 600), num3, __instance.largeSize, false, num2, true);
                            }
                        }
                    }
                    bool gongFaFEffect4 = __instance.GetGongFaFEffect(40010, isActor, num2, 0);
                    if (gongFaFEffect4)
                    {
                        int count3 = DateFile.instance.GetLifeDateList(num2, 801, false).Count;
                        int count4 = DateFile.instance.GetLifeDateList(num3, 801, false).Count;
                        int num15 = count3 - count4;
                        bool flag22 = num15 > 0;
                        if (flag22)
                        {
                            __instance.ShowBattleState(30010, isActor, 0);
                            for (int m = 0; m < num15; m++)
                            {
                                int num16 = (Random.Range(0, 100) < 50) ? 0 : 1;
                                __instance.SetRealDamage(!isActor, num16, (num16 == 0) ? 57 : 60, Random.Range(150, 300), num3, __instance.largeSize, false, num2, true);
                            }
                        }
                    }
                    bool flag23 = __instance.battleEnd;
                    if (!flag23)
                    {
                        bool flag24 = DateFile.instance.HaveLifeDate(num3, 501);
                        if (flag24)
                        {
                            int num17 = Mathf.Clamp(DateFile.instance.actorLife[num3][501][0], 0, 200);
                            bool flag25 = num17 > 100 && __instance.GetGongFaFEffect(20006, isActor, num2, 0);
                            if (flag25)
                            {
                                int num18 = num17 * num17 / 80;
                                __instance.SetPoisonDamage(!isActor, num3, Random.Range(0, 6), Random.Range(num18, num18 * 2 + 1), true);
                            }
                        }
                        bool gongFaFEffect5 = __instance.GetGongFaFEffect(5276, isActor, num2, 0);
                        if (gongFaFEffect5)
                        {
                            for (int n = 0; n < 6; n++)
                            {
                                int num19 = DateFile.instance.battleActorsPoisons[num2][n];
                                __instance.SetPoisonDamage(!isActor, num3, n, num19 * 20 / 100, true);
                                __instance.SetPoisonDamage(isActor, num2, n, -(num19 * 20 / 100), true);
                            }
                        }
                        bool gongFaFEffect6 = __instance.GetGongFaFEffect(20009, isActor, num2, 0);
                        if (gongFaFEffect6)
                        {
                            List<int> list4 = new List<int>();
                            for (int num20 = 0; num20 < 4; num20++)
                            {
                                bool flag26 = isActor ? (__instance.enemyGongFaSp[num3][num20] > 0) : (__instance.actorGongFaSp[num3][num20] > 0);
                                if (flag26)
                                {
                                    list4.Add(num20);
                                }
                            }
                            int num21 = (list4.Count > 0) ? list4[Random.Range(0, list4.Count)] : Random.Range(0, 4);
                            int num22 = Mathf.Min(3, isActor ? __instance.enemyGongFaSp[num3][num21] : __instance.actorGongFaSp[num3][num21]);
                            __instance.ChangeActorSp(isActor, num21, num22);
                            __instance.ChangeActorSp(!isActor, num21, -num22);
                        }
                        bool flag27 = __instance.enemyTeamId == 4 & isActor;
                        if (flag27)
                        {
                            __instance.TimePause(0f, true, 0f);
                            UIManager.Instance.AddUI("ui_MessageWindow", new object[]
                            {
                                new int[]
                                {
                                    0,
                                    -1,
                                    13428
                                },
                                true
                            });
                        }
                        bool flag28 = DateFile.GetCombatSkillConsumedMobility(gongFaId, num2) > 0;
                        if (flag28)
                        {
                            int num23 = isActor ? __instance.actorLegGongFaMoveTyp : __instance.enemyLegGongFaMoveTyp;
                            int num24 = num23;
                            if (num24 <= 460)
                            {
                                if (num24 != 430)
                                {
                                    if (num24 == 460)
                                    {
                                        bool gongFaMoveEffect = __instance.GetGongFaMoveEffect(num23, isActor, num2, 0);
                                        if (gongFaMoveEffect)
                                        {
                                            __instance.ShowBattleState(num23, isActor, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect2 = __instance.GetGongFaMoveEffect(num23, isActor, num2, 0);
                                    if (gongFaMoveEffect2)
                                    {
                                        __instance.ShowBattleState(num23, isActor, 0);
                                    }
                                }
                            }
                            else if (num24 != 490)
                            {
                                if (num24 != 5430)
                                {
                                    if (num24 == 5490)
                                    {
                                        bool gongFaMoveEffect3 = __instance.GetGongFaMoveEffect(num23, isActor, num2, 0);
                                        if (gongFaMoveEffect3)
                                        {
                                            __instance.ShowBattleState(num23, isActor, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect4 = __instance.GetGongFaMoveEffect(num23, isActor, num2, 0);
                                    if (gongFaMoveEffect4)
                                    {
                                        __instance.ShowBattleState(num23, isActor, 0);
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaMoveEffect5 = __instance.GetGongFaMoveEffect(num23, isActor, num2, 0);
                                if (gongFaMoveEffect5)
                                {
                                    __instance.ShowBattleState(num23, isActor, 0);
                                }
                            }
                        }
                        SingletonObject.getInstance<YieldHelper>().DelayFrameDo(1U, delegate
                        {
                            __instance.ShowGongFaDef(isActor, gongFaId);
                        });
                    }
                    break;
                }
                case 2:
                {
                    __instance.SetMoveGongFa(isActor, gongFaId);
                    int num25 = __instance.NowMoveEffectId(isActor, num2);
                    bool flag29 = num25 != 0;
                    if (flag29)
                    {
                        int num26 = num25;
                        if (num26 <= 493)
                        {
                            if (num26 <= 448)
                            {
                                if (num26 <= 410)
                                {
                                    if (num26 != 403)
                                    {
                                        if (num26 == 410)
                                        {
                                            bool gongFaMoveEffect6 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                            if (gongFaMoveEffect6)
                                            {
                                                __instance.ChangeMoveCost(isActor, -1, 25 + Mathf.Min((isActor ? __instance.actorActionCost.Count : __instance.enemyActionCost.Count) * 10, 50), false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool gongFaMoveEffect7 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect7)
                                        {
                                            __instance.ChangeActorSp(isActor, 1, 1);
                                        }
                                    }
                                }
                                else if (num26 != 428)
                                {
                                    switch (num26)
                                    {
                                    case 436:
                                    {
                                        bool gongFaMoveEffect8 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect8)
                                        {
                                            int num27 = isActor ? __instance.actorGongFaSp[num2][0] : __instance.enemyGongFaSp[num2][0];
                                            __instance.ChangeActorSp(isActor, 0, -num27);
                                            __instance.ChangeActorSp(isActor, 1, num27);
                                        }
                                        break;
                                    }
                                    case 437:
                                    {
                                        bool gongFaMoveEffect9 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect9)
                                        {
                                            int num28 = 6000;
                                            __instance.UpdateStrength(isActor, (float)num28, false);
                                            __instance.UpdateStrength(!isActor, (float)(-(float)num28), false);
                                        }
                                        break;
                                    }
                                    case 438:
                                    case 439:
                                        break;
                                    case 440:
                                    {
                                        bool gongFaMoveEffect10 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect10)
                                        {
                                            __instance.ShowBattleState(num25, isActor, 0);
                                        }
                                        break;
                                    }
                                    default:
                                        if (num26 == 448)
                                        {
                                            bool gongFaMoveEffect11 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                            if (gongFaMoveEffect11)
                                            {
                                                __instance.ShowBattleState(num25, isActor, 0);
                                            }
                                        }
                                        break;
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect12 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect12)
                                    {
                                        __instance.UpdateMagic(isActor, 30000f, false);
                                    }
                                }
                            }
                            else if (num26 <= 458)
                            {
                                if (num26 != 449)
                                {
                                    if (num26 != 454)
                                    {
                                        if (num26 == 458)
                                        {
                                            bool gongFaMoveEffect13 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                            if (gongFaMoveEffect13)
                                            {
                                                __instance.ShowBattleState(num25, isActor, 0);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool gongFaMoveEffect14 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect14)
                                        {
                                            __instance.ShowBattleState(num25, isActor, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect15 = __instance.GetGongFaMoveEffect(num25, isActor, num2, int.Parse(DateFile.instance.GetActorDate(num2, 15, true)) / 10);
                                    if (gongFaMoveEffect15)
                                    {
                                        __instance.ChangeActorSp(isActor, 1, 2);
                                    }
                                }
                            }
                            else if (num26 != 463)
                            {
                                if (num26 != 481)
                                {
                                    if (num26 == 493)
                                    {
                                        bool gongFaMoveEffect16 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect16)
                                        {
                                            __instance.AddBattleState(num3, 2, 500493, 100, 1);
                                        }
                                    }
                                }
                                else
                                {
                                    int num29 = isActor ? __instance.actorGongFaSp[num2][1] : __instance.enemyGongFaSp[num2][1];
                                    bool flag30 = num29 >= 9 && __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (flag30)
                                    {
                                        __instance.ChangeActorSp(isActor, 1, -9);
                                        __instance.UpdateRangeText(90, isActor);
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaMoveEffect17 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                if (gongFaMoveEffect17)
                                {
                                    __instance.ChangeMoveCost(!isActor, 1, -50, true);
                                }
                            }
                        }
                        else if (num26 <= 5448)
                        {
                            if (num26 <= 5428)
                            {
                                if (num26 != 5403)
                                {
                                    if (num26 != 5410)
                                    {
                                        if (num26 == 5428)
                                        {
                                            bool gongFaMoveEffect18 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                            if (gongFaMoveEffect18)
                                            {
                                                __instance.UpdateStrength(isActor, 30000f, false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool gongFaMoveEffect19 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect19)
                                        {
                                            __instance.ChangeMoveCost(isActor, -1, 75 - Mathf.Min((isActor ? __instance.actorActionCost.Count : __instance.enemyActionCost.Count) * 10, 50), false);
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect20 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect20)
                                    {
                                        __instance.ChangeActorSp(!isActor, 1, -1);
                                    }
                                }
                            }
                            else if (num26 != 5429)
                            {
                                switch (num26)
                                {
                                case 5436:
                                {
                                    bool gongFaMoveEffect21 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect21)
                                    {
                                        int num30 = isActor ? __instance.actorGongFaSp[num2][2] : __instance.enemyGongFaSp[num2][2];
                                        __instance.ChangeActorSp(isActor, 2, -num30);
                                        __instance.ChangeActorSp(isActor, 1, num30);
                                    }
                                    break;
                                }
                                case 5437:
                                {
                                    bool gongFaMoveEffect22 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect22)
                                    {
                                        int num31 = 6000;
                                        __instance.UpdateMagic(isActor, (float)num31, false);
                                        __instance.UpdateMagic(!isActor, (float)(-(float)num31), false);
                                    }
                                    break;
                                }
                                case 5438:
                                case 5439:
                                    break;
                                case 5440:
                                {
                                    bool gongFaMoveEffect23 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect23)
                                    {
                                        __instance.ShowBattleState(num25, isActor, 0);
                                    }
                                    break;
                                }
                                default:
                                    if (num26 == 5448)
                                    {
                                        bool gongFaMoveEffect24 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect24)
                                        {
                                            __instance.ShowBattleState(num25, isActor, 0);
                                        }
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                bool gongFaMoveEffect25 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                if (gongFaMoveEffect25)
                                {
                                    __instance.RemoveRandWeaponCd(isActor);
                                }
                            }
                        }
                        else if (num26 <= 5458)
                        {
                            if (num26 != 5449)
                            {
                                if (num26 != 5454)
                                {
                                    if (num26 == 5458)
                                    {
                                        bool gongFaMoveEffect26 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect26)
                                        {
                                            __instance.ShowBattleState(num25, isActor, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect27 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect27)
                                    {
                                        __instance.ShowBattleState(num25, isActor, 0);
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaMoveEffect28 = __instance.GetGongFaMoveEffect(num25, isActor, num2, int.Parse(DateFile.instance.GetActorDate(num2, 15, true)) / 10);
                                if (gongFaMoveEffect28)
                                {
                                    __instance.ChangeActorSp(!isActor, 1, -2);
                                }
                            }
                        }
                        else if (num26 != 5463)
                        {
                            if (num26 != 5481)
                            {
                                if (num26 == 5493)
                                {
                                    bool gongFaMoveEffect29 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect29)
                                    {
                                        __instance.AddBattleState(num3, 2, 505493, 100, 1);
                                    }
                                }
                            }
                            else
                            {
                                int num32 = isActor ? __instance.actorGongFaSp[num2][1] : __instance.enemyGongFaSp[num2][1];
                                bool flag31 = num32 >= 9 && __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                if (flag31)
                                {
                                    __instance.ChangeActorSp(isActor, 1, -9);
                                    __instance.UpdateRangeText(20, isActor);
                                }
                            }
                        }
                        else
                        {
                            bool gongFaMoveEffect30 = __instance.GetGongFaMoveEffect(num25, isActor, num2, 0);
                            if (gongFaMoveEffect30)
                            {
                                __instance.RemoveMoveCost(!isActor, 0, true, false, false);
                            }
                        }
                    }
                    break;
                }
                case 3:
                {
                    __instance.SetDefGongFa(isActor, gongFaId);
                    int num33 = __instance.NowDefEffectId(isActor, num2);
                    bool flag32 = num33 != 0;
                    if (flag32)
                    {
                        int num34 = num33;
                        if (num34 <= 315)
                        {
                            if (num34 <= 269)
                            {
                                if (num34 <= 248)
                                {
                                    if (num34 != 232)
                                    {
                                        if (num34 == 248)
                                        {
                                            bool gongFaDefEffect3 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                            if (gongFaDefEffect3)
                                            {
                                                int num35 = __instance.battlerDpValue[num2][0];
                                                bool flag33 = num35 > 0;
                                                if (flag33)
                                                {
                                                    __instance.ChangeActorSp(isActor, 0, num35 * 100 / Mathf.Max(__instance.battlerDpValue[num2][1], 1) / 10);
                                                    BattleSystem.AddDefence(num2, isActor, -num35);
                                                    __instance.ShowBattleState(num33, isActor, 0);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool gongFaDefEffect4 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                        if (gongFaDefEffect4)
                                        {
                                            bool flag34 = false;
                                            for (int num36 = 0; num36 < 4; num36++)
                                            {
                                                int num37 = isActor ? __instance.actorGongFaSp[num2][num36] : __instance.enemyGongFaSp[num2][num36];
                                                bool flag35 = num37 < DateFile.instance.GetMaxGongFaSp(num2, num36, false);
                                                if (flag35)
                                                {
                                                    flag34 = true;
                                                    __instance.ChangeActorSp(isActor, num36, 5);
                                                }
                                            }
                                            bool flag36 = flag34;
                                            if (flag36)
                                            {
                                                __instance.ShowBattleState(num33, isActor, 0);
                                            }
                                        }
                                    }
                                }
                                else if (num34 != 255)
                                {
                                    if (num34 == 269)
                                    {
                                        bool gongFaDefEffect5 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                        if (gongFaDefEffect5)
                                        {
                                            bool flag37 = false;
                                            for (int num38 = 0; num38 < 3; num38++)
                                            {
                                                int num39 = int.Parse(DateFile.instance.GetActorDate(num2, 308 + num38, false));
                                                bool flag38 = num39 > 0 && int.Parse(DateFile.instance.GetItemDate(num39, 901, true, -1)) > 0;
                                                if (flag38)
                                                {
                                                    switch (int.Parse(DateFile.instance.GetItemDate(num39, 506, true, -1)))
                                                    {
                                                    case 0:
                                                        flag37 = true;
                                                        __instance.EquipBreak(num2, isActor, num39, num2, 1, 100);
                                                        __instance.ChangeActorSp(isActor, 1, 1);
                                                        break;
                                                    case 1:
                                                        flag37 = true;
                                                        __instance.EquipBreak(num2, isActor, num39, num2, 1, 100);
                                                        __instance.ChangeActorSp(isActor, 0, 1);
                                                        break;
                                                    case 2:
                                                        flag37 = true;
                                                        __instance.EquipBreak(num2, isActor, num39, num2, 1, 100);
                                                        __instance.ChangeActorSp(isActor, 3, 1);
                                                        break;
                                                    case 3:
                                                        flag37 = true;
                                                        __instance.EquipBreak(num2, isActor, num39, num2, 1, 100);
                                                        __instance.ChangeActorSp(isActor, 2, 1);
                                                        break;
                                                    }
                                                }
                                            }
                                            bool flag39 = flag37;
                                            if (flag39)
                                            {
                                                __instance.ShowBattleState(num33, isActor, 0);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaDefEffect6 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                    if (gongFaDefEffect6)
                                    {
                                        int totalFlaws = BattleSystem.GetTotalFlaws(num2);
                                        bool flag40 = totalFlaws > 0;
                                        if (flag40)
                                        {
                                            BattleSystem.TransferFlawToEnemy(isActor, num2, num3, -1, Mathf.Min(totalFlaws, 3));
                                            __instance.ShowBattleState(num33, !isActor, 0);
                                        }
                                    }
                                }
                            }
                            else if (num34 <= 291)
                            {
                                if (num34 != 284)
                                {
                                    if (num34 == 291)
                                    {
                                        bool gongFaDefEffect7 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                        if (gongFaDefEffect7)
                                        {
                                            int num40 = isActor ? __instance.enemyBugSize : __instance.actorBugSize;
                                            bool flag41 = num40 > 0;
                                            if (flag41)
                                            {
                                                __instance.ChangeBugSize(!isActor, -3);
                                                __instance.ShowBattleState(num33, isActor, 0);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaDefEffect8 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                    if (gongFaDefEffect8)
                                    {
                                        __instance.ChangeActorSp(isActor, 2, 2);
                                    }
                                }
                            }
                            else if (num34 != 313)
                            {
                                if (num34 == 315)
                                {
                                    bool gongFaDefEffect9 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                    if (gongFaDefEffect9)
                                    {
                                        __instance.UpdateBattlerMove(!isActor, 20, null, null, false);
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaDefEffect10 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                if (gongFaDefEffect10)
                                {
                                    __instance.AddBattleState(num3, 2, 500313, 50 + Mathf.Max(150 - DateFile.instance.GetActorResources(num3)[3] * 5, 0), 1);
                                }
                            }
                        }
                        else if (num34 <= 5269)
                        {
                            if (num34 <= 5248)
                            {
                                if (num34 != 5232)
                                {
                                    if (num34 == 5248)
                                    {
                                        bool gongFaDefEffect11 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                        if (gongFaDefEffect11)
                                        {
                                            int num41 = __instance.battlerDpValue[num2][0];
                                            bool flag42 = num41 > 0;
                                            if (flag42)
                                            {
                                                __instance.ChangeActorSp(isActor, 2, num41 * 100 / Mathf.Max(__instance.battlerDpValue[num2][1], 1) / 10);
                                                BattleSystem.AddDefence(num2, isActor, -num41);
                                                __instance.ShowBattleState(num33, isActor, 0);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaDefEffect12 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                    if (gongFaDefEffect12)
                                    {
                                        bool flag43 = false;
                                        for (int num42 = 0; num42 < 4; num42++)
                                        {
                                            int num43 = isActor ? __instance.enemyGongFaSp[num3][num42] : __instance.actorGongFaSp[num3][num42];
                                            bool flag44 = num43 > DateFile.instance.GetMaxGongFaSp(num3, num42, false);
                                            if (flag44)
                                            {
                                                flag43 = true;
                                                __instance.ChangeActorSp(!isActor, num42, -5);
                                            }
                                        }
                                        bool flag45 = flag43;
                                        if (flag45)
                                        {
                                            __instance.ShowBattleState(num33, isActor, 0);
                                        }
                                    }
                                }
                            }
                            else if (num34 != 5255)
                            {
                                if (num34 == 5269)
                                {
                                    bool gongFaDefEffect13 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                    if (gongFaDefEffect13)
                                    {
                                        bool flag46 = false;
                                        for (int num44 = 0; num44 < 3; num44++)
                                        {
                                            int num45 = int.Parse(DateFile.instance.GetActorDate(num2, 308 + num44, false));
                                            bool flag47 = num45 > 0 && int.Parse(DateFile.instance.GetItemDate(num45, 901, true, -1)) > 0;
                                            if (flag47)
                                            {
                                                switch (int.Parse(DateFile.instance.GetItemDate(num45, 506, true, -1)))
                                                {
                                                case 0:
                                                    flag46 = true;
                                                    __instance.EquipBreak(num2, isActor, num45, num2, 1, 100);
                                                    __instance.ChangeActorSp(!isActor, 1, -1);
                                                    break;
                                                case 1:
                                                    flag46 = true;
                                                    __instance.EquipBreak(num2, isActor, num45, num2, 1, 100);
                                                    __instance.ChangeActorSp(!isActor, 0, -1);
                                                    break;
                                                case 2:
                                                    flag46 = true;
                                                    __instance.EquipBreak(num2, isActor, num45, num2, 1, 100);
                                                    __instance.ChangeActorSp(!isActor, 3, -1);
                                                    break;
                                                case 3:
                                                    flag46 = true;
                                                    __instance.EquipBreak(num2, isActor, num45, num2, 1, 100);
                                                    __instance.ChangeActorSp(!isActor, 2, -1);
                                                    break;
                                                }
                                            }
                                        }
                                        bool flag48 = flag46;
                                        if (flag48)
                                        {
                                            __instance.ShowBattleState(num33, isActor, 0);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaDefEffect14 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                if (gongFaDefEffect14)
                                {
                                    int totalBlockedAcupoints = BattleSystem.GetTotalBlockedAcupoints(num2);
                                    bool flag49 = totalBlockedAcupoints > 0;
                                    if (flag49)
                                    {
                                        BattleSystem.TransferBlockedAcupointToEnemy(isActor, num2, num3, -1, Mathf.Min(totalBlockedAcupoints, 3));
                                        __instance.ShowBattleState(num33, !isActor, 0);
                                    }
                                }
                            }
                        }
                        else if (num34 <= 5291)
                        {
                            if (num34 != 5284)
                            {
                                if (num34 == 5291)
                                {
                                    bool gongFaDefEffect15 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                    if (gongFaDefEffect15)
                                    {
                                        int num46 = isActor ? __instance.enemyBugSize : __instance.actorBugSize;
                                        bool flag50 = num46 > 0;
                                        if (flag50)
                                        {
                                            int injuryId2 = 55 + Mathf.Min(num46 / 3, 2);
                                            __instance.SetRealDamage(!isActor, BattleSystem.GetDamageType(injuryId2), injuryId2, num46 * 100, num3, BattleSystem.GetDamageTextSize(injuryId2), true, num2, true);
                                            __instance.ShowBattleState(num33, isActor, 0);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaDefEffect16 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                if (gongFaDefEffect16)
                                {
                                    __instance.ChangeActorSp(isActor, 0, 2);
                                }
                            }
                        }
                        else if (num34 != 5313)
                        {
                            if (num34 == 5315)
                            {
                                bool gongFaDefEffect17 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                                if (gongFaDefEffect17)
                                {
                                    __instance.UpdateBattlerMove(!isActor, 90, null, null, false);
                                }
                            }
                        }
                        else
                        {
                            bool gongFaDefEffect18 = __instance.GetGongFaDefEffect(num33, isActor, num2, 0);
                            if (gongFaDefEffect18)
                            {
                                __instance.AddBattleState(num3, 2, 505313, 50 + Mathf.Max(150 - DateFile.instance.GetActorResources(num3)[3] * 5, 0), 1);
                            }
                        }
                    }
                    break;
                }
                }
            }
            return false;
        }
    }
}
