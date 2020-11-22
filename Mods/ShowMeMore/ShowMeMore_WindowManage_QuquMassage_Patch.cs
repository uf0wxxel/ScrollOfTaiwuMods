using System;
using Harmony12;

namespace ShowMeMore
{
    // Token: 0x02000005 RID: 5
    [HarmonyPatch(typeof(WindowManage), "QuquMassage")]
    public static class ShowMeMore_WindowManage_QuquMassage_Patch
    {
        // Token: 0x0600000A RID: 10 RVA: 0x0000294C File Offset: 0x00000B4C
        private static void Postfix(WindowManage __instance, int ququId, ref string __result)
        {
            bool flag = Main.enabled && Main.settings.ShowQuquMassage;
            if (flag)
            {
                string text = "";
                int ququDate = GetQuquWindow.instance.GetQuquDate(ququId, 31, false, true);
                int ququDate2 = GetQuquWindow.instance.GetQuquDate(ququId, 32, false, true);
                int ququDate3 = GetQuquWindow.instance.GetQuquDate(ququId, 36, false, true);
                int num = ququDate + ququDate3;
                int ququDate4 = GetQuquWindow.instance.GetQuquDate(ququId, 33, false, true);
                int ququDate5 = GetQuquWindow.instance.GetQuquDate(ququId, 34, false, true);
                int ququDate6 = GetQuquWindow.instance.GetQuquDate(ququId, 35, true, true);
                int num2 = 2 * DateFile.instance.GetQuquPrestige(ququId);
                int num3 = int.Parse(DateFile.instance.GetItemDate(ququId, 8, true, -1));
                bool flag2 = ququId % 5 == 0 && num3 < 8;
                text = string.Concat(new string[]
                {
                    text,
                    DateFile.instance.SetColoer(10002, "\n【隐藏属性】\n", false),
                    WindowManage.instance.Dit(),
                    "暴击概率： ",
                    DateFile.instance.SetColoer(20009, ququDate.ToString() + "%", false),
                    " (增加",
                    DateFile.instance.SetColoer(20009, ququDate2.ToString(), false),
                    "点伤害， 击伤概率",
                    DateFile.instance.SetColoer(20010, num.ToString() + "%", false),
                    ")\n",
                    WindowManage.instance.Dit(),
                    "格挡概率： ",
                    DateFile.instance.SetColoer(20005, ququDate4.ToString() + "%", false),
                    "  (减少伤害：",
                    DateFile.instance.SetColoer(20005, ququDate5.ToString(), false),
                    ")\n",
                    WindowManage.instance.Dit(),
                    "反击概率： ",
                    DateFile.instance.SetColoer(20006, ququDate6.ToString() + "%", false),
                    "\n",
                    WindowManage.instance.Dit(),
                    "促织品相： ",
                    DateFile.instance.SetColoer((flag2 || num3 >= 8) ? 20009 : 20002, flag2 ? "神采非凡" : "平平无奇", false),
                    "\n",
                    WindowManage.instance.Dit(),
                    "提供威望： +",
                    DateFile.instance.SetColoer(20007, num2.ToString(), false),
                    " /",
                    DateFile.instance.SetColoer(20008, "「立秋」", false)
                });
                __result += text;
            }
        }
    }
}
