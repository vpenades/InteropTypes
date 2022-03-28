
// GENERATED CODE: using CodeGenUtils.t4

namespace InteropTypes.Graphics.Bitmaps.Processing
{    

        partial struct BitmapTransform
            : SpanBitmap.ITransfer<Pixel.BGR24, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.RGBP32, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.RGBP32, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.RGBP32, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.RGBP32, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.RGBP32, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.RGBP32>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.RGBP32>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.RGBP32>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.RGBP32>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.RGBP32>
            , SpanBitmap.ITransfer<Pixel.RGBP32, Pixel.RGBP32>
        {

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.BGR24> target)
            {
                if (Opacity >= 1)
                {
                    switch(UseBilinear)
                    {
                         case true: _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, true); break;
                         case false: _BitmapTransformImplementation.OpaquePixelsDirect(source, target, Transform); break;
                    }
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.BGR24> target)
            {
                if (Opacity >= 1)
                {
                    _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.BGR24> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.BGR24> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.BGR24> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBP32> source, SpanBitmap<Pixel.BGR24> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.RGB24> target)
            {
                if (Opacity >= 1)
                {
                    _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.RGB24> target)
            {
                if (Opacity >= 1)
                {
                    switch(UseBilinear)
                    {
                         case true: _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, true); break;
                         case false: _BitmapTransformImplementation.OpaquePixelsDirect(source, target, Transform); break;
                    }
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.RGB24> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.RGB24> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.RGB24> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBP32> source, SpanBitmap<Pixel.RGB24> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.RGBA32> target)
            {
                if (Opacity >= 1)
                {
                    _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.RGBA32> target)
            {
                if (Opacity >= 1)
                {
                    _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.RGBA32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.RGBA32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.RGBA32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBP32> source, SpanBitmap<Pixel.RGBA32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.BGRA32> target)
            {
                if (Opacity >= 1)
                {
                    _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.BGRA32> target)
            {
                if (Opacity >= 1)
                {
                    _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.BGRA32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.BGRA32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.BGRA32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBP32> source, SpanBitmap<Pixel.BGRA32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.BGRP32> target)
            {
                if (Opacity >= 1)
                {
                    _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.BGRP32> target)
            {
                if (Opacity >= 1)
                {
                    _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.BGRP32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.BGRP32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.BGRP32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBP32> source, SpanBitmap<Pixel.BGRP32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.RGBP32> target)
            {
                if (Opacity >= 1)
                {
                    _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.RGBP32> target)
            {
                if (Opacity >= 1)
                {
                    _BitmapTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.RGBP32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.RGBP32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.RGBP32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBP32> source, SpanBitmap<Pixel.RGBP32> target)
            {
                _BitmapTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, Opacity);
                return true;
            }
        }
        }