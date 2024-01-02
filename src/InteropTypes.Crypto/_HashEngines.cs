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
        private static Lazy<RandomNumberGenerator> _Randomizer = new Lazy<RandomNumberGenerator>(RandomNumberGenerator.Create);        

        private static Lazy<HashAlgorithm> _Sha512Engine = new Lazy<HashAlgorithm>(SHA512.Create);
        private static Lazy<HashAlgorithm> _Sha384Engine = new Lazy<HashAlgorithm>(SHA384.Create);
        private static Lazy<HashAlgorithm> _Sha256Engine = new Lazy<HashAlgorithm>(SHA256.Create);
        private static Lazy<HashAlgorithm> _Md5Engine = new Lazy<HashAlgorithm>(MD5.Create);

        public static RandomNumberGenerator Randomizer => _Randomizer.Value;
        public static HashAlgorithm Sha512Engine => _Sha512Engine.Value;
        public static HashAlgorithm Sha384Engine => _Sha384Engine.Value;
        public static HashAlgorithm Sha256Engine => _Sha256Engine.Value;
        public static HashAlgorithm Md5Engine => _Md5Engine.Value;
    }
}
