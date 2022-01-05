

### TODO:
- Handle YUV and other non RGB formats
- use System.Runtime.Intrinsics whenever possible  
- use MathF when applicable

### Todo, color spaces:

Color spaces could be supported with:
- Additional components (LinearRed16, LinearGreen16, LinearBlue16, LinearRed32F,LinearGreen32F,LinearBlue32F)
  - components are limited to 255, so we could end up using all of them, fast.
- Adding intra-conversions in the floating point formats: like .ConvertToColorSpace(from,to)
  - The user would be required to track the color format of the pixels.


### Pixel Formats

Unless specified, all formats are in [sRGB](https://en.wikipedia.org/wiki/SRGB) color space.

Unless specified, endianness is Little Endian.

Alpha channel can be defined in two ways:
- 'A' non premultiplied alpha channel.
- 'P' premultiplied alpha channel.

#### 4,5,6 bit Quantized formats (16bit values)

- **BGR565**
- **BGRA5551**
- **BGRA4444**

#### 8 bit Quantized formats, in the 0-255 range (8,24 & 32 bit values)

- **Alpha8** 
- **Luminance8**

- **RGB24**
- **BGR24**
- **BGRA32**
- **BGRP32** (premultiplied RGB)
- **RGBA32**
- **RGBP32** (premultiplied RGB)
- **ARGB32**
- **PRGB32** (premultiplied RGB)

#### 16 bit quantized formats, in the 0-65535 range

- **Luminance16**

#### 32 bit floating point formats, in the range of 0-1

- **Luminance32F**
- **RGB96F**
- **BGR96F**
- **RGBA128F**
- **RGBP128F** (premultiplied RGB)
- **BGRA128F**
- **BGRP128F** (premultiplied RGB)

### Annex

- [RGB Color Spaces](https://en.wikipedia.org/wiki/RGB_color_spaces)
- [Standard RGB](https://en.wikipedia.org/wiki/SRGB)
