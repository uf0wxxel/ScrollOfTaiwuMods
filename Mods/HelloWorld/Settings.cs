using UnityModManagerNet;

namespace BossGongfaFixEnhance
{
    public class Settings : UnityModManager.ModSettings
    {
        public bool Enabled { get; set; }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
