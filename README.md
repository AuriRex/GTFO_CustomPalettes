# Custom Palettes

A GTFO mod that lets you add custom color palettes using json files, no Unity or DataBlock fiddling required!

Custom Palettes go into the `BepInEx/Assets/CustomPalettes/` folder (created upon starting the game once with the mod installed).  
Additionally a template json file will be generated inside of that folder.
Make sure to copy and rename this template to something else, the template won't be loaded itself.

MTFO Hot-Reloading is supported for palettes **and** texture files, just place them in the folder and hit reload.

## **Either [AllVanity](https://thunderstore.io/c/gtfo/p/AuriRex/AllVanity/) or [DeviousLick](https://thunderstore.io/c/gtfo/p/Frog/DeviousLick/) is required for the Palettes to show up in game!**

### [custom rundown developers look here](#for-rundown-devs)

## Custom Palettes json file

Textures and colors are applied *multiplicative*, that means if you want your image texture to properly show make sure to set your color to white / `#FFF`

If you set your color to green (`#00FF00`) and choose a rainbow texture `examples/rainbow.png`, then only the parts that are green are gonna show up in game, which results in a stripped pattern alternating green and black.

Also because of this you can treat textures like patterns, where white equals full color and black no color.  
Check the `examples` folder in this repo to see a few different patterns.

### Things to be aware of

To avoid conflicts with other palettes I'd recommend making sure your custom palette json files have a unique name;  
Maybe include your name, something like `AuriRex_MyCoolPalette1.json` should do the trick.

The same thing goes for textures, I'd recommend creating a folder with the same name as your json file and putting all your textures in there.  
Don't forget to include the folder in your `TextureFile` field: `AuriRex_MyCoolPalette1/custom_texture1.png`.

### Tone Reference

<p align="center">
  <img src="https://raw.githubusercontent.com/AuriRex/GTFO_CustomPalettes/main/palette_tone_ref.png" alt="Tone Reference"/>
</p>

### Fields
* `Name`: The name displayed in game in the palettes selection menu.
* `Author`: The palette author, your name most likely (not shown in game atm).
* `SortingName`: A string used in the palette sorting process, you can change this to move your palette up and down (alphabetically sorted)
* `VanityGroupID`: Allows rundown developers to automatically insert this palette into a specific group.
  This property is ignored if a block with the identifier of this CustomPalette already exists.  
  (Note: Leave this set to `0` for palettes that aren't tied to your own rundown.)
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
      (Check out the [Material Reference Sheet](https://github.com/AuriRex/GTFO_CustomPalettes/blob/main/img/material_ref/README.md)!!)
## A few example palettes:
### Colors only
```json
{
  "Name": "My Cool Palette",
  "Author": "AuriRex",
  "SortingName": "SomethingHere",
  "VanityGroupID": 0,
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
    "TextureTiling": 1
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
    "TextureTiling": 1
  }
}
```

## For rundown devs:

CustomPalettes checks for already existing `VanityItemsTemplateDataBlock`s with the `name` field set to the custom palettes' identifier.  
Identifiers look like this: `CUSTOMPALETTE_FILE_NAME_ALL_UPPERCASE.JSON` and are printed into the BepInEx console on startup!

With `SimpleProgression` installed, you're able to drop custom palettes via all vanilla means.

So by specifying an 'empty' block like in the example below, you're able to ensure a specific palette always ends up with the specified persistentID.  
`VanityItemsTemplateDataBlock`:
```json
{
  "publicName": "",
  "type": 4,
  "prefab": "",
  "DropWeight": 1.1,
  "icon": "",
  "name": "CUSTOMPALETTE_FILE_NAME_ALL_UPPERCASE.JSON",
  "internalEnabled": true,
  "persistentID": 170
}
```
This way you're able to use the ID in other blocks like:
* `VanityItemsGroupDataBlock`: Used to define 'drop groups' for expeditions ...  
  or
* `VanityItemsLayerDropsDataBlock`: Uses the same groups to drop on x layers completed

