

### TODO:
- Handle YUV and other non RGB formats
- use System.Runtime.Intrinsics whenever possible  
- use MathF when applicable

### Todo, color spaces:

Color spaces could be supported with:
- Additional components (LinearRed32F,LinearGreen32F,LinearBlue32F)
  - components are limited to 255, so we could end up using all of them, fast.
- Adding intra-conversions in the floating point formats: like .ConvertToColorSpace(from,to)
  - The user would be required to track the color format of the pixels.


### Pixel Formats

Unless specified, all formats are in [sRGB](https://en.wikipedia.org/wiki/SRGB) color space.

Unless specified, endianness is Little Endian.

Alpha channel can be defined with:
- 'A' non premultiplied alpha channel.
- 'P' premultiplied alpha channel.

8 bit Quantized formats, in the 0-255 range

- **Alpha8** 
- **Luminance8**

- **BGR565** RRRRRGGGGGGBBBBB  
- **BGRA5551** ARRRRRGGGGGBBBBB
- **BGRA4444** AAAARRRRGGGGBBBB
- **RGB24**
- **BGR24**
- **BGRA32**
- **BGRP32**
- **RGBA32**
- **RGBP32**
- **ARGB32**
- **PRGB32**

16 bit quantized formats, in the 0-65535 range

- **Luminance16**

32 bit floating point formats, in the range of 0-1

- **Luminance32F**
- **RGB96F**
- **BGR96F**
- **RGBA128F**
- **RGBP128F**
- **BGRA128F**
- **BGRP128F**

### Annex

- [RGB Color Spaces](https://en.wikipedia.org/wiki/RGB_color_spaces)
- [Standard RGB](https://en.wikipedia.org/wiki/SRGB)
