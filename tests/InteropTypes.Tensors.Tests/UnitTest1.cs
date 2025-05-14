using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using InteropTypes.Tensors.Imaging;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

using NUnit.Framework;

using XY = System.Numerics.Vector2;

namespace InteropTypes.Tensors
{
    [Category("Interop Tensors")]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }        

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void TestByteToFloatConversion()
        {
            var bytes = Enumerable.Range(0,256).Select(idx => (Byte)idx).ToArray();

            var floats = new float[256];

            _ArrayUtilities.TryConvertSpan<Byte,float>(bytes, floats);

            for(int i=0; i < floats.Length; i++)
            {
                floats[i] /= 255;
            }

            var hexints = System.Runtime.InteropServices.MemoryMarshal.Cast<float,uint>(floats);

            foreach(var v in hexints)
            {
                TestContext.Out.WriteLine(v.ToString("x"));
            }

        }


        [Test]
        public void TestIndexing()
        {
            var s1 = new TensorSize2(5, 5);

            var idx = s1.GetFlattenedIndex(1, 2);

            var indices = s1.GetDecomposedIndex(idx);

            Assert.That(indices[0], Is.EqualTo(1));
            Assert.That(indices[1], Is.EqualTo(2));
        }

        [Test]
        public void MatrixMultiplyTest()
        {
            var m1 = Matrix4x4.CreateFromYawPitchRoll(1, 2, 3) * Matrix4x4.CreateTranslation(2,5,6);
            var m2 = Matrix4x4.CreateScale(1,2,1) * Matrix4x4.CreateFromYawPitchRoll(2, -1, 2);
            var mr = m1 * m2;

            var t1 = SpanTensor.Wrap(m1)[0];
            var t2 = SpanTensor.Wrap(m2)[0];
            var tr = SpanTensor.Wrap(Matrix4x4.Identity)[0];
            SpanTensor.MatrixMultiply(t1, t2, tr);
            var trm = SpanTensor.ToMatrix4x4(tr);

            Assert.That(trm, Is.EqualTo(mr));            
        }

        [Test]
        public void PointCloudFitting()
        {
            // https://github.com/ClayFlannigan/icp/blob/master/icp.py

            // create original point cloud
            var ppp0 = new Vector3[16];
            var rnd = new Random(117);
            for(int i=0; i < ppp0.Length; ++i)
            {
                var x = (float)rnd.NextDouble() * 16 - 8;
                var y = (float)rnd.NextDouble() * 16 - 8;
                var z = (float)rnd.NextDouble() * 16 - 8;
                ppp0[i] = new Vector3(x, y, z);
            }

            // create rotated point clound
            var rot = Matrix4x4.CreateFromYawPitchRoll(1, 2, 3);
            Matrix4x4.Invert(rot, out Matrix4x4 irot);
            
            var ppp1 = new Vector3[ppp0.Length];
            for (int i = 0; i < ppp0.Length; ++i)
            {
                ppp1[i] = Vector3.Transform(ppp0[i], rot);
            }

            // original point cloud tensor
            var t0 = SpanTensor.Wrap(ppp0);
            var t0t = new SpanTensor2<float>(t0.Dimensions[1], t0.Dimensions[0]);
            SpanTensor.Transpose(t0, t0t);

            // rotated point cloud tensor
            var t1 = SpanTensor.Wrap(ppp1);
            var t1t = new SpanTensor2<float>(t1.Dimensions[1], t1.Dimensions[0]);
            SpanTensor.Transpose(t1, t1t);

            // multiplied
            var combined = new SpanTensor2<float>(3, 3);
            SpanTensor.MatrixMultiply(t1t, t0, combined);

            // decomposition
            var v = new SpanTensor2<float>(3, 3);
            var w = new float[3];

            combined.SVD(w, v);
        }


        [Test]
        public void LeastSquaresFitting2D() // does not work
        {
            var aa = new XY[] { XY.Zero, XY.One, new XY(3, 7), new XY(2,5), new XY(-5, 7) };
            var bb = new XY[aa.Length];

            var referenceX = Matrix3x2.CreateScale(2,3) * Matrix3x2.CreateRotation(0.5f) * Matrix3x2.CreateTranslation(6, 7);

            for(int i=0; i<aa.Length; i++)
            {
                bb[i] = XY.Transform(aa[i], referenceX);
            }            

            var resultX = LeastSquaresFitting2D_MathNet(aa, bb);

            Assert.That(resultX.M11, Is.EqualTo(referenceX.M11).Within(0.0001f));
            Assert.That(resultX.M12, Is.EqualTo(referenceX.M12).Within(0.0001f));
            Assert.That(resultX.M21, Is.EqualTo(referenceX.M21).Within(0.0001f));
            Assert.That(resultX.M22, Is.EqualTo(referenceX.M22).Within(0.0001f));
            Assert.That(resultX.M31, Is.EqualTo(referenceX.M31).Within(0.0001f));
            Assert.That(resultX.M32, Is.EqualTo(referenceX.M32).Within(0.0001f));
        }


        public static Matrix3x2 LeastSquaresFitting2D_MathNet(IReadOnlyList<XY> sourcePoints, IReadOnlyList<XY> destinationPoints)
        {
            if (sourcePoints.Count != destinationPoints.Count) throw new ArgumentException("size mismatch", nameof(destinationPoints));

            if (sourcePoints.Count == 0) return Matrix3x2.Identity;            

            if (sourcePoints.Count == 1) return Matrix3x2.CreateTranslation(destinationPoints[0] - sourcePoints[0]);

            // center around centroids (does not work?) (maybe we can add the centroid as part of the points?)
            var scenter = sourcePoints.Aggregate((a, b) => a + b) / sourcePoints.Count;
            var dcenter = destinationPoints.Aggregate((a, b) => a + b) / destinationPoints.Count;

            // center around 1st point
            scenter = sourcePoints[0];
            dcenter = destinationPoints[0];

            var d = dcenter - scenter;            

            var src = new double[sourcePoints.Count, 2];
            for (int i = 0; i < sourcePoints.Count; ++i)
            {
                var p = sourcePoints[i] - scenter; // bring to origin. Technically we should calculate the point's centroid ?

                src[i, 0] = p.X;
                src[i, 1] = p.Y;
            }

            var dst = new double[destinationPoints.Count, 2];
            for (int i = 0; i < destinationPoints.Count; ++i)
            {
                var p = destinationPoints[i] - dcenter; // bring to origin. Technically we should calculate the point's centroid ?

                dst[i, 0] = p.X;
                dst[i, 1] = p.Y;
            }

            // Create matrices from the points
            var sourceMatrix = DenseMatrix.OfArray(src);
            var destinationMatrix = DenseMatrix.OfArray(dst);


            // Perform least squares calculation to find the transformation matrix
            var qrMatrix = sourceMatrix.QR();
            var transformationMatrix = qrMatrix.Solve(destinationMatrix);

            Matrix3x2 result = default;

            result.M11 = (float)transformationMatrix[0,0];
            result.M12 = (float)transformationMatrix[0,1];
            result.M21 = (float)transformationMatrix[1,0];
            result.M22 = (float)transformationMatrix[1,1];
            result.Translation = d;

            return result;
        }

    }


    static class SingularValueDecomposition
    {
        public static int SVD(this SpanTensor2<float> a, float[] w, SpanTensor2<float> v)
        {
            int flag, jj, l = 0, nm = 0;
            float c, f, h, s, x, y, z;
            float anorm = 0f, g = 0f, scale = 0f;


            int m = a.Dimensions[0]; // not sure if it's 0 or 1
            int n = a.Dimensions[1]; // not sure if it's 0 or 1


            if (m < n)
            {
                // fprintf(stderr, "#rows must be > #cols \n");
                return (0);
            }

            var rv1 = new float[n]; // use stackalloc

            /* Householder reduction to bidiagonal form */
            for (int i = 0; i < n; i++)
            {
                /* left-hand reduction */
                l = i + 1;
                rv1[i] = scale * g;
                g = s = scale = 0f;

                if (i < m)
                {
                    for (int k = i; k < m; k++) scale += Math.Abs(a[k, i]);

                    if (scale != 0)
                    {
                        for (int k = i; k < m; k++)
                        {
                            a[k, i] = (a[k, i] / scale);
                            s += (a[k, i] * a[k, i]);
                        }
                        f = a[i, i];
                        g = -_Sign((float)Math.Sqrt(s), f);
                        h = f * g - s;
                        a[i, i] = (f - g);
                        if (i != n - 1)
                        {
                            for (int j = l; j < n; j++)
                            {
                                s = 0;
                                for (int k = i; k < m; k++) s += (a[k, i] * a[k, j]);
                                f = s / h;
                                for (int k = i; k < m; k++) a[k, j] += (f * a[k, i]);
                            }
                        }
                        for (int k = i; k < m; k++) a[k, i] = (a[k, i] * scale);
                    }
                }
                w[i] = (scale * g);

                /* right-hand reduction */
                g = s = scale = 0f;
                if (i < m && i != n - 1)
                {
                    for (int k = l; k < n; k++) scale += Math.Abs(a[i, k]);

                    if (scale != 0)
                    {
                        for (int k = l; k < n; k++)
                        {
                            a[i, k] = (a[i, k] / scale);
                            s += (a[i, k] * a[i, k]);
                        }
                        f = a[i, l];
                        g = -_Sign((float)Math.Sqrt(s), f);
                        h = f * g - s;
                        a[i, l] = (f - g);
                        for (int k = l; k < n; k++) rv1[k] = a[i, k] / h;
                        if (i != m - 1)
                        {
                            for (int j = l; j < m; j++)
                            {
                                s = 0;
                                for (int k = l; k < n; k++) s += (a[j, k] * a[i, k]);
                                for (int k = l; k < n; k++) a[j, k] += s * rv1[k];
                            }
                        }
                        for (int k = l; k < n; k++) a[i, k] = (a[i, k] * scale);
                    }
                }
                anorm = Math.Max(anorm, (Math.Abs(w[i]) + Math.Abs(rv1[i])));
            }

            /* accumulate the right-hand transformation */
            for (int i = n - 1; i >= 0; i--)
            {
                if (i < n - 1)
                {
                    if (g != 0)
                    {
                        for (int j = l; j < n; j++) v[j, i] = ((a[i, j] / a[i, l]) / g);
                        /* double division to avoid underflow */
                        for (int j = l; j < n; j++)
                        {
                            s = 0;
                            for (int k = l; k < n; k++) s += (a[i, k] * v[k, j]);
                            for (int k = l; k < n; k++) v[k, j] += (s * v[k, i]);
                        }
                    }
                    for (int j = l; j < n; j++) v[i, j] = v[j, i] = 0f;
                }
                v[i, i] = 1f;
                g = rv1[i];
                l = i;
            }

            /* accumulate the left-hand transformation */
            for (int i = n - 1; i >= 0; i--)
            {
                l = i + 1;
                g = w[i];
                if (i < n - 1)
                    for (int j = l; j < n; j++) a[i, j] = 0f;
                if (g != 0)
                {
                    g = 1f / g;
                    if (i != n - 1)
                    {
                        for (int j = l; j < n; j++)
                        {
                            s = 0;
                            for (int k = l; k < m; k++) s += (a[k, i] * a[k, j]);
                            f = (s / a[i, i]) * g;
                            for (int k = i; k < m; k++) a[k, j] += (f * a[k, i]);
                        }
                    }
                    for (int j = i; j < m; j++) a[j, i] = (a[j, i] * g);
                }
                else
                {
                    for (int j = i; j < m; j++) a[j, i] = 0f;
                }
                ++a[i, i];
            }

            /* diagonalize the bidiagonal form */
            for (int k = n - 1; k >= 0; k--)
            {                             /* loop over singular values */
                for (int its = 0; its < 30; its++)
                {                         /* loop over allowed iterations */
                    flag = 1;
                    for (l = k; l >= 0; l--)
                    {                     /* test for splitting */
                        nm = l - 1;
                        if (Math.Abs(rv1[l]) + anorm == anorm) { flag = 0; break; }
                        if (Math.Abs(w[nm]) + anorm == anorm) break;
                    }
                    if (flag != 0)
                    {
                        c = 0f;
                        s = 1f;
                        for (int i = l; i <= k; i++)
                        {
                            f = s * rv1[i];
                            if (Math.Abs(f) + anorm != anorm)
                            {
                                g = w[i];
                                h = _Pythagoras(f, g);
                                w[i] = h;
                                h = 1f / h;
                                c = g * h;
                                s = -f * h;
                                for (int j = 0; j < m; j++)
                                {
                                    y = a[j, nm];
                                    z = a[j, i];
                                    a[j, nm] = y * c + z * s;
                                    a[j, i] = z * c - y * s;
                                }
                            }
                        }
                    }

                    z = w[k];

                    if (l == k)
                    {                  /* convergence */
                        if (z < 0f)
                        {              /* make singular value nonnegative */
                            w[k] = -z;
                            for (int j = 0; j < n; j++) v[j, k] = (-v[j, k]);
                        }
                        break;
                    }

                    if (its >= 30)
                    {
                        // fprintf(stderr, "No convergence after 30,000! iterations \n");
                        return (0);
                    }

                    /* shift from bottom 2 x 2 minor */
                    x = w[l];
                    nm = k - 1;
                    y = w[nm];
                    g = rv1[nm];
                    h = rv1[k];
                    f = ((y - z) * (y + z) + (g - h) * (g + h)) / (2f * h * y);
                    g = _Pythagoras(f, 1f);
                    f = ((x - z) * (x + z) + h * ((y / (f + _Sign(g, f))) - h)) / x;

                    /* next QR transformation */
                    c = s = 1f;
                    for (int j = l; j <= nm; j++)
                    {
                        int i = j + 1;
                        g = rv1[i];
                        y = w[i];
                        h = s * g;
                        g = c * g;

                        z = _Pythagoras(f, h);
                        rv1[j] = z;
                        c = f / z;
                        s = h / z;
                        f = x * c + g * s;
                        g = g * c - x * s;
                        h = y * s;
                        y *= c;

                        for (jj = 0; jj < n; jj++)
                        {
                            x = v[jj, j];
                            z = v[jj, i];
                            v[jj, j] = x * c + z * s;
                            v[jj, i] = z * c - x * s;
                        }

                        z = _Pythagoras(f, h);
                        w[j] = z;
                        if (z != 0) { z = 1f / z; c = f * z; s = h * z; }
                        f = (c * g) + (s * y);
                        x = (c * y) - (s * g);

                        for (jj = 0; jj < m; jj++)
                        {
                            y = a[jj, j];
                            z = a[jj, i];
                            a[jj, j] = y * c + z * s;
                            a[jj, i] = z * c - y * s;
                        }
                    }

                    rv1[l] = 0f;
                    rv1[k] = f;
                    w[k] = x;
                }
            }

            return 1;
        }

        static float _Sign(float a, float b) { return (b) >= 0f ? Math.Abs(a) : -Math.Abs(a); }

        static float _Pythagoras(float a, float b)
        {
            float at = Math.Abs(a), bt = Math.Abs(b), ct, result;

            if (at > bt) { ct = bt / at; result = at * (float)Math.Sqrt(1f + ct * ct); }
            else if (bt > 0f) { ct = at / bt; result = bt * (float)Math.Sqrt(1f + ct * ct); }
            else result = 0f;
            return (result);
        }
    }
}