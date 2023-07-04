﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes
{
    internal static class _StreamExtensions
    {
        public static void ReadBytesFrom(this Span<Byte> bytes, System.IO.Stream stream)
        {
            #if NETSTANDARD2_0
            var tmp = new byte[bytes.Length];
            tmp.ReadBytesFrom(stream);
            tmp.AsSpan().CopyTo(bytes);
            #else
            var tmp = bytes;
            while (tmp.Length > 0)
            {
                var r = stream.Read(tmp);
                if (r <= 0) throw new System.IO.EndOfStreamException();
                tmp = tmp.Slice(r);
            }
            #endif
        }

        public static void ReadBytesFrom(this Byte[] bytes, System.IO.Stream stream)
        {
            int c = 0;
            while (c < bytes.Length)
            {
                var r = stream.Read(bytes, c, bytes.Length - c);
                if (r <= 0) throw new System.IO.EndOfStreamException();
                c += r;
            }
        }

        public static void ReadBytesFrom(this Span<Byte> bytes, System.IO.BinaryReader stream)
        {
            #if NETSTANDARD2_0            
            stream.ReadBytes(bytes.Length).AsSpan().CopyTo(bytes);
            #else
            var tmp = bytes;
            while (tmp.Length > 0)
            {
                var r = stream.Read(tmp);
                if (r <= 0) throw new System.IO.EndOfStreamException();
                tmp = tmp.Slice(r);
            }
            #endif
        }

        public static void WriteBytesTo(this ReadOnlySpan<Byte> bytes, System.IO.BinaryWriter stream)
        {
            #if NETSTANDARD2_0
            var tmp = new Byte[bytes.Length];
            bytes.CopyTo(tmp);
            tmp.WriteBytesTo(stream);
            #else
            stream.Write(bytes);
            #endif
        }

        public static void WriteBytesTo(this Byte[] bytes, System.IO.BinaryWriter stream)
        {            
            stream.Write(bytes);            
        }        

        public static void WriteBytesTo(this ReadOnlySpan<Byte> bytes, System.IO.Stream stream)
        {
            #if NETSTANDARD2_0
            var tmp = new Byte[bytes.Length];
            bytes.CopyTo(tmp);
            stream.Write(tmp, 0, tmp.Length);
            #else
            stream.Write(bytes);
            #endif
        }

        public static void WriteBytesTo(this Byte[] bytes, System.IO.Stream stream)
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}