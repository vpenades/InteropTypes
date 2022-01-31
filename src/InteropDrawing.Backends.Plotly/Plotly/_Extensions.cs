using System;
using System.Collections.Generic;
using System.Text;

using static System.FormattableString;

using POINT2 = InteropTypes.Graphics.Drawing.Point2;
using POINT3 = InteropTypes.Graphics.Drawing.Point3;

namespace Plotly
{
    static class _Extensions
    {
        public static int ToPlotlyIntegerRGB(this System.Drawing.Color color)
        {
            var ccc = color.R * 65536 + color.G * 256 + color.B;

            return (int)ccc;
        }

        public static string ToPlotlyStringRGBA(this System.Drawing.Color color)
        {
            if (color.A == 255) return color.ToPlotlyStringRGB();

            var opaci = (float)color.A / 255f;
            return Invariant($"rgba({color.R}, {color.G}, {color.B}, {opaci})");
        }

        public static string ToPlotlyStringRGB(this System.Drawing.Color color)
        {            
            return Invariant($"rgb({color.R}, {color.G}, {color.B})");
        }

        public static float ToPlotlyOpacity(this System.Drawing.Color color)
        {
            return (float)color.A / 255f;
        }

        public static (float[] x, float[] y) Split(this ReadOnlySpan<POINT2> points)
        {
            var mx = new float[points.Length];
            var my = new float[points.Length];

            for (int i = 0; i < points.Length; ++i)
            {
                mx[i] = points[i].X;
                my[i] = points[i].Y;
            }

            return (mx, my);
        }

        public static (float[] x, float[] y, float[] z) Split(this ReadOnlySpan<POINT3> points)
        {
            var mx = new float[points.Length];
            var my = new float[points.Length];
            var mz = new float[points.Length];

            for (int i = 0; i < points.Length; ++i)
            {
                mx[i] = points[i].X;
                my[i] = points[i].Y;
                mz[i] = points[i].Z;
            }

            return (mx, my, mz);
        }


        public static TContainer CreateTypedProperty<TContainer>(this Array array,
            Func<Single[], TContainer> asFloats,
            Func<Int32[], TContainer> asIntegers = null,            
            Func<String[], TContainer> asStrings = null,
            Func<DateTime[], TContainer> asDates = null,
            Func<Boolean[], TContainer> asBooleans = null)
        {
            if (array is Int32[] arrayOfInts) return asIntegers(arrayOfInts);
            if (array is Single[] arrayOfFloats) return asFloats(arrayOfFloats);            
            if (array is String[] arrayOfStrings) return asStrings(arrayOfStrings);
            if (array is Boolean[] arrayOfBools) return asBooleans(arrayOfBools);
            if (array is DateTime[] arrayOfDates) return asDates(arrayOfDates);
            throw new NotImplementedException();
        }

        public static TContainer ArrayOrValue<TValue, TContainer>(this TValue[] array, Func<TValue, TContainer> asValue, Func<TValue[], TContainer> asArray)
            where TValue : IEquatable<TValue>
        {
            if (array.Length == 1) return asValue(array[0]);

            for (int i = 1; i < array.Length; ++i)
            {
                if (!array[0].Equals(array[i])) return asArray(array);
            }

            return asValue(array[0]);

        }
    }
}
