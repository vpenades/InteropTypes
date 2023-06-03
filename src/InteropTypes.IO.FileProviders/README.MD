
### Overview

This library contains multiple implementations of `Microsoft.Extensions.FileProviders.Abstractions`

The Physical file system implementation is a strip down of `Microsoft.Extensions.FileProviders.Physical` where:
- The file watch features have been removed
- directory info and directory contents have been merged into a single class.
- added equality support (WIP)


TODO:
- archiveprovider with a static delegate to query for archive passwords
- copy some features of directory commander, like file reflection
- file provider for inbuilt zip archive
- IFileInfo to IFileProvider when the referenced file is an archive
- If IFileInfo represents an archive file, expose a mechanism to open a disposable ArchiveFileProvider
- equality support requires handling case sensitiveness, which can be checked at the provider level.
- file and directory creation should be done at the provider level to allow subclassing.
- On physical, retrieve icon with ShGetFileIcon
- Memory based file system
- registry based file system
- Writer API (maybe `IProgress<MemoryStream>` or `GetService(typeof(Func<Stream>)` )
- Extension for "ReadAllBytes, ReadAllText" ,etc, by possibly using a FileInfoReader(IFileInfo xxx) to prevent extension collisions


### references

- [Microsoft.Extensions.FileProviders.Physical](https://github.com/dotnet/runtime/tree/main/src/libraries/Microsoft.Extensions.FileProviders.Physical)
- [Microsoft.Extensions.FileSystemGlobbing](https://github.com/dotnet/runtime/tree/main/src/libraries/Microsoft.Extensions.FileSystemGlobbing)
- [Image Metadata extractor](https://github.com/drewnoakes/metadata-extractor-dotnet)
- [NTFS Alternate Data Streams](https://es.wikipedia.org/wiki/Alternate_Data_Streams)
  - [How to ADS IO](https://stackoverflow.com/questions/604960/how-to-read-and-modify-ntfs-alternate-data-streams-using-net)
- [ReFS](https://en.wikipedia.org/wiki/ReFS)

### Issues

- [Dotnet System.IO issues](https://github.com/dotnet/runtime/projects/42)

- [File casing](https://github.com/dotnet/runtime/issues/14321)
- [Replace System.IO.File / System.IO.Directory with interfaces](https://github.com/dotnet/runtime/issues/29328)
- [Flesh out FileProvider APIs](https://github.com/dotnet/runtime/issues/82008)
- [IDirectoryContents](https://github.com/dotnet/runtime/issues/86354)
- [cross platform issues](https://github.com/dotnet/runtime/issues/13946#issuecomment-69137144)
	- Unix filenames are a stream of bytes, not necessarily something that can be converted to a valid Unicode string. While uncommon, they do happen, in particular in countries where codepages are still commonly used, or when dealing with old archives from easter european countries that contain filenames that do not decode properly.
	- Directory iteration primitives that decouple the filename from the file name information (getdents + stat vs FindFirst/FindNext idioms). getdents is fast, stat is slow, specially over network shares.
	- Support for symbolic links on the API
	- Support the various Unix open modes (O_SYMLINK, O_NOFOLLOW)
	- Splitting support for Windows and Unix attributes, similar to Java's NIO FileAttribute and FileAttributeViews.
	- Design an ACL system that is Unix friendly, and not tied to Windows.


