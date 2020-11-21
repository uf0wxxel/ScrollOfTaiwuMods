using UnityModManagerNet;

namespace HelloWorld
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
