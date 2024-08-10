using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CustomPalettes.Core
{
    public class TextureLoader
    {
        private static readonly Dictionary<string, TextureData> _textures = new();

        public static void Setup(IEnumerable<CustomPalette> palletes, bool doCleanup = false)
        {
            foreach(var pal in palletes)
            {
                if (pal == null)
                    continue;

                ProcessPaletteData(pal);
            }

            if(doCleanup)
                CleanupUnusedTextures(palletes);
        }

        private static void CleanupUnusedTextures(IEnumerable<CustomPalette> palletes)
        {
            var potentiallyUnused = new HashSet<string>(_textures.Keys);

            foreach (var pal in palletes)
            {
                var tones = pal?.Data?.Tones;

                if (tones == null)
                    continue;

                foreach(var tone in tones)
                {
                    if (string.IsNullOrWhiteSpace(tone.TextureFile))
                        continue;

                    if(potentiallyUnused.Contains(tone.TextureFile))
                    {
                        potentiallyUnused.Remove(tone.TextureFile);
                    }
                }
            }

            if(potentiallyUnused.Count == 0)
            {
                return;
            }

            L.Debug($"Found {potentiallyUnused.Count} unused Textures.");

            foreach(var unused in potentiallyUnused)
            {
                if(_textures.TryGetValue(unused, out var unusedTexData))
                {
                    UnityEngine.Object.Destroy(unusedTexData.Texture);
                    _textures.Remove(unused);
                    L.Debug($"Unloaded \"{unused}\"!");
                }
            }
        }

        private static void ProcessPaletteData(CustomPalette pal)
        {
            var data = pal.Data;
            if (data == null)
                return;

            foreach(var tone in data.Tones)
            {
                var tex = tone.TextureFile;

                if (string.IsNullOrWhiteSpace(tex))
                    continue;

                if (AttemptLoad(tex, pal))
                {
                    L.Msg($"Loaded Texture \"{tex}\" for Palette {pal.FileName}");
                }
            }
        }

        private static bool AttemptLoad(string tex, CustomPalette pal)
        {
            var fullTexturePath = Path.Combine(PaletteManager.CustomPalettesPath, tex);

            if (!File.Exists(fullTexturePath))
            {
                PrintLoadError("File doesn't exist!", tex, pal);
                return false;
            }

            var lastWrittenTime = File.GetLastWriteTimeUtc(fullTexturePath);

            if (_textures.TryGetValue(tex, out var loadedTexData))
            {
                if(lastWrittenTime == loadedTexData.EditedTime)
                {
                    return true;
                }

                UnityEngine.Object.Destroy(loadedTexData.Texture);

                _textures.Remove(tex);
            }

            var extension = Path.GetExtension(fullTexturePath);
            if (extension.ToLower() != ".png" && extension.ToLower() != ".jpg")
            {
                PrintLoadError("TextureFile is not a valid image file.", tex, pal);
                return false;
            }

            try
            {
                var tex2d = new Texture2D(2, 2);

                var bytes = File.ReadAllBytes(fullTexturePath);

                if (!ImageConversion.LoadImage(tex2d, bytes, false))
                {
                    PrintLoadError("Image data can't be loaded!", tex, pal);
                    UnityEngine.Object.Destroy(tex2d);
                    return false;
                }

                tex2d.hideFlags = HideFlags.HideAndDontSave;
                UnityEngine.Object.DontDestroyOnLoad(tex2d);

                _textures.Add(tex, new TextureData()
                {
                    Texture = tex2d,
                    EditedTime = lastWrittenTime,
                });

                return true;
            }
            catch(Exception ex)
            {
                PrintLoadError(ex.Message, tex, pal);
            }

            return false;
        }

        private static void PrintLoadError( string message, string tex, CustomPalette pal)
        {
            L.Error($"Texture \"{tex}\" could not be loaded for Palette \"{pal?.Name}\" ({pal.FileName}): {message}");
        }

        public static Texture2D GetTexture(string textureFile)
        {
            if (string.IsNullOrWhiteSpace(textureFile))
                return Texture2D.whiteTexture;

            if (_textures.TryGetValue(textureFile, out var texData))
            {
                return texData.Texture;
            }

            return Texture2D.whiteTexture;
        }
    }

    public class TextureData
    {
        public Texture2D Texture { get; internal set; }
        public DateTime EditedTime { get; internal set; }
    }
}
