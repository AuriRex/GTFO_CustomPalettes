using Clonesoft.Json;
using GameData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CustomPalettes.Core
{
    public class PaletteManager
    {
        private static string _path;
        public static string CustomPalettesPath => _path ??= Path.Combine(BepInEx.Paths.BepInExRootPath, "Assets", "CustomPalettes");

        private static readonly List<CustomPalette> _palettes = new();


        private static readonly JsonSerializerSettings _jsonSettings = new()
        {
            Formatting = Formatting.Indented
        };

        private const string templateFileName = "_template_palette.json";
        public static bool DoLoadTemplateFile { get; set; } = false;

        internal static void LoadPalettes()
        {
            
            var path = Path.Combine(BepInEx.Paths.BepInExRootPath, "Assets", "CustomPalettes");

            L.Info($"Loading Custom Palettes from [{path}]!");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var templateFilePath = Path.Combine(path, templateFileName);
            if (!File.Exists(templateFilePath))
            {
                var template = JsonConvert.SerializeObject(GetTemplate(), _jsonSettings);
                File.WriteAllText(templateFilePath, template);
            }

            foreach(var file in Directory.GetFiles(path))
            {
                var fileName = Path.GetFileName(file);

                if (fileName == templateFileName && !DoLoadTemplateFile)
                {
                    continue;
                }

                try
                {
                    var cPal = JsonConvert.DeserializeObject<CustomPalette>(File.ReadAllText(file), _jsonSettings);

                    L.Info($"Loaded Custom Palette: ({cPal.InternalName}): {cPal.Author} | {cPal.Name}");

                    _palettes.Add(cPal);
                }
                catch(Exception ex)
                {
                    L.Exception(ex);
                }
            }
        }

        private static CustomPalette GetTemplate()
        {
            return new CustomPalette()
            {
                Author = "YourNameHere",
                Name = "Display Name Ingame",
                InternalName = "ANameHere",
                Data = new()
                {
                    PrimaryTone = new()
                    {
                        HexColor = "#000000"
                    },
                    SecondaryTone = new()
                    {
                        HexColor = "#44006F"
                    },
                    TertiaryTone = new()
                    {
                        HexColor = "#2c0000"
                    },
                    QuaternaryTone = new()
                    {
                        HexColor = "#332300"
                    },
                    QuinaryTone = new()
                    {
                        HexColor = "#410073"
                    }
                }
            };
        }

        internal static void InjectPalettes()
        {
            L.Info($"Injecting {_palettes.Count} Custom Palettes ...");
            var allPalettes = _palettes.OrderBy(i => $"{i.Author}_{i.InternalName}");

            foreach (var cPal in allPalettes)
            {
                var block = new VanityItemsTemplateDataBlock();

                try
                {
                    block.name = cPal.InternalName;
                    block.internalEnabled = true;

                    block.DropWeight = 1;
                    block.type = ClothesType.Palette;
                    block.publicName = cPal.Name;
                    block.prefab = GeneratePrefab(cPal);

                    VanityItemsTemplateDataBlock.AddBlock(block);
                }
                catch(Exception ex)
                {
                    L.Warning($"Failed to load Custom Palette \"{cPal?.InternalName ?? "Unnamed"}\".");
                    L.Exception(ex);
                }
                
            }
        }

        private static string GeneratePrefab(CustomPalette cPal)
        {
            var identifier = $"{cPal.Author}_{cPal.InternalName}".ToUpper();// cPal.Author + "_" + cPal.InternalName;

            if(AssetShards.AssetShardManager.s_loadedAssetsLookup.ContainsKey(identifier))
            {
                return identifier;
            }

            var data = cPal.Data ?? throw new InvalidDataException($"{nameof(CustomPalette)}.{nameof(CustomPalette.Data)} can't be null!");
            
            var go = new GameObject(identifier);

            go.hideFlags = HideFlags.HideAndDontSave | HideFlags.DontUnloadUnusedAsset;
            GameObject.DontDestroyOnLoad(go);

            var palette = go.AddComponent<ClothesPalette>();

            palette.m_textureTiling = data.TextureTiling;
            palette.m_primaryTone = data.GetTone(1);
            palette.m_secondaryTone = data.GetTone(2);
            palette.m_tertiaryTone = data.GetTone(3);
            palette.m_quaternaryTone = data.GetTone(4);
            palette.m_quinaryTone = data.GetTone(5);

            AssetShards.AssetShardManager.s_loadedAssetsLookup.Add(identifier, go);

            return identifier;
        }
    }
}
