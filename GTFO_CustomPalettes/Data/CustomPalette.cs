using Clonesoft.Json;

namespace CustomPalettes.Data
{
    public class CustomPalette
    {
        public string Name { get; set; } = "Custom Palette";

        public string Author { get; set; } = string.Empty;

        public string SortingName { get; set; } = "MyCustomPalette";

        public uint VanityGroupID { get; set; } = 0;

        public PaletteData Data { get; set; } = new PaletteData();

        [JsonIgnore]
        public string FileName { get; internal set; } = string.Empty;
    }
}
