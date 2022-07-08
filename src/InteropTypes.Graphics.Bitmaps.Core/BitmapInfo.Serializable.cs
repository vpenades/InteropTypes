using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{    
    partial struct BitmapInfo
    {
        public static BitmapInfo Read(System.IO.BinaryReader reader)
        {
            var w = reader.ReadInt32();
            var h = reader.ReadInt32();
            var p = reader.ReadInt32();
            var s = reader.ReadInt32();
            var f = Bitmaps.PixelFormat.Read(reader);

            if (p != f.ByteCount) throw new System.IO.IOException("Pixel byte size mismatch");

            return new BitmapInfo(w, h, f, s);            
        }

        public void Write(System.IO.BinaryWriter writer)
        {
            writer.Write(this.Width);
            writer.Write(this.Height);
            writer.Write(this.PixelByteSize);
            writer.Write(this.StepByteSize);
            this.PixelFormat.Write(writer);
        }
    }
}
