### InteropTypes.IO.FilePreviews

## Overview

This library can be used to retrieve the operating system's file preview thumbnail.

Current support:

- Windows
  - COM interface: it works on Net8.0 , but it fails to run if compiled in AOT
  - WindowsRT APIs: requires the app to be net8.0-windows10.0.19041.0 to work.


### Thumbs.DB

- [Windows_thumbnail_cache](https://en.wikipedia.org/wiki/Windows_thumbnail_cache)
  
  - [Ole DB](https://www.nuget.org/packages/System.Data.OleDb)
  - [forensics wiki](https://forensics.wiki/thumbs.db/) 
	- OLE is a binary file format developed by Microsoft, which acts like a mini file system within a file.
	  - [ole_compound_file](https://forensics.wiki/ole_compound_file/)	  
	  - [OleCF reader](https://www.nuget.org/packages/OleCf)
  - [forensics thumbs.db](https://forensics.wiki/thumbs.db/)
  - [thumbs.py](https://gist.github.com/peter17/29d2fa106cb59705ae85)
  - [viewer c++](https://github.com/thumbsviewer/thumbsviewer)
  - [vinetto thumbs.db forensics](https://github.com/thinkski/vinetto)
  - [forensics rust 1](https://github.com/berkus/thumbsdb)
  - [forensics rust 2](https://github.com/berkus/thumbsdbkit)

## Windows COM support in AOT

- [IUnknown COM struct doesn't generate a nested interface](https://github.com/microsoft/CsWin32/issues/724)
- [AOT COM Interop requires ComWrapper instance registered for marshalling](https://github.com/dotnet/runtime/issues/115753)
- [COM Wrappers](https://learn.microsoft.com/en-us/dotnet/standard/native-interop/tutorial-comwrappers)
- [CsWin32 is not AOT ready](https://github.com/microsoft/CsWin32/issues/1273)
- [CsWin32 AOT question](https://github.com/microsoft/CsWin32/issues/1444)

	
## android

[media-thumbnails](https://developer.android.com/social-and-messaging/guides/media-thumbnails)
	
## references

- [ThumbnailSharp (outdated and using System.Drawing)](https://github.com/mirzaevolution/ThumbnailSharp)
  