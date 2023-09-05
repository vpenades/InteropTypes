using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace InteropTypes.IO
{
    partial class XFile
    {
        public static void WriteAllText(IFileInfo xdir, string fileName, string contents)
        {
            GuardIsValidDirectory(xdir);

            if (TryGetDirectoryInfo(xdir, out var dinfo))
            {
                var path = System.IO.Path.Combine(dinfo.FullName, fileName);
                System.IO.File.WriteAllText(path, contents);                
                return;
            }

            if (xdir is IServiceProvider srv)
            {
                if (srv.GetService(typeof(Func<string,IFileInfo>)) is Func<string, IFileInfo> accessor)
                {
                    var fxinfo = accessor(fileName);
                    if (fxinfo != null)
                    {
                        WriteAllText(fxinfo, contents);
                        return;
                    }
                }
            }

            throw new NotSupportedException(xdir.GetType().FullName);
        }
        public static void WriteAllBytes(IFileInfo xdir, string fileName, Byte[] bytes)
        {
            GuardIsValidDirectory(xdir);

            if (TryGetDirectoryInfo(xdir, out var dinfo))
            {
                var path = System.IO.Path.Combine(dinfo.FullName, fileName);
                System.IO.File.WriteAllBytes(path, bytes);
                return;
            }

            if (xdir is IServiceProvider srv)
            {
                if (srv.GetService(typeof(Func<string, IFileInfo>)) is Func<string, IFileInfo> accessor)
                {
                    var fxinfo = accessor(fileName);
                    if (fxinfo != null)
                    {
                        WriteAllBytes(fxinfo, bytes);
                        return;
                    }
                }
            }

            throw new NotSupportedException(xdir.GetType().FullName);
        }
        
        public static System.IO.Stream Create(IFileInfo xfile)
        {
            GuardIsValidFile(xfile);
            return StreamProvider<IFileInfo>.Default.CreateWriteStreamFrom(xfile);
        }
        
        public static void WriteAllText(IFileInfo xfile, string contents)
        {
            GuardIsValidFile(xfile);
            StreamProvider<IFileInfo>.Default.WriteAllTextTo(xfile, contents);
        }
        public static void WriteAllText(IFileInfo xfile, string contents, Encoding encoding)
        {
            GuardIsValidFile(xfile);
            StreamProvider<IFileInfo>.Default.WriteAllTextTo(xfile, contents, encoding);
        }
        public static string ReadAllText(IFileInfo xfile)
        {
            GuardIsValidFile(xfile);
            return StreamProvider<IFileInfo>.Default.ReadAllTextFrom(xfile);
        }
        public static string ReadAllText(IFileInfo xfile, Encoding encoding)
        {
            GuardIsValidFile(xfile);
            return StreamProvider<IFileInfo>.Default.ReadAllTextFrom(xfile, encoding);
        }
        public static Byte[] ReadAllBytes(IFileInfo xfile)
        {
            GuardIsValidFile(xfile);
            return StreamProvider<IFileInfo>.Default.ReadAllBytesFrom(xfile);
        }        
        public static void WriteAllBytes(IFileInfo xfile, Byte[] bytes)
        {
            GuardIsValidFile(xfile);
            StreamProvider<IFileInfo>.Default.WriteAllBytesTo(xfile, bytes);
        }
    }
}
