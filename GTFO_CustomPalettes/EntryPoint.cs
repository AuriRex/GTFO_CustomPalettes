using BepInEx;
using BepInEx.Unity.IL2CPP;
using CustomPalettes.Core;
using HarmonyLib;
using System.Reflection;

namespace CustomPalettes
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class EntryPoint : BasePlugin
    {
        public const string GUID = "dev.aurirex.custompalettes";
        public const string NAME = "Custom Palettes";
        public const string VERSION = "0.0.1";

        private Harmony _harmonyInstance;

        public override void Load()
        {
            L.Logger = Log;

            _harmonyInstance = new Harmony(GUID);

            _harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            //PaletteManager.DoLoadTemplateFile = true;
            PaletteManager.LoadPalettes();
        }

        internal static void OnDatablocksReady()
        {
            L.Debug("Datablocks ready, Injecting Palettes");
            PaletteManager.InjectPalettes();
        }
    }
}