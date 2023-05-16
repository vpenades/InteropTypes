using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes
{
    internal static class _HashEngines
    {
        public static RandomNumberGenerator Randomizer { get; } = new Lazy<RandomNumberGenerator>(RandomNumberGenerator.Create).Value;

        public static HashAlgorithm Sha512Engine { get; } = new Lazy<HashAlgorithm>(SHA512.Create).Value;

        public static HashAlgorithm Sha384Engine { get; } = new Lazy<HashAlgorithm>(SHA384.Create).Value;

        public static HashAlgorithm Sha256Engine { get; } = new Lazy<HashAlgorithm>(SHA256.Create).Value;

        public static HashAlgorithm Md5Engine { get; } = new Lazy<HashAlgorithm>(MD5.Create).Value;

    }
}
