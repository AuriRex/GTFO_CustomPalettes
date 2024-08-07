using AssetShards;
using HarmonyLib;

namespace CustomPalettes
{
    public static class Patches
    {

        [HarmonyPriority(Priority.High)]
        [HarmonyPatch(typeof(AssetShardManager), "Setup")]
        internal static class AssetShardManager_Setup_Patch
        {
            public static void Postfix()
            {
                EntryPoint.OnDatablocksReady();
            }
        }

    }
}
