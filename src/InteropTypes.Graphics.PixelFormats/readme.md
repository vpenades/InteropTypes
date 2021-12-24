

### TODO:
- Handle YUV and other non RGB formats
- Handle Linear RGB (LinearRed32F,LinearGreen32F,LinearBlue32F) so we can have LinearRGBP
- use System.Runtime.Intrinsics whenever possible

### Descriptions:

Pixel formats are generally in:
- Little endian
- sRGB color space.


Alpha channel can be defined with:
- 'A' non premultiplied alpha channel.
- 'P' premultiplied alpha channel.


### Pixel Formats

- **Alpha8**  
- **Luminance8**  
- **Luminance16**  
- **Luminance32F**  
- **BGR565** sRGB color space RRRRRGGGGGGBBBBB  
- **BGRA5551** sRGB color space ARRRRRGGGGGBBBBB
- **BGRA4444** sRGB color space AAAARRRRGGGGBBBB
- **RGB24**
- **BGR24**
- **BGRA32**
- **BGRP32**
- **RGBA32**
- **RGBP32**
- **ARGB32**
- **PRGB32**
- **RGB96F**
- **BGR96F**
- **RGBA128F**
- **RGBP128F**
- **BGRA128F**
- **BGRP128F**


### Annex

- [RGB Color Spaces](https://en.wikipedia.org/wiki/RGB_color_spaces)
- [Standard RGB](https://en.wikipedia.org/wiki/SRGB)
