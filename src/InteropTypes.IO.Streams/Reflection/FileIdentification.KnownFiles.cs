using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.IO.Reflection
{
    internal abstract partial class FileIdentification
    {
        static FileIdentification() { _Init(_Signatures); }

        private static void _Init(Dictionary<FileIdentification, string> signatures)
        {
            // https://en.wikipedia.org/wiki/List_of_file_signatures

            signatures[CreateFromSignature("425A68")] = "bz2";
            signatures[CreateFromManySignatures("474946383761", "474946383961")] = "gif"; // GIF87a, GIF89a

            signatures[CreateFromManySignatures("49492A00", "4D4D002A")] = "tif"; // little endian, big endian

            signatures[CreateFromSignature("762F3101")] = "exr";

            signatures[CreateFromManySignatures("FFD8FFDB", "FFD8FFE0", "FFD8FFEE", "FFD8FFE000104A4649460001", "FFD8FFE1????457869660000")] = "jpg";

            // https://github.com/link-u/avif-sample-images
            signatures[CreateFromSignature("000000206674797061766966")] = "avif"; // 'ftypavif' this image format is used to disguise JPGs from stock sites.

            // signatures[CreateFromAscii("MZ")] = "exe"; // 'MZ' exe,dll and many other

            signatures[CreateFromManySignatures("504B0304", "504B0506", "504B0708")] = "zip"; // default, empty, spanned

            signatures[CreateFromSignature("526172211A0700")] = "rar"; // ver 1.5
            signatures[CreateFromSignature("526172211A070100")] = "rar"; // ver 5.0

            signatures[CreateFromSignature("89504E470D0A1A0A")] = "png";

            signatures[CreateFromSignature("FFFE")] = "txt";
            signatures[CreateFromSignature("FEFF")] = "txt";
            signatures[CreateFromSignature("EFBBBF")] = "txt";
            signatures[CreateFromSignature("FFFE0000")] = "txt";
            signatures[CreateFromSignature("0000FEFF")] = "txt";

            signatures[CreateFromSignature("2B2F7638")] = "txt";
            signatures[CreateFromSignature("2B2F7639")] = "txt";
            signatures[CreateFromSignature("2B2F762B")] = "txt";
            signatures[CreateFromSignature("2B2F762F")] = "txt";

            signatures[CreateFromSignature("0EFEFF")] = "txt";
            signatures[CreateFromSignature("DD736673")] = "txt";

            signatures[CreateFromSignature("255044462D")] = "pdf";

            signatures[CreateFromSignature("38425053")] = "psd";

            signatures[CreateFromSignature("52494646")] = "wav";

            signatures[CreateFromSignature("FFF2")] = "mp3";
            signatures[CreateFromSignature("FFF3")] = "mp3";
            signatures[CreateFromSignature("FFFB")] = "mp3";

            signatures[CreateFromSignature("424D")] = "bmp";

            signatures[CreateFromSignature("4344303031")] = "iso";

            signatures[CreateFromSignature("1F8B")] = "gz"; // also tar.gz
            signatures[CreateFromSignature("FD377A585A00")] = "xz"; // also tar.xz
            signatures[CreateFromSignature("7573746172003030")] = "tar";
            signatures[CreateFromSignature("7573746172202000")] = "tar";

            signatures[CreateFromSignature("377ABCAF271C")] = "7z";

            signatures[CreateFromSignature("04224D18")] = "lz4";

            signatures[CreateFromSignature("4D534346")] = "cab";

            signatures[CreateFromSignature("233F52414449414E43450A")] = "hdr";

            signatures[CreateFromAscii("<!doctype html")] = "html";
            signatures[CreateFromAscii("<!DOCTYPE html")] = "html";
        }
    }
}
