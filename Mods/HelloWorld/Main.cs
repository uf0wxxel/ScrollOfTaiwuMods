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
        static bool Prefix(bool isActor, int damageTyp, int injuryId, int injuryPower, int deferKey, int attackerKey, float damageMassageSize, bool canDefttack = true, bool getWin = true, bool canTransfer = false, bool directDamage = true)
        {
            if (!Main.enabled) return true;
            bool gongFaFEffect = this.GetGongFaFEffect(40004, isActor, deferKey, 0);
            if (gongFaFEffect && isActor)
            {
                bool flag = this.actorGongFaSp[deferKey][0] >= this.enemyGongFaSp[attackerKey][0] * 2 && this.actorGongFaSp[deferKey][1] >= this.enemyGongFaSp[attackerKey][1] * 2 && this.actorGongFaSp[deferKey][2] >= this.enemyGongFaSp[attackerKey][2] * 2 && this.actorGongFaSp[deferKey][3] >= this.enemyGongFaSp[attackerKey][3] * 2;
                if (flag)
                {
                    this.ShowBattleState(40004, isActor, 0);
                    injuryPower = 0;
                    base.StartCoroutine(this.UpdateDamageText(isActor, 0, DateFile.instance.SetColoer(20002, "0", false), this.smallSize, true));
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(BattleSystem), "AutoSetDefGongFa")]
    class BattleSystem_AutoSetDefGongFa_Patch
    {
        static bool Prefix(bool isActor)
        {
            int num2 = this.ActorId(!isActor, false);
            bool gongFaFEffect = this.GetGongFaFEffect(30004, isActor, num, 0);
            if (gongFaFEffect)
            {
                this.ShowBattleState(30004, isActor, 0);
                List<int> list = new List<int>();
                for (int i = 0; i < 4; i++)
                {
                    bool flag = this.enemyGongFaSp[num][i] < this.actorGongFaSp[num2][i] * 2;
                    if (flag)
                    {
                        list.Add(i);
                    }
                }
                this.ChangeActorSp(isActor, (list.Count > 0) ? list[Random.Range(0, list.Count)] : -1, Random.Range(1, 4));
            }
            bool gongFaFEffect2 = this.GetGongFaFEffect(40004, isActor, num, 0);
            if (gongFaFEffect2)
            {
                this.ShowBattleState(40004, isActor, 0);
                List<int> list2 = new List<int>();
                for (int j = 0; j < 4; j++)
                {
                    bool flag2 = this.actorGongFaSp[num][j] < this.enemyGongFaSp[num2][j] * 2;
                    if (flag2)
                    {
                        list2.Add(j);
                    }
                }
                this.ChangeActorSp(isActor, (list2.Count > 0) ? list2[Random.Range(0, list2.Count)] : -1, Random.Range(1, 4));
            }
            int num3 = this.NowDefEffectId(isActor, num);
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
                            bool gongFaDefEffect = this.GetGongFaDefEffect(num3, isActor, num, 0);
                            if (gongFaDefEffect)
                            {
                                int num5 = isActor ? this.actorGongFaSp[num][0] : this.enemyGongFaSp[num][0];
                                this.SetPoisonDamage(!isActor, num2, 3, Random.Range(10, 40) + num5 * 3, true);
                            }
                        }
                    }
                    else
                    {
                        bool gongFaDefEffect2 = this.GetGongFaDefEffect(num3, isActor, num, 0);
                        if (gongFaDefEffect2)
                        {
                            this.DoHeal(isActor, 50, false, true, -1);
                        }
                    }
                }
                else if (num4 != 5278)
                {
                    if (num4 == 5318)
                    {
                        bool gongFaDefEffect3 = this.GetGongFaDefEffect(num3, isActor, num, 0);
                        if (gongFaDefEffect3)
                        {
                            int num6 = isActor ? this.actorGongFaSp[num][0] : this.enemyGongFaSp[num][0];
                            this.SetPoisonDamage(!isActor, num2, 1, Random.Range(10, 40) + num6 * 3, true);
                        }
                    }
                }
                else
                {
                    bool gongFaDefEffect4 = this.GetGongFaDefEffect(num3, isActor, num, 0);
                    if (gongFaDefEffect4)
                    {
                        List<int> list3 = new List<int>(DateFile.instance.battleActorsInjurys[num2].Keys);
                        foreach (int injuryId in list3)
                        {
                            this.WorsenInjury(num2, injuryId, 50, false, true);
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
        static bool Prefix(bool isActor, int gongFaId, bool noPrepare = false)
        {
            this.StopGongFaTime(true);
            int num = 0;
            int num2 = this.ActorId(isActor, false);
            int num3 = this.ActorId(!isActor, true);
            int num4 = int.Parse(DateFile.instance.gongFaDate[gongFaId][2]);
            int combatSkillDataInt = DateFile.GetCombatSkillDataInt(gongFaId, 7, num2, true);
            if (noPrepare)
            {
                bool flag = !(isActor ? this._actorNextImmediateGongfaNotShowName : this._enemyNextImmediateGongfaNotShowName);
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
                        this._actorNextImmediateGongfaNotShowName = false;
                    }
                    else
                    {
                        this._enemyNextImmediateGongfaNotShowName = false;
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
                this.SetRealDamage(isActor, 1, injuryId, num6, num2, (num5 == 2) ? this.mediumSize : ((num5 == 3) ? this.largeSize : this.smallSize), false, num2, false);
                this.ShowBattleState(10009, isActor, 0);
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
                    this.ShowBattleState(10010, isActor, 0);
                }
                DateFile.instance.ChangeMianQi(num2, num, 5);
            }
            bool flag10 = this.battleEnd;
            if (!flag10)
            {
                bool isActor3 = isActor;
                if (isActor3)
                {
                    bool flag11 = num4 > this.actorUseGongFaMaxLevel;
                    if (flag11)
                    {
                        this.actorUseGongFaMaxLevel = num4;
                    }
                }
                else
                {
                    bool flag12 = num4 > this.enemyUseGongFaMaxLevel;
                    if (flag12)
                    {
                        this.enemyUseGongFaMaxLevel = num4;
                    }
                }
                bool gongFaFEffect = this.GetGongFaFEffect(5235, isActor, num2, 0);
                if (gongFaFEffect)
                {
                    List<int> list2 = new List<int>();
                    for (int j = 0; j < 7; j++)
                    {
                        bool flag13 = this.battlerQiDamagePart[num2][j] > BattleSystem.GetNotHealBlockedAcupointCount(num2, j);
                        if (flag13)
                        {
                            list2.Add(j);
                        }
                    }
                    bool flag14 = list2.Count > 0;
                    if (flag14)
                    {
                        this.ShowBattleState(5235, isActor, 0);
                        this.ChangeQiDamagePart(isActor, num2, num2, list2[Random.Range(0, list2.Count)], -3, true, true);
                    }
                }
                switch (int.Parse(DateFile.instance.gongFaDate[gongFaId][6]))
                {
                case 1:
                {
                    this.useGongFaMask.CrossFadeAlpha(0.6f, 1f, true);
                    bool flag15 = !isActor && this.maxBattleDefAttack > 0;
                    if (flag15)
                    {
                        int num10 = (this.battleDefAttack + 1 == this.maxBattleDefAttack) ? 9 : this.battleDefAttack;
                        this.ShowBattleState(10201 + num10, false, 0);
                    }
                    int num11 = this.NowDefEffectId(!isActor, num3);
                    bool flag16 = num11 != 0;
                    if (flag16)
                    {
                        int num12 = num11;
                        if (num12 != 287)
                        {
                            if (num12 == 5287)
                            {
                                bool gongFaDefEffect = this.GetGongFaDefEffect(num11, !isActor, num3, 0);
                                if (gongFaDefEffect)
                                {
                                    this.AddBattleState(num3, 1, 505287, 100 + DateFile.instance.GetActorValue(num3, 514, true) / 5, 1);
                                }
                            }
                        }
                        else
                        {
                            bool gongFaDefEffect2 = this.GetGongFaDefEffect(num11, !isActor, num3, 0);
                            if (gongFaDefEffect2)
                            {
                                this.ChangeActorSp(!isActor, -1, Mathf.Max(DateFile.instance.GetActorValue(num3, 514, true) / 50, 1));
                            }
                        }
                    }
                    bool gongFaFEffect2 = this.GetGongFaFEffect(276, isActor, num2, 0);
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
                            this.ShowBattleState(276, isActor, 0);
                            this.SetPoisonEffect(!isActor, num3, list3[Random.Range(0, list3.Count)], 0, true, -1);
                        }
                    }
                    bool flag19 = int.Parse(DateFile.instance.GetActorDate(num2, 15, true)) > int.Parse(DateFile.instance.GetActorDate(num3, 15, true)) && this.GetGongFaFEffect(261, isActor, num2, 0);
                    if (flag19)
                    {
                        int combatSkillDataInt2 = DateFile.GetCombatSkillDataInt(gongFaId, 7, num2, true);
                        this.SetRealDamage(!isActor, 1, 45 + combatSkillDataInt2, Random.Range(150, 301), num3, (combatSkillDataInt2 == 2) ? this.mediumSize : ((combatSkillDataInt2 == 3) ? this.largeSize : this.smallSize), false, num2, true);
                    }
                    bool flag20 = int.Parse(DateFile.instance.GetActorDate(num3, 15, true)) > int.Parse(DateFile.instance.GetActorDate(num2, 15, true)) && this.GetGongFaFEffect(5261, !isActor, num3, 0);
                    if (flag20)
                    {
                        int combatSkillDataInt3 = DateFile.GetCombatSkillDataInt(gongFaId, 7, num2, true);
                        this.SetRealDamage(isActor, 1, 45 + combatSkillDataInt3, Random.Range(150, 301), num2, (combatSkillDataInt3 == 2) ? this.mediumSize : ((combatSkillDataInt3 == 3) ? this.largeSize : this.smallSize), false, num3, true);
                    }
                    bool gongFaFEffect3 = this.GetGongFaFEffect(30010, isActor, num2, 0);
                    if (gongFaFEffect3)
                    {
                        int count = DateFile.instance.GetLifeDateList(num2, 801, false).Count;
                        int count2 = DateFile.instance.GetLifeDateList(num3, 801, false).Count;
                        int num13 = count - count2;
                        bool flag21 = num13 > 0;
                        if (flag21)
                        {
                            this.ShowBattleState(30010, isActor, 0);
                            for (int l = 0; l < num13; l++)
                            {
                                int num14 = (Random.Range(0, 100) < 50) ? 0 : 1;
                                this.SetRealDamage(!isActor, num14, (num14 == 0) ? 57 : 60, Random.Range(300, 600), num3, this.largeSize, false, num2, true);
                            }
                        }
                    }
                    bool gongFaFEffect4 = this.GetGongFaFEffect(40010, isActor, num2, 0);
                    if (gongFaFEffect4)
                    {
                        int count3 = DateFile.instance.GetLifeDateList(num2, 801, false).Count;
                        int count4 = DateFile.instance.GetLifeDateList(num3, 801, false).Count;
                        int num15 = count3 - count4;
                        bool flag22 = num15 > 0;
                        if (flag22)
                        {
                            this.ShowBattleState(30010, isActor, 0);
                            for (int m = 0; m < num15; m++)
                            {
                                int num16 = (Random.Range(0, 100) < 50) ? 0 : 1;
                                this.SetRealDamage(!isActor, num16, (num16 == 0) ? 57 : 60, Random.Range(150, 300), num3, this.largeSize, false, num2, true);
                            }
                        }
                    }
                    bool flag23 = this.battleEnd;
                    if (!flag23)
                    {
                        bool flag24 = DateFile.instance.HaveLifeDate(num3, 501);
                        if (flag24)
                        {
                            int num17 = Mathf.Clamp(DateFile.instance.actorLife[num3][501][0], 0, 200);
                            bool flag25 = num17 > 100 && this.GetGongFaFEffect(20006, isActor, num2, 0);
                            if (flag25)
                            {
                                int num18 = num17 * num17 / 80;
                                this.SetPoisonDamage(!isActor, num3, Random.Range(0, 6), Random.Range(num18, num18 * 2 + 1), true);
                            }
                        }
                        bool gongFaFEffect5 = this.GetGongFaFEffect(5276, isActor, num2, 0);
                        if (gongFaFEffect5)
                        {
                            for (int n = 0; n < 6; n++)
                            {
                                int num19 = DateFile.instance.battleActorsPoisons[num2][n];
                                this.SetPoisonDamage(!isActor, num3, n, num19 * 20 / 100, true);
                                this.SetPoisonDamage(isActor, num2, n, -(num19 * 20 / 100), true);
                            }
                        }
                        bool gongFaFEffect6 = this.GetGongFaFEffect(20009, isActor, num2, 0);
                        if (gongFaFEffect6)
                        {
                            List<int> list4 = new List<int>();
                            for (int num20 = 0; num20 < 4; num20++)
                            {
                                bool flag26 = isActor ? (this.enemyGongFaSp[num3][num20] > 0) : (this.actorGongFaSp[num3][num20] > 0);
                                if (flag26)
                                {
                                    list4.Add(num20);
                                }
                            }
                            int num21 = (list4.Count > 0) ? list4[Random.Range(0, list4.Count)] : Random.Range(0, 4);
                            int num22 = Mathf.Min(3, isActor ? this.enemyGongFaSp[num3][num21] : this.actorGongFaSp[num3][num21]);
                            this.ChangeActorSp(isActor, num21, num22);
                            this.ChangeActorSp(!isActor, num21, -num22);
                        }
                        bool flag27 = this.enemyTeamId == 4 & isActor;
                        if (flag27)
                        {
                            this.TimePause(0f, true, 0f);
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
                            int num23 = isActor ? this.actorLegGongFaMoveTyp : this.enemyLegGongFaMoveTyp;
                            int num24 = num23;
                            if (num24 <= 460)
                            {
                                if (num24 != 430)
                                {
                                    if (num24 == 460)
                                    {
                                        bool gongFaMoveEffect = this.GetGongFaMoveEffect(num23, isActor, num2, 0);
                                        if (gongFaMoveEffect)
                                        {
                                            this.ShowBattleState(num23, isActor, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect2 = this.GetGongFaMoveEffect(num23, isActor, num2, 0);
                                    if (gongFaMoveEffect2)
                                    {
                                        this.ShowBattleState(num23, isActor, 0);
                                    }
                                }
                            }
                            else if (num24 != 490)
                            {
                                if (num24 != 5430)
                                {
                                    if (num24 == 5490)
                                    {
                                        bool gongFaMoveEffect3 = this.GetGongFaMoveEffect(num23, isActor, num2, 0);
                                        if (gongFaMoveEffect3)
                                        {
                                            this.ShowBattleState(num23, isActor, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect4 = this.GetGongFaMoveEffect(num23, isActor, num2, 0);
                                    if (gongFaMoveEffect4)
                                    {
                                        this.ShowBattleState(num23, isActor, 0);
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaMoveEffect5 = this.GetGongFaMoveEffect(num23, isActor, num2, 0);
                                if (gongFaMoveEffect5)
                                {
                                    this.ShowBattleState(num23, isActor, 0);
                                }
                            }
                        }
                        SingletonObject.getInstance<YieldHelper>().DelayFrameDo(1U, delegate
                        {
                            this.ShowGongFaDef(isActor, gongFaId);
                        });
                    }
                    break;
                }
                case 2:
                {
                    this.SetMoveGongFa(isActor, gongFaId);
                    int num25 = this.NowMoveEffectId(isActor, num2);
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
                                            bool gongFaMoveEffect6 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                            if (gongFaMoveEffect6)
                                            {
                                                this.ChangeMoveCost(isActor, -1, 25 + Mathf.Min((isActor ? this.actorActionCost.Count : this.enemyActionCost.Count) * 10, 50), false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool gongFaMoveEffect7 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect7)
                                        {
                                            this.ChangeActorSp(isActor, 1, 1);
                                        }
                                    }
                                }
                                else if (num26 != 428)
                                {
                                    switch (num26)
                                    {
                                    case 436:
                                    {
                                        bool gongFaMoveEffect8 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect8)
                                        {
                                            int num27 = isActor ? this.actorGongFaSp[num2][0] : this.enemyGongFaSp[num2][0];
                                            this.ChangeActorSp(isActor, 0, -num27);
                                            this.ChangeActorSp(isActor, 1, num27);
                                        }
                                        break;
                                    }
                                    case 437:
                                    {
                                        bool gongFaMoveEffect9 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect9)
                                        {
                                            int num28 = 6000;
                                            this.UpdateStrength(isActor, (float)num28, false);
                                            this.UpdateStrength(!isActor, (float)(-(float)num28), false);
                                        }
                                        break;
                                    }
                                    case 438:
                                    case 439:
                                        break;
                                    case 440:
                                    {
                                        bool gongFaMoveEffect10 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect10)
                                        {
                                            this.ShowBattleState(num25, isActor, 0);
                                        }
                                        break;
                                    }
                                    default:
                                        if (num26 == 448)
                                        {
                                            bool gongFaMoveEffect11 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                            if (gongFaMoveEffect11)
                                            {
                                                this.ShowBattleState(num25, isActor, 0);
                                            }
                                        }
                                        break;
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect12 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect12)
                                    {
                                        this.UpdateMagic(isActor, 30000f, false);
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
                                            bool gongFaMoveEffect13 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                            if (gongFaMoveEffect13)
                                            {
                                                this.ShowBattleState(num25, isActor, 0);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool gongFaMoveEffect14 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect14)
                                        {
                                            this.ShowBattleState(num25, isActor, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect15 = this.GetGongFaMoveEffect(num25, isActor, num2, int.Parse(DateFile.instance.GetActorDate(num2, 15, true)) / 10);
                                    if (gongFaMoveEffect15)
                                    {
                                        this.ChangeActorSp(isActor, 1, 2);
                                    }
                                }
                            }
                            else if (num26 != 463)
                            {
                                if (num26 != 481)
                                {
                                    if (num26 == 493)
                                    {
                                        bool gongFaMoveEffect16 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect16)
                                        {
                                            this.AddBattleState(num3, 2, 500493, 100, 1);
                                        }
                                    }
                                }
                                else
                                {
                                    int num29 = isActor ? this.actorGongFaSp[num2][1] : this.enemyGongFaSp[num2][1];
                                    bool flag30 = num29 >= 9 && this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (flag30)
                                    {
                                        this.ChangeActorSp(isActor, 1, -9);
                                        this.UpdateRangeText(90, isActor);
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaMoveEffect17 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                if (gongFaMoveEffect17)
                                {
                                    this.ChangeMoveCost(!isActor, 1, -50, true);
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
                                            bool gongFaMoveEffect18 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                            if (gongFaMoveEffect18)
                                            {
                                                this.UpdateStrength(isActor, 30000f, false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool gongFaMoveEffect19 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect19)
                                        {
                                            this.ChangeMoveCost(isActor, -1, 75 - Mathf.Min((isActor ? this.actorActionCost.Count : this.enemyActionCost.Count) * 10, 50), false);
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect20 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect20)
                                    {
                                        this.ChangeActorSp(!isActor, 1, -1);
                                    }
                                }
                            }
                            else if (num26 != 5429)
                            {
                                switch (num26)
                                {
                                case 5436:
                                {
                                    bool gongFaMoveEffect21 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect21)
                                    {
                                        int num30 = isActor ? this.actorGongFaSp[num2][2] : this.enemyGongFaSp[num2][2];
                                        this.ChangeActorSp(isActor, 2, -num30);
                                        this.ChangeActorSp(isActor, 1, num30);
                                    }
                                    break;
                                }
                                case 5437:
                                {
                                    bool gongFaMoveEffect22 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect22)
                                    {
                                        int num31 = 6000;
                                        this.UpdateMagic(isActor, (float)num31, false);
                                        this.UpdateMagic(!isActor, (float)(-(float)num31), false);
                                    }
                                    break;
                                }
                                case 5438:
                                case 5439:
                                    break;
                                case 5440:
                                {
                                    bool gongFaMoveEffect23 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect23)
                                    {
                                        this.ShowBattleState(num25, isActor, 0);
                                    }
                                    break;
                                }
                                default:
                                    if (num26 == 5448)
                                    {
                                        bool gongFaMoveEffect24 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect24)
                                        {
                                            this.ShowBattleState(num25, isActor, 0);
                                        }
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                bool gongFaMoveEffect25 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                if (gongFaMoveEffect25)
                                {
                                    this.RemoveRandWeaponCd(isActor);
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
                                        bool gongFaMoveEffect26 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                        if (gongFaMoveEffect26)
                                        {
                                            this.ShowBattleState(num25, isActor, 0);
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaMoveEffect27 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect27)
                                    {
                                        this.ShowBattleState(num25, isActor, 0);
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaMoveEffect28 = this.GetGongFaMoveEffect(num25, isActor, num2, int.Parse(DateFile.instance.GetActorDate(num2, 15, true)) / 10);
                                if (gongFaMoveEffect28)
                                {
                                    this.ChangeActorSp(!isActor, 1, -2);
                                }
                            }
                        }
                        else if (num26 != 5463)
                        {
                            if (num26 != 5481)
                            {
                                if (num26 == 5493)
                                {
                                    bool gongFaMoveEffect29 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                    if (gongFaMoveEffect29)
                                    {
                                        this.AddBattleState(num3, 2, 505493, 100, 1);
                                    }
                                }
                            }
                            else
                            {
                                int num32 = isActor ? this.actorGongFaSp[num2][1] : this.enemyGongFaSp[num2][1];
                                bool flag31 = num32 >= 9 && this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                                if (flag31)
                                {
                                    this.ChangeActorSp(isActor, 1, -9);
                                    this.UpdateRangeText(20, isActor);
                                }
                            }
                        }
                        else
                        {
                            bool gongFaMoveEffect30 = this.GetGongFaMoveEffect(num25, isActor, num2, 0);
                            if (gongFaMoveEffect30)
                            {
                                this.RemoveMoveCost(!isActor, 0, true, false, false);
                            }
                        }
                    }
                    break;
                }
                case 3:
                {
                    this.SetDefGongFa(isActor, gongFaId);
                    int num33 = this.NowDefEffectId(isActor, num2);
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
                                            bool gongFaDefEffect3 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                            if (gongFaDefEffect3)
                                            {
                                                int num35 = this.battlerDpValue[num2][0];
                                                bool flag33 = num35 > 0;
                                                if (flag33)
                                                {
                                                    this.ChangeActorSp(isActor, 0, num35 * 100 / Mathf.Max(this.battlerDpValue[num2][1], 1) / 10);
                                                    BattleSystem.AddDefence(num2, isActor, -num35);
                                                    this.ShowBattleState(num33, isActor, 0);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool gongFaDefEffect4 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                        if (gongFaDefEffect4)
                                        {
                                            bool flag34 = false;
                                            for (int num36 = 0; num36 < 4; num36++)
                                            {
                                                int num37 = isActor ? this.actorGongFaSp[num2][num36] : this.enemyGongFaSp[num2][num36];
                                                bool flag35 = num37 < DateFile.instance.GetMaxGongFaSp(num2, num36, false);
                                                if (flag35)
                                                {
                                                    flag34 = true;
                                                    this.ChangeActorSp(isActor, num36, 5);
                                                }
                                            }
                                            bool flag36 = flag34;
                                            if (flag36)
                                            {
                                                this.ShowBattleState(num33, isActor, 0);
                                            }
                                        }
                                    }
                                }
                                else if (num34 != 255)
                                {
                                    if (num34 == 269)
                                    {
                                        bool gongFaDefEffect5 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
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
                                                        this.EquipBreak(num2, isActor, num39, num2, 1, 100);
                                                        this.ChangeActorSp(isActor, 1, 1);
                                                        break;
                                                    case 1:
                                                        flag37 = true;
                                                        this.EquipBreak(num2, isActor, num39, num2, 1, 100);
                                                        this.ChangeActorSp(isActor, 0, 1);
                                                        break;
                                                    case 2:
                                                        flag37 = true;
                                                        this.EquipBreak(num2, isActor, num39, num2, 1, 100);
                                                        this.ChangeActorSp(isActor, 3, 1);
                                                        break;
                                                    case 3:
                                                        flag37 = true;
                                                        this.EquipBreak(num2, isActor, num39, num2, 1, 100);
                                                        this.ChangeActorSp(isActor, 2, 1);
                                                        break;
                                                    }
                                                }
                                            }
                                            bool flag39 = flag37;
                                            if (flag39)
                                            {
                                                this.ShowBattleState(num33, isActor, 0);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaDefEffect6 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                    if (gongFaDefEffect6)
                                    {
                                        int totalFlaws = BattleSystem.GetTotalFlaws(num2);
                                        bool flag40 = totalFlaws > 0;
                                        if (flag40)
                                        {
                                            BattleSystem.TransferFlawToEnemy(isActor, num2, num3, -1, Mathf.Min(totalFlaws, 3));
                                            this.ShowBattleState(num33, !isActor, 0);
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
                                        bool gongFaDefEffect7 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                        if (gongFaDefEffect7)
                                        {
                                            int num40 = isActor ? this.enemyBugSize : this.actorBugSize;
                                            bool flag41 = num40 > 0;
                                            if (flag41)
                                            {
                                                this.ChangeBugSize(!isActor, -3);
                                                this.ShowBattleState(num33, isActor, 0);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaDefEffect8 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                    if (gongFaDefEffect8)
                                    {
                                        this.ChangeActorSp(isActor, 2, 2);
                                    }
                                }
                            }
                            else if (num34 != 313)
                            {
                                if (num34 == 315)
                                {
                                    bool gongFaDefEffect9 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                    if (gongFaDefEffect9)
                                    {
                                        this.UpdateBattlerMove(!isActor, 20, null, null, false);
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaDefEffect10 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                if (gongFaDefEffect10)
                                {
                                    this.AddBattleState(num3, 2, 500313, 50 + Mathf.Max(150 - DateFile.instance.GetActorResources(num3)[3] * 5, 0), 1);
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
                                        bool gongFaDefEffect11 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                        if (gongFaDefEffect11)
                                        {
                                            int num41 = this.battlerDpValue[num2][0];
                                            bool flag42 = num41 > 0;
                                            if (flag42)
                                            {
                                                this.ChangeActorSp(isActor, 2, num41 * 100 / Mathf.Max(this.battlerDpValue[num2][1], 1) / 10);
                                                BattleSystem.AddDefence(num2, isActor, -num41);
                                                this.ShowBattleState(num33, isActor, 0);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool gongFaDefEffect12 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                    if (gongFaDefEffect12)
                                    {
                                        bool flag43 = false;
                                        for (int num42 = 0; num42 < 4; num42++)
                                        {
                                            int num43 = isActor ? this.enemyGongFaSp[num3][num42] : this.actorGongFaSp[num3][num42];
                                            bool flag44 = num43 > DateFile.instance.GetMaxGongFaSp(num3, num42, false);
                                            if (flag44)
                                            {
                                                flag43 = true;
                                                this.ChangeActorSp(!isActor, num42, -5);
                                            }
                                        }
                                        bool flag45 = flag43;
                                        if (flag45)
                                        {
                                            this.ShowBattleState(num33, isActor, 0);
                                        }
                                    }
                                }
                            }
                            else if (num34 != 5255)
                            {
                                if (num34 == 5269)
                                {
                                    bool gongFaDefEffect13 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
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
                                                    this.EquipBreak(num2, isActor, num45, num2, 1, 100);
                                                    this.ChangeActorSp(!isActor, 1, -1);
                                                    break;
                                                case 1:
                                                    flag46 = true;
                                                    this.EquipBreak(num2, isActor, num45, num2, 1, 100);
                                                    this.ChangeActorSp(!isActor, 0, -1);
                                                    break;
                                                case 2:
                                                    flag46 = true;
                                                    this.EquipBreak(num2, isActor, num45, num2, 1, 100);
                                                    this.ChangeActorSp(!isActor, 3, -1);
                                                    break;
                                                case 3:
                                                    flag46 = true;
                                                    this.EquipBreak(num2, isActor, num45, num2, 1, 100);
                                                    this.ChangeActorSp(!isActor, 2, -1);
                                                    break;
                                                }
                                            }
                                        }
                                        bool flag48 = flag46;
                                        if (flag48)
                                        {
                                            this.ShowBattleState(num33, isActor, 0);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaDefEffect14 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                if (gongFaDefEffect14)
                                {
                                    int totalBlockedAcupoints = BattleSystem.GetTotalBlockedAcupoints(num2);
                                    bool flag49 = totalBlockedAcupoints > 0;
                                    if (flag49)
                                    {
                                        BattleSystem.TransferBlockedAcupointToEnemy(isActor, num2, num3, -1, Mathf.Min(totalBlockedAcupoints, 3));
                                        this.ShowBattleState(num33, !isActor, 0);
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
                                    bool gongFaDefEffect15 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                    if (gongFaDefEffect15)
                                    {
                                        int num46 = isActor ? this.enemyBugSize : this.actorBugSize;
                                        bool flag50 = num46 > 0;
                                        if (flag50)
                                        {
                                            int injuryId2 = 55 + Mathf.Min(num46 / 3, 2);
                                            this.SetRealDamage(!isActor, BattleSystem.GetDamageType(injuryId2), injuryId2, num46 * 100, num3, BattleSystem.GetDamageTextSize(injuryId2), true, num2, true);
                                            this.ShowBattleState(num33, isActor, 0);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bool gongFaDefEffect16 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                if (gongFaDefEffect16)
                                {
                                    this.ChangeActorSp(isActor, 0, 2);
                                }
                            }
                        }
                        else if (num34 != 5313)
                        {
                            if (num34 == 5315)
                            {
                                bool gongFaDefEffect17 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                                if (gongFaDefEffect17)
                                {
                                    this.UpdateBattlerMove(!isActor, 90, null, null, false);
                                }
                            }
                        }
                        else
                        {
                            bool gongFaDefEffect18 = this.GetGongFaDefEffect(num33, isActor, num2, 0);
                            if (gongFaDefEffect18)
                            {
                                this.AddBattleState(num3, 2, 505313, 50 + Mathf.Max(150 - DateFile.instance.GetActorResources(num3)[3] * 5, 0), 1);
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
