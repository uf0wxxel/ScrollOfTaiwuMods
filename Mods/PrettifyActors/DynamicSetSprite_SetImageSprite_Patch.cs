using Harmony12;
using UnityEngine.UI;
using UnityModManagerNet;

namespace PrettifyActors
{
    [HarmonyPatch(typeof(DynamicSetSprite), "SetImageSprite", typeof(Image), typeof(string))]
    internal static class DynamicSetSprite_SetImageSprite_Patch
    {
        private static bool Prefix(Image image, string spriteName = "")
        {
            if (!Main.Enabled)
            {
                return true;
            }

            return SharedSpritePatchHelper.Patch(image, spriteName);
        }
    }
}
