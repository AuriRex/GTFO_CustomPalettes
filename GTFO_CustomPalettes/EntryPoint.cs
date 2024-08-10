using BepInEx;
using BepInEx.Unity.IL2CPP;
using CustomPalettes.Core;
using HarmonyLib;
using System.Reflection;

[assembly: AssemblyVersion(CustomPalettes.EntryPoint.VERSION)]
[assembly: AssemblyFileVersion(CustomPalettes.EntryPoint.VERSION)]
[assembly: AssemblyInformationalVersion(CustomPalettes.EntryPoint.VERSION)]

namespace CustomPalettes
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class EntryPoint : BasePlugin
    {
        public const string GUID = "dev.aurirex.gtfo.custompalettes";
        public const string NAME = "Custom Palettes";
        public const string VERSION = "1.0.0";

        private Harmony _harmonyInstance;

        public override void Load()
        {
            L.Logger = Log;

            _harmonyInstance = new Harmony(GUID);

            _harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            PaletteManager.Setup();
            PaletteManager.LoadPalettes();
        }

        internal static void OnAssetShardManagerReady()
        {
            L.Debug("AssetShardManager ready, Injecting Palettes");
            TextureLoader.Setup(PaletteManager.Palletes);
            PaletteManager.InjectPalettes();
        }

        private static bool _hasInitedOnce = false;
        internal static void OnGameDataInit()
        {
            L.Debug("GameDataInit.Initialize called.");

            if (_hasInitedOnce)
            {
                // Most likely MTFO Hot-Reload
                L.Warning("Reloading Custom Palettes ...");
                PaletteManager.LoadPalettes();
                TextureLoader.Setup(PaletteManager.Palletes, doCleanup: true);
                PaletteManager.InjectPalettes(forceRegeneration: true);
                PersistentInventoryManager.m_dirty = true; // Refreshes inventory
            }

            _hasInitedOnce = true;
        }
    }
}