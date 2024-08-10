# GTFO_CustomPalettes

A GTFO mod that lets you add custom color palettes using json files, no Unity or DataBlock fiddling required!

Custom Plaettes go into the `BepInEx/Assets/CustomPalettes/` folder (created upon starting the game once with the mod installed).  
Additionally a template json file will be generated inside of that folder.
Make sure to copy and rename this template to something else, the template won't be loaded itself.

## Custom Palettes json file

Textures and colors are applied *multiplicative*, that means if you want your image texture to properly show make sure to set your color to white / `#FFF`

If you set your color to green (`#00FF00`) and choose a rainbow texture `examples/rainbow.png`, then only the parts that are green are gonna show up in game, which results in a stripped pattern alternating green and black.

### Fields
* `Name`: The name displayed in game in the palettes selection menu.
* `Author`: The palette author, your name most likely (not shown in game atm).
* `SortingName`: A string used in the palette sorting process, you can change this to move your palette up and down (alphabetically sorted)
* `Data`: Contains all the important things:
  * `TextureTiling`: How often the texture should tile. (Bigger number = smaller pattern)
  * `<?>Tone`: The five different parts of a palette
    * `HexColor`: A HEX string denoting this tones color.  
    Supported Formats:  
      * #RRGGBB
      * #RRGGBBAA
      * #RGB
      * #RGBA 
      * The literal strings: `red`, `cyan`, `blue`, `darkblue`, `lightblue`, `purple`, `yellow`, `lime`, `fuchsia`, `white`, `silver`, `grey`, `black`, `orange`, `brown`, `maroon`, `green`, `olive`, `navy`, `teal`, `aqua`, `magenta`.
    * `TextureFile`: The (relative) path to an image file (.png or .jpg)
    * `MaterialOverride`: Used to override the material used on this tone, leave as -1 if unsure.
## A few example palettes:
### Colors only
```json
{
  "Name": "My Cool Palette",
  "Author": "AuriRex",
  "SortingName": "SomethingHere",
  "Data": {
    "PrimaryTone": {
      "HexColor": "#FF0000",
      "TextureFile": "",
      "MaterialOverride": -1
    },
    "SecondaryTone": {
      "HexColor": "#007700",
      "TextureFile": "",
      "MaterialOverride": 1
    },
    "TertiaryTone": {
      "HexColor": "#1c0000",
      "TextureFile": "",
      "MaterialOverride": -1
    },
    "QuaternaryTone": {
      "HexColor": "#010101",
      "TextureFile": "",
      "MaterialOverride": -1
    },
    "QuinaryTone": {
      "HexColor": "#810093",
      "TextureFile": "",
      "MaterialOverride": 2
    },
    "TextureTiling": 20
  }
}
```
### With Images
In this example, the `icon.png` file has been placed into the folder `AuriRex_MyCoolPalette`, next to our palette json file.  
```json
{
  "Name": "My Cool Textured Palette",
  "Author": "AuriRex",
  "SortingName": "SomethingHere",
  "Data": {
    "PrimaryTone": {
      "HexColor": "#FFFFFF",
      "TextureFile": "AuriRex_MyCoolPalette/icon.png",
      "MaterialOverride": -1
    },
    "SecondaryTone": {
      "HexColor": "#007700",
      "TextureFile": "AuriRex_MyCoolPalette/icon.png",
      "MaterialOverride": 1
    },
    "TertiaryTone": {
      "HexColor": "#1c0000",
      "TextureFile": "AuriRex_MyCoolPalette/icon.png",
      "MaterialOverride": -1
    },
    "QuaternaryTone": {
      "HexColor": "#013101",
      "TextureFile": "AuriRex_MyCoolPalette/icon.png",
      "MaterialOverride": -1
    },
    "QuinaryTone": {
      "HexColor": "#310093",
      "TextureFile": "AuriRex_MyCoolPalette/icon.png",
      "MaterialOverride": 2
    },
    "TextureTiling": 20
  }
}
```