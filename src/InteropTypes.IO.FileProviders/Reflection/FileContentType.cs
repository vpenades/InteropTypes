using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO.Reflection
{
    public enum FileContentType
    {
        Unknown,

        Archive,
        Document,
        Image,
        Audio,
        Video,
        Runtime,
        SourceCode,
        MetaData,

        Garbage,
        Dangerous,
        UserAttention        
    }

    static class FileContentTypeExtensions
    {
        public static string GetUltraShortDesc(this FileContentType ft)
        {
            if (Environment.OSVersion.Version <= new Version(8, 0))
            {
                switch (ft)
                {
                    case FileContentType.Archive: return "Arc";
                    case FileContentType.Document: return "Doc";
                    case FileContentType.Image: return "Img";
                    case FileContentType.Audio: return "Aud";
                    case FileContentType.Video: return "Vid";
                    case FileContentType.Runtime: return "Run";
                    case FileContentType.SourceCode: return "Prog";

                    case FileContentType.Garbage: return "Garbage";
                    case FileContentType.Dangerous: return "Danger";
                }

                return "?";
            }
            else
            {
                switch (ft)
                {
                    case FileContentType.Archive: return "🗜";
                    case FileContentType.Document: return "💼";
                    case FileContentType.Image: return "🖼";
                    case FileContentType.Audio: return "🎵";
                    case FileContentType.Video: return "🎞";
                    case FileContentType.Runtime: return "⚙";
                    case FileContentType.SourceCode: return "💾";

                    case FileContentType.Garbage: return "🗑";
                    case FileContentType.Dangerous: return "☠";
                }

                return "❔";
            }
        }

        public static string GetFileTypeFormat(this FileContentType ft)
        {
            return GetUltraShortDesc(ft) + "{0}";
        }

        public static FileContentType IdentifyContentType(this IFileInfo finfo)
        {
            return IdentifyContentTypeFromName(finfo.Name);
        }

        public static FileContentType IdentifyContentTypeFromName(string name)
        {
            name = System.IO.Path.GetFileName(name);
            name = name.ToLower();

            // specific file names

            if (FileContentInfo.MetadataFiles.Contains(name)) return FileContentType.MetaData;
            if (FileContentInfo.GarbageFiles.Contains(name)) return FileContentType.Garbage;
            if (FileContentInfo.DangerousFiles.Contains(name)) return FileContentType.Dangerous;

            switch (name)
            {
                // case ArchiveManifest.FILENAMELOWERCASE: return FileContentType.Garbage;                
                case "rarbg_do_not_mirror.exe": return FileContentType.Dangerous;
                case "desktop.ini": return FileContentType.Runtime; // it depends on the context (the location)                
            }

            // composited extensions

            if (name.EndsWith(".$db$.json")) return FileContentType.MetaData;
            if (name.EndsWith(".tar.gz")) return FileContentType.Archive;

            // regular extensions

            var extension = System.IO.Path.GetExtension(name);
            var ft = IdentifyContentTypeFromExtension(extension);

            // any executable or script containing the "mirror character" must be considered dangerous
            // https://stackoverflow.com/questions/3115204/unicode-mirror-character
            
            if (ft == FileContentType.Runtime && name.Contains('\u202e')) ft = FileContentType.Dangerous;            

            return ft;
        }

        public static FileContentType IdentifyContentTypeFromExtension(this string extension)
        {
            // https://www.howtogeek.com/137270/50-file-extensions-that-are-potentially-dangerous-on-windows/

            extension = extension.ToLower();

            switch (extension)
            {
                case ".pcx":
                case ".tga":
                case ".bmp":
                case ".ico":
                case ".gif":
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".tif":
                case ".tiff":
                case ".webp":
                case ".dds":
                case ".svg":
                case ".psd":
                case ".kra":
                    return FileContentType.Image;


                case ".avi":
                case ".asf":
                case ".flv":
                case ".mpg":
                case ".mp4":
                case ".wmv":
                case ".m4v":
                case ".mkv":
                case ".mjpg":
                case ".webm":
                case ".3gp":
                case ".3gpp":
                    return FileContentType.Video;


                case ".wav":
                case ".wma":
                case ".mp3":
                case ".flac":
                case ".midi":
                    return FileContentType.Audio;


                case ".exe":
                case ".dll":
                case ".com":
                case ".cmd":
                case ".bat":

                case ".ps": // .PS1, .PS1XML, .PS2, .PS2XML, .PSC1, .PSC2 – A Windows PowerShell script. Runs PowerShell commands in the order specified in the file.
                case ".ps1":
                case ".ps2":
                case ".ps1xml":
                case ".ps2xml":
                case ".psc1":
                case ".psc2":

                case ".inf": // .INF – A text file used by AutoRun. If run, this file could potentially launch dangerous applications it came with or pass dangerous options to programs included with Windows.

                case ".reg": // .REG – A Windows registry file. .REG files contain a list of registry entries that will be added or removed if you run them. A malicious .REG file could remove important information from your registry, replace it with junk data, or add malicious data.


                case ".scf": // .SCF – A Windows Explorer command file. Could pass potentially dangerous commands to Windows Explorer.
                case ".lnk": // .LNK – A link to a program on your computer. A link file could potentially contain command-line attributes that do dangerous things, such as deleting files without asking.
                case ".url": // equivalent to .LNK

                case ".msi": // .MSI – A Microsoft installer file. These install other applications on your computer, although applications can also be installed by .exe files.
                case ".msp": // .MSP – A Windows installer patch file. Used to patch applications deployed with .MSI files.
                case ".application": // .APPLICATION – An application installer deployed with Microsoft’s ClickOnce technology.
                case ".jar": // .JAR – .JAR files contain executable Java code. If you have the Java runtime installed, .JAR files will be run as programs.
                    return FileContentType.Runtime;


                case ".md":
                case ".txt":
                case ".nfo":
                case ".pdf":
                case ".doc":
                case ".docx":
                case ".xls":
                case ".xlsx":
                case ".ppt":
                case ".odf":
                    return FileContentType.Document;


                case ".docm": // .DOCM, .DOTM, .XLSM, .XLTM, .XLAM, .PPTM, .POTM, .PPAM, .PPSM, .SLDM – New file extensions introduced in Office 2007. The M at the end of the file extension indicates that the document contains Macros. For example, a .DOCX file contains no macros, while a .DOCM file can contain macros.
                case ".dotm":
                case ".xlsm":
                case ".xltm":
                case ".xlam":
                case ".pptm":
                case ".potm":
                case ".ppam":
                case ".ppsm":
                case ".sldm":
                    return FileContentType.Document; // DangerousDocument ??


                case ".zip":
                case ".rar":
                case ".cbz":
                case ".cbr":
                case ".cb7":
                case ".7z":
                case ".tar":
                case ".gz":
                case ".lz4":
                case ".lzh":
                case ".cab":
                    return FileContentType.Archive;


                case ".c":
                case ".h":
                case ".cpp":
                case ".cxx":
                case ".cs":
                case ".js":
                case ".jse": // .JSE – An encrypted JavaScript file.
                case ".java":
                case ".py":
                case ".vb": // visual basic script
                    return FileContentType.SourceCode;



                case ".vbs": // visual basic script
                case ".vbe": // .VBE – An encrypted VBScript file. Similar to a VBScript file, but it’s not easy to tell what the file will actually do if you run it.

                case ".scr": // screen saver

                case ".pif": // .PIF – A program information file for MS-DOS programs. While .PIF files aren’t supposed to contain executable code, Windows will treat .PIFs the same as .EXE files if they contain executable code.
                case ".gadget": // .GADGET – A gadget file for the Windows desktop gadget technology introduced in Windows Vista.

                case ".hta": // .HTA – An HTML application. Unlike HTML applications run in browsers, .HTA files are run as trusted applications without sandboxing.
                case ".cpl": // .CPL – A Control Panel file. All of the utilities found in the Windows Control Panel are .CPL files.
                case ".msc": // .MSC – A Microsoft Management Console file. Applications such as the group policy editor and disk management tool are .MSC files.
                case ".ws": // .WS, .WSF – A Windows Script file.
                case ".wsf": // .WS, .WSF – A Windows Script file.
                case ".wsc": // .WSC, .WSH – Windows Script Component and Windows Script Host control files.Used along with with Windows Script files.
                case ".wsh": // .WSC, .WSH – Windows Script Component and Windows Script Host control files.Used along with with Windows Script files.

                case ".msh": // .MSH, .MSH1, .MSH2, .MSHXML, .MSH1XML, .MSH2XML – A Monad script file. Monad was later renamed PowerShell.
                case ".msh1":
                case ".msh2":
                case ".mshxml":
                case ".msh1xml":
                case ".msh2xml":
                    return FileContentType.Dangerous;
            }

            return FileContentType.Unknown;
        }
    }
}
