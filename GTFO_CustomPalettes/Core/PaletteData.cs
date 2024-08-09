using Clonesoft.Json;
using UnityEngine;

namespace CustomPalettes.Core
{
    public class PaletteData
    {
        public Tone PrimaryTone { get; set; } = new Tone();

        public Tone SecondaryTone { get; set; } = new Tone();

        public Tone TertiaryTone { get; set; } = new Tone();

        public Tone QuaternaryTone { get; set; } = new Tone();

        public Tone QuinaryTone { get; set; } = new Tone();

        public float TextureTiling { get; set; } = 1f;

        public class Tone
        {
            public string HexColor { get; set; } = "#FFFFFF";

            public string TextureFile { get; set; } = string.Empty;

            public int MaterialOverride { get; set; } = -1;
        }


        [JsonIgnore]
        private static readonly ClothesPalette.Tone _errorTone = new()
        {
            m_color = Color.magenta,
            m_texture = Texture2D.whiteTexture,
            m_materialOverride = -1,
        };

        internal ClothesPalette.Tone GetTone(int v)
        {
            switch (v)
            {
                default:
                case 1:
                    return GetTone(PrimaryTone);
                case 2:
                    return GetTone(SecondaryTone);
                case 3:
                    return GetTone(TertiaryTone);
                case 4:
                    return GetTone(QuaternaryTone);
                case 5:
                    return GetTone(QuinaryTone);
            }
        }

        private static ClothesPalette.Tone GetTone(Tone tone)
        {
            if (tone == null)
                return _errorTone;

            if (!ColorUtility.TryParseHtmlString(tone.HexColor, out var col))
            {
                if (!ColorUtility.TryParseHtmlString($"#{tone.HexColor}", out col))
                    return _errorTone;
            }

            Texture2D texture = TextureLoader.GetTexture(tone.TextureFile);

            return new ClothesPalette.Tone
            {
                m_color = col,
                m_texture = texture,
                m_materialOverride = tone.MaterialOverride,
            };
        }
    }
}
