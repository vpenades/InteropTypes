using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.FileProviders;
using NUnit.Framework;

namespace InteropTypes.IO
{
    internal static class Utils
    {
        public static void _PrintContents(this IDirectoryContents contents, int indent = 0)
        {
            foreach (var entry in contents.Where(item => item.IsDirectory))
            {
                Indent(indent); TestContext.WriteLine($"📁 {entry.Name}");

                if (entry is IDirectoryContents child)
                {
                    _PrintContents(child, indent + 1);
                }
            }

            foreach (var entry in contents.Where(item => !item.IsDirectory))
            {
                var h256 = Crypto.Hash256.Sha256FromFile(entry);

                Indent(indent); TestContext.WriteLine($"🗎 {entry.Name} => {h256.ToHexString()}");

                if (entry is IServiceProvider srv)
                {
                    if (srv.GetService(typeof(JsonDocument)) is JsonDocument ppp)
                    {
                        Indent(indent + 2); TestContext.WriteLine(ppp.RootElement);                        
                    }
                }
            }
        }

        private static void Indent(int indent)
        {
            for (int i = 0; i < indent; i++) TestContext.Write("  ");
        }
    }
}
