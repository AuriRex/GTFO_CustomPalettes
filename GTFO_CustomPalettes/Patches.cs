using AssetShards;
using GameData;
using HarmonyLib;

namespace CustomPalettes
{
    public static class Patches
    {

        [HarmonyPatch(typeof(AssetShardManager), nameof(AssetShardManager.Setup))]
        internal static class AssetShardManager_Setup_Patch
        {
            [HarmonyPriority(Priority.High)]
            public static void Postfix()
            {
                EntryPoint.OnAssetShardManagerReady();
            }
        }

        [HarmonyPatch(typeof(GameDataInit), nameof(GameDataInit.Initialize))]
        internal static class GameDataInit_Initialize_Patch
        {
            [HarmonyPriority(Priority.High)]
            public static void Postfix()
            {
                EntryPoint.OnGameDataInit();
            }
        }

    }
}
