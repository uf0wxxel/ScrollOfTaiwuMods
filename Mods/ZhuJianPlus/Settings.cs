using UnityModManagerNet;

namespace ZhuJianPlus
{
    // Token: 0x02000003 RID: 3
    public class Settings : UnityModManager.ModSettings
    {
        // Token: 0x06000007 RID: 7 RVA: 0x0000228D File Offset: 0x0000048D
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }

        // Token: 0x04000005 RID: 5
        public bool notEnchant = false;

        // Token: 0x04000006 RID: 6
        public int extraEnchant = 0;

        // Token: 0x04000007 RID: 7
        public int maxEnchantTimes = 10;
    }
}
