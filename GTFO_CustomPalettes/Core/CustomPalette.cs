namespace CustomPalettes.Core
{
    public class CustomPalette
    {
        public string Name { get; set; } = "Custom Palette";

        public string Author { get; set; } = string.Empty;

        public string InternalName { get; set; } = "MyCustomPalette";

        public PaletteData Data { get; set; } = new PaletteData();
    }
}
