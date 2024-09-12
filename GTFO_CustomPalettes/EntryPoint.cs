using BepInEx;
using BepInEx.Unity.IL2CPP;
using CustomPalettes.AVUnlock;
using CustomPalettes.Core;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyVersion(CustomPalettes.EntryPoint.VERSION)]
[assembly: AssemblyFileVersion(CustomPalettes.EntryPoint.VERSION)]
[assembly: AssemblyInformationalVersion(CustomPalettes.EntryPoint.VERSION)]

namespace CustomPalettes
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(AllVanity.Plugin.GUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class EntryPoint : BasePlugin
    {
        public const string GUID = "dev.aurirex.gtfo.custompalettes";
        public const string NAME = "Custom Palettes";
        public const string VERSION = "1.1.0";

        private Harmony _harmonyInstance;

        public override void Load()
        {
            L.Logger = Log;

            _harmonyInstance = new Harmony(GUID);

            _harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            if (IL2CPPChainloader.Instance.Plugins.Keys.Any(guid => guid == AllVanity.Plugin.GUID))
            {
                L.Debug($"{nameof(AllVanity)} is installed, registering unlock method.");
                UnlockInterop.Register();
            }

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