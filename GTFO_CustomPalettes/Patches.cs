using AssetShards;
using GameData;
using HarmonyLib;

namespace CustomPalettes
{
    public static class Patches
    {

        [HarmonyPriority(Priority.High)]
        [HarmonyPatch(typeof(AssetShardManager), nameof(AssetShardManager.Setup))]
        internal static class AssetShardManager_Setup_Patch
        {
            public static void Postfix()
            {
                EntryPoint.OnAssetShardManagerReady();
            }
        }

        [HarmonyPriority(Priority.High)]
        [HarmonyPatch(typeof(GameDataInit), nameof(GameDataInit.Initialize))]
        internal static class GameDataInit_Initialize_Patch
        {
            public static void Postfix()
            {
                EntryPoint.OnGameDataInit();
            }
        }

    }
}
