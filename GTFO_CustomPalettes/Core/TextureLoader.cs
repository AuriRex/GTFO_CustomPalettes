using UnityEngine;

namespace CustomPalettes.Core
{
    internal class TextureLoader
    {
        public static Texture2D GetTexture(string textureFile)
        {
            if(string.IsNullOrWhiteSpace(textureFile))
                return Texture2D.whiteTexture;

            return Texture2D.whiteTexture;
        }
    }
}
