using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.FileProviders;

using BYTESSEGMENT = System.ArraySegment<byte>;

namespace InteropTypes.IO
{
    partial class XFile
    {
        public static System.IO.Stream CreateWriteStream(IFileInfo xfile)
        {
            GuardIsValidFile(xfile);
            return StreamProvider<IFileInfo>.Default.CreateWriteStreamFrom(xfile);
        }


        public static void WriteAllText(IFileInfo xdir, string fileName, string contents, Encoding encoding = null)
        {
            GuardIsValidDirectory(xdir);            

            if (TryGetDirectoryInfo(xdir, out var dinfo)) // try a direct approach
            {
                var path = System.IO.Path.Combine(dinfo.FullName, fileName);
                if (encoding == null) System.IO.File.WriteAllText(path, contents);
                else System.IO.File.WriteAllText(path, contents, encoding);
                return;
            }

            if (xdir is IServiceProvider srv) // try with a service provider
            {
                if (srv.GetService(typeof(Func<string,IFileInfo>)) is Func<string, IFileInfo> accessor)
                {
                    var fxinfo = accessor(fileName);
                    if (fxinfo != null)
                    {
                        WriteAllText(fxinfo, contents, encoding);
                        return;
                    }
                }
            }

            throw new NotSupportedException(xdir.GetType().FullName);
        }                              
        public static void WriteAllText(IFileInfo xfile, string contents, Encoding encoding = null)
        {
            GuardIsValidFile(xfile);
            StreamProvider<IFileInfo>.Default.WriteAllTextTo(xfile, contents, encoding);
        }        
        public static string ReadAllText(IFileInfo xfile, Encoding encoding = null)
        {
            GuardIsValidFile(xfile);
            return StreamProvider<IFileInfo>.Default.ReadAllTextFrom(xfile, encoding);
        }

              
        
        public static void WriteAllBytes(IFileInfo xdir, string fileName, IReadOnlyList<Byte> bytes)
        {
            GuardIsValidDirectory(xdir);

            if (TryGetDirectoryInfo(xdir, out var dinfo)) // try a direct approach
            {
                FilePathUtils.HasInvalidNameChars(fileName);

                var path = System.IO.Path.Combine(dinfo.FullName, fileName);
                WriteAllBytes(new System.IO.FileInfo(path), bytes);
                return;
            }

            if (xdir is IServiceProvider srv) // try with a service provider
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
        public static void WriteAllBytes(IFileInfo xfile, IReadOnlyList<Byte> bytes)
        {
            GuardIsValidFile(xfile);
            StreamProvider<IFileInfo>.Default.WriteAllBytesTo(xfile, bytes);
        }        
        public static BYTESSEGMENT ReadAllBytes(IFileInfo xfile)
        {
            GuardIsValidFile(xfile);
            return StreamProvider<IFileInfo>.Default.ReadAllBytesFrom(xfile);
        }



        public static void WriteAllBytes(FileInfo finfo, IReadOnlyList<Byte> bytes)
        {
            GuardIsValidFile(finfo);

            switch (bytes)
            {
                case Byte[] array:
                    System.IO.File.WriteAllBytes(finfo.FullName, array);
                    break;

                default:
                    using (var s = finfo.OpenWrite()) { XStream.WriteAllBytes(s, bytes); }
                    break;
            }
        }
    }
}
