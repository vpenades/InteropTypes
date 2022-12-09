using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    partial struct Point3
    {
        /// <inheritdoc/>  
        public readonly override string ToString() { return XYZ.ToString(); }

        /// <inheritdoc/>
        public readonly string ToString(string format, IFormatProvider formatProvider) { return XYZ.ToString(format, formatProvider); }



        /*
        private static readonly System.Numerics.Vector3 XYZ_127 =new System.Numerics.Vector3(127);
               
        public static void SerializeQuantizedScaled(Action<Byte> writer, in Point3 point)
        {
            var v = point.XYZ;
            Encoded7BitInt32.FromSigned32((int)Math.Round(v.X)).WriteTo(writer);
            Encoded7BitInt32.FromSigned32((int)Math.Round(v.Y)).WriteTo(writer);
            Encoded7BitInt32.FromSigned32((int)Math.Round(v.Z)).WriteTo(writer);
        }

        public static void DeserializeQuantizedScaled(Func<Byte> reader, out Point3 point)
        {
            var x = Encoded7BitInt32.ReadFrom(reader).ToSigned32();
            var y = Encoded7BitInt32.ReadFrom(reader).ToSigned32();
            var z = Encoded7BitInt32.ReadFrom(reader).ToSigned32();
            point = new Point3(x, y, z);
        }

        public static void SerializeQuantizedDirectionLength(Action<Byte> writer, in Point3 point)
        {
            var v = point.XYZ;

            var len = v.Length();
            v /= len;

            writer(ToQuantizedByte(v.X));
            writer(ToQuantizedByte(v.Y));
            writer(ToQuantizedByte(v.Z));
            
            Encoded7BitInt32.FromUnsigned32((uint)Math.Round(len)).WriteTo(writer);
        }

        public static void DeserializeQuantizedDirectionLength(Func<Byte> reader, out Point3 point)
        {
            var x = ToNormalizedFloat(reader());
            var y = ToNormalizedFloat(reader());
            var z = ToNormalizedFloat(reader());
            var l = Encoded7BitInt32.ReadFrom(reader).ToUnsigned32();
            point = new System.Numerics.Vector3(x, y, z) * l;
        }

        private static Byte ToQuantizedByte(float normalized)
        {
            if (normalized < -1) normalized = -1;
            if (normalized > 1) normalized = 1;
            var q = (int)(normalized * 127);

            return (byte)q;
        }

        private static float ToNormalizedFloat(Byte value)
        {
            float x = value > 127
                ? value - 256
                : value;

            return x /= 127f;
        }*/
    }
}
