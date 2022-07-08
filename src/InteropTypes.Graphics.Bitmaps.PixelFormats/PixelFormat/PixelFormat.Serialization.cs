using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;

namespace InteropTypes.Graphics.Bitmaps
{
    using PCID = PixelComponentID;
    
    partial struct PixelFormat
    {
        // We cannot store the component numeric value because the enum is subject to change.

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write(this.Component0.Id.ToString());
            w.Write(this.Component1.Id.ToString());
            w.Write(this.Component2.Id.ToString());
            w.Write(this.Component3.Id.ToString());
        }

        public static PixelFormat Read(System.IO.BinaryReader r)
        {
            var c0 = r.ReadString();
            var c1 = r.ReadString();
            var c2 = r.ReadString();
            var c3 = r.ReadString();

            if (!Enum.TryParse<PCID>(c0, true, out var c00)) throw new System.IO.IOException($"Can't parse '{c0}'");
            if (!Enum.TryParse<PCID>(c1, true, out var c01)) throw new System.IO.IOException($"Can't parse '{c1}'");
            if (!Enum.TryParse<PCID>(c2, true, out var c02)) throw new System.IO.IOException($"Can't parse '{c2}'");
            if (!Enum.TryParse<PCID>(c3, true, out var c03)) throw new System.IO.IOException($"Can't parse '{c3}'");

            return new PixelFormat(c00, c01, c02, c03);
        }
        
    }
}
