
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
- include a mechanism to query for additional information (hashes, metadata, etc)
