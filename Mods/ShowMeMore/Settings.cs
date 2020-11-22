using UnityModManagerNet;

namespace ShowMeMore
{
    // Token: 0x02000003 RID: 3
    public class Settings : UnityModManager.ModSettings
    {
        // Token: 0x06000007 RID: 7 RVA: 0x0000250B File Offset: 0x0000070B
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }

        // Token: 0x04000004 RID: 4
        public bool ShowDebug = false;

        // Token: 0x04000005 RID: 5
        public bool ShowGongFaMassage = true;

        // Token: 0x04000006 RID: 6
        public bool ShowHitRatio = true;

        // Token: 0x04000007 RID: 7
        public bool ShowRealTime = false;

        // Token: 0x04000008 RID: 8
        public bool ShowQiRate = true;

        // Token: 0x04000009 RID: 9
        public bool AddHunYuan = false;

        // Token: 0x0400000A RID: 10
        public bool ShowQuquMassage = true;

        // Token: 0x0400000B RID: 11
        public bool ShowMakingMassage = true;

        // Token: 0x0400000C RID: 12
        public bool ShowItemMassage = true;

        // Token: 0x0400000D RID: 13
        public bool ShowItemFavor = false;

        // Token: 0x0400000E RID: 14
        public bool ShowWeaponMassage = true;

        // Token: 0x0400000F RID: 15
        public bool ShowMoreAttackTimeAtEffect = false;

        // Token: 0x04000010 RID: 16
        public bool ShowHomeMassage = true;

        // Token: 0x04000011 RID: 17
        public bool ShowHomeMassageAtTop = true;
    }
}
