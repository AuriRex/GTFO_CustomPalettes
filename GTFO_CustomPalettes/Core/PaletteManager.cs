﻿using Clonesoft.Json;
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

        private const string TEMPLATE_FILE_NAME = "_template_palette.json";
        public static string BLOCK_PREFIX => $"{EntryPoint.GUID}_{nameof(CustomPalette)}_".ToUpper();

        public static bool DoLoadTemplateFile { get; set; } = false;

        internal static void Setup()
        {
            if (!Directory.Exists(CustomPalettesPath))
            {
                Directory.CreateDirectory(CustomPalettesPath);
            }

            var templateFilePath = Path.Combine(CustomPalettesPath, TEMPLATE_FILE_NAME);
            if (!File.Exists(templateFilePath))
            {
                var template = JsonConvert.SerializeObject(GetTemplate(), _jsonSettings);
                File.WriteAllText(templateFilePath, template);
            }
        }

        internal static void LoadPalettes()
        {
            _palettes.Clear();

            L.Info($"Loading Custom Palettes from [{CustomPalettesPath}]!");

            foreach(var file in Directory.GetFiles(CustomPalettesPath))
            {
                var fileName = Path.GetFileName(file);

                if (!Path.HasExtension(file))
                {
                    continue;
                }

                if (Path.GetExtension(file) != ".json")
                {
                    continue;
                }

                if (fileName == TEMPLATE_FILE_NAME && !DoLoadTemplateFile)
                {
                    continue;
                }

                try
                {
                    var cPal = JsonConvert.DeserializeObject<CustomPalette>(File.ReadAllText(file), _jsonSettings);

                    L.Info($"Loaded Custom Palette: ({cPal.SortingName}): {cPal.Author} | {cPal.Name}");

                    cPal.FileName = fileName;

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
                SortingName = "UsedToSort",
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

        internal static void InjectPalettes(bool forceRegeneration = false)
        {
            L.Info($"Injecting {_palettes.Count} Custom Palettes ...");
            var allPalettes = _palettes.OrderBy(pal => $"{pal.Author}_{pal.SortingName}_{pal.FileName}");


            foreach (var block in VanityItemsTemplateDataBlock.GetAllBlocks())
            {
                if (block.name.StartsWith(BLOCK_PREFIX))
                {
                    block.internalEnabled = false;
                }
            }

            foreach (var cPal in allPalettes)
            {
                try
                {
                    var identifier = BLOCK_PREFIX + cPal.FileName.ToUpper();

                    VanityItemsTemplateDataBlock block;

                    if (VanityItemsTemplateDataBlock.HasBlock(identifier))
                    {
                        block = VanityItemsTemplateDataBlock.GetBlock(identifier);

                        block.prefab = GeneratePrefab(identifier, cPal.Data, forceRegeneration);
                        block.publicName = cPal.Name;
                        block.internalEnabled = true;

                        continue;
                    }

                    block = new();

                    block.name = identifier;
                    block.internalEnabled = true;

                    block.DropWeight = 1;
                    block.type = ClothesType.Palette;
                    block.publicName = cPal.Name;
                    block.prefab = GeneratePrefab(identifier, cPal.Data, forceRegeneration);

                    VanityItemsTemplateDataBlock.AddBlock(block);
                }
                catch(Exception ex)
                {
                    L.Warning($"Failed to load Custom Palette \"{cPal?.Name ?? "Unnamed"}\" ({cPal.FileName}).");
                    L.Exception(ex);
                }
                
            }
        }

        private static string GeneratePrefab(string identifier, PaletteData data, bool forceRegeneration = false)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier may not be null or whitespace.", nameof(identifier));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (AssetShards.AssetShardManager.s_loadedAssetsLookup.ContainsKey(identifier))
            {
                if (!forceRegeneration)
                    return identifier;

                L.Debug($"Regenerating Palette prefab for \"{identifier}\" ...");
                UnityEngine.Object.Destroy(AssetShards.AssetShardManager.s_loadedAssetsLookup[identifier]);
            }

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

            AssetShards.AssetShardManager.s_loadedAssetsLookup[identifier] = go;

            return identifier;
        }
    }
}
