using System.Reflection;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using CustomPalettes;
//using CustomPalettes.AVUnlock;
using CustomPalettes.Core;
using HarmonyLib;

[assembly: AssemblyVersion(Plugin.VERSION)]
[assembly: AssemblyFileVersion(Plugin.VERSION)]
[assembly: AssemblyInformationalVersion(Plugin.VERSION)]

namespace CustomPalettes;

[BepInPlugin(GUID, MOD_NAME, VERSION)]
public class Plugin : BasePlugin
{
    public const string GUID = "dev.aurirex.gtfo.custompalettes";
    public const string MOD_NAME = "Custom Palettes";
    public const string VERSION = ManifestInfo.TSVersion;

    private static readonly Harmony _harmony = new(GUID);

    public override void Load()
    {
        L.Logger = Log;

        _harmony.PatchAll(Assembly.GetExecutingAssembly());

        PaletteManager.Setup();
        PaletteManager.LoadPalettes();
    }

    internal static void OnAssetShardManagerReady()
    {
        L.Debug("AssetShardManager ready, Injecting Palettes");
        TextureLoader.Setup(PaletteManager.Palettes);
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
            TextureLoader.Setup(PaletteManager.Palettes, doCleanup: true);
            PaletteManager.InjectPalettes(forceRegeneration: true);
            PersistentInventoryManager.m_dirty = true; // Refreshes inventory
        }

        _hasInitedOnce = true;
    }
}