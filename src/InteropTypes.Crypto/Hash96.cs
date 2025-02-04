﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Crypto
{
    partial struct Hash96
    {
        // https://en.wikipedia.org/wiki/Jenkins_hash_function

        public static Hash96 FromFile(Microsoft.Extensions.FileProviders.IFileInfo finfo, string jsonProperty = null)
        {
            if (!string.IsNullOrWhiteSpace(jsonProperty) && finfo is IServiceProvider srv)
            {
                if (TryGetFromService(srv, jsonProperty, out var r)) return r;
            }

            if (finfo is ISource src)
            {
                var h = src.GetHash96Code();
                if (!h.IsZero) return h;
            }

            return default;
        }
    }
}
