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
        public static void _PrintContents(this IDirectoryContents contents)
        {
            var entries = LinkedFileInfo
                .Enumerate(contents, System.IO.SearchOption.AllDirectories)
                .ToList();

            // Equality check
            var hashed = entries.Distinct(FileInfoComparer.OrdinalIgnoreCase).ToList();
            Assert.That(hashed.Count, Is.EqualTo(entries.Count));

            foreach (var entry in entries)
            {
                if (entry.IsDirectory)
                {
                    Indent(entry.Depth); TestContext.WriteLine($"📁 {entry.Name}");
                }

                else
                {
                    var h256 = Crypto.Hash256.Sha256FromFile(entry);
                    Indent(entry.Depth); TestContext.WriteLine($"🗎 {entry.Name} => {h256.ToHexString()}");

                    if (entry is IServiceProvider srv)
                    {
                        if (srv.GetService(typeof(JsonDocument)) is JsonDocument ppp)
                        {
                            Indent(entry.Depth + 2); TestContext.WriteLine(ppp.RootElement);
                        }
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
