
// GENERATED CODE: using CodeGenUtils.t4

namespace InteropTypes.Graphics.Bitmaps.Processing
{    

        partial struct BitmapTransform
            : SpanBitmap.ITransfer<Pixel.BGR24, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.Luminance8, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.Luminance32F, Pixel.BGR24>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.Luminance8, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.Luminance32F, Pixel.RGB24>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.Luminance8, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.Luminance32F, Pixel.RGBA32>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.Luminance8, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.Luminance32F, Pixel.BGRA32>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.Luminance8, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.Luminance32F, Pixel.BGRP32>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.BGR96F>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.BGR96F>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.BGR96F>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.BGR96F>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.BGR96F>
            , SpanBitmap.ITransfer<Pixel.Luminance8, Pixel.BGR96F>
            , SpanBitmap.ITransfer<Pixel.Luminance32F, Pixel.BGR96F>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.RGB96F>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.RGB96F>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.RGB96F>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.RGB96F>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.RGB96F>
            , SpanBitmap.ITransfer<Pixel.Luminance8, Pixel.RGB96F>
            , SpanBitmap.ITransfer<Pixel.Luminance32F, Pixel.RGB96F>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.Luminance8>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.Luminance8>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.Luminance8>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.Luminance8>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.Luminance8>
            , SpanBitmap.ITransfer<Pixel.Luminance8, Pixel.Luminance8>
            , SpanBitmap.ITransfer<Pixel.Luminance32F, Pixel.Luminance8>
            , SpanBitmap.ITransfer<Pixel.BGR24, Pixel.Luminance32F>
            , SpanBitmap.ITransfer<Pixel.RGB24, Pixel.Luminance32F>
            , SpanBitmap.ITransfer<Pixel.RGBA32, Pixel.Luminance32F>
            , SpanBitmap.ITransfer<Pixel.BGRA32, Pixel.Luminance32F>
            , SpanBitmap.ITransfer<Pixel.BGRP32, Pixel.Luminance32F>
            , SpanBitmap.ITransfer<Pixel.Luminance8, Pixel.Luminance32F>
            , SpanBitmap.ITransfer<Pixel.Luminance32F, Pixel.Luminance32F>
        {

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.BGR24> target)
            {
                if (PixelOp.IsIdentity)
                {
                    switch(UseBilinear)
                    {
                         case true: _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, true); break;
                         case false: _PixelsTransformImplementation.OpaquePixelsDirect(source, target, Transform); break;
                    }
                    return true;
                }

                var localPixelOp = new Pixel.BGR24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.BGR24> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.BGR24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.BGR24> target)
            {

                var localPixelOp = new Pixel.BGR24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.BGR24> target)
            {

                var localPixelOp = new Pixel.BGR24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.BGR24> target)
            {

                var localPixelOp = new Pixel.BGR24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance8> source, SpanBitmap<Pixel.BGR24> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.BGR24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance32F> source, SpanBitmap<Pixel.BGR24> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.BGR24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.RGB24> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.RGB24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.RGB24> target)
            {
                if (PixelOp.IsIdentity)
                {
                    switch(UseBilinear)
                    {
                         case true: _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, true); break;
                         case false: _PixelsTransformImplementation.OpaquePixelsDirect(source, target, Transform); break;
                    }
                    return true;
                }

                var localPixelOp = new Pixel.RGB24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.RGB24> target)
            {

                var localPixelOp = new Pixel.RGB24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.RGB24> target)
            {

                var localPixelOp = new Pixel.RGB24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.RGB24> target)
            {

                var localPixelOp = new Pixel.RGB24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance8> source, SpanBitmap<Pixel.RGB24> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.RGB24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance32F> source, SpanBitmap<Pixel.RGB24> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.RGB24.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.RGBA32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.RGBA32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.RGBA32> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.RGBA32> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.RGBA32> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance8> source, SpanBitmap<Pixel.RGBA32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance32F> source, SpanBitmap<Pixel.RGBA32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.BGRA32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.BGRA32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.BGRA32> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.BGRA32> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.BGRA32> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance8> source, SpanBitmap<Pixel.BGRA32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance32F> source, SpanBitmap<Pixel.BGRA32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.BGRP32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.BGRP32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.BGRP32> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.BGRP32> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.BGRP32> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance8> source, SpanBitmap<Pixel.BGRP32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance32F> source, SpanBitmap<Pixel.BGRP32> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.BGR96F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.BGR96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.BGR96F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.BGR96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.BGR96F> target)
            {

                var localPixelOp = new Pixel.BGR96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.BGR96F> target)
            {

                var localPixelOp = new Pixel.BGR96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.BGR96F> target)
            {

                var localPixelOp = new Pixel.BGR96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance8> source, SpanBitmap<Pixel.BGR96F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.BGR96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance32F> source, SpanBitmap<Pixel.BGR96F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.BGR96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.RGB96F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.RGB96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.RGB96F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.RGB96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.RGB96F> target)
            {

                var localPixelOp = new Pixel.RGB96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.RGB96F> target)
            {

                var localPixelOp = new Pixel.RGB96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.RGB96F> target)
            {

                var localPixelOp = new Pixel.RGB96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance8> source, SpanBitmap<Pixel.RGB96F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.RGB96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance32F> source, SpanBitmap<Pixel.RGB96F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }

                var localPixelOp = new Pixel.RGB96F.MulAdd(this.PixelOp);

                switch(PixelOp.IsOpacity)
                {
                     case true: _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity); break;
                     case false: _PixelsTransformImplementation.ConvertPixels(source, target, Transform, UseBilinear, localPixelOp); break;
                }
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.Luminance8> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.Luminance8> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.Luminance8> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.Luminance8> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.Luminance8> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance8> source, SpanBitmap<Pixel.Luminance8> target)
            {
                if (PixelOp.IsIdentity)
                {
                    switch(UseBilinear)
                    {
                         case true: _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, true); break;
                         case false: _PixelsTransformImplementation.OpaquePixelsDirect(source, target, Transform); break;
                    }
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance32F> source, SpanBitmap<Pixel.Luminance8> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.Luminance32F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.Luminance32F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.RGBA32> source, SpanBitmap<Pixel.Luminance32F> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRA32> source, SpanBitmap<Pixel.Luminance32F> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.BGRP32> source, SpanBitmap<Pixel.Luminance32F> target)
            {
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance8> source, SpanBitmap<Pixel.Luminance32F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, UseBilinear);
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }

            /// <inheritdoc/>
            public bool TryTransfer(SpanBitmap<Pixel.Luminance32F> source, SpanBitmap<Pixel.Luminance32F> target)
            {
                if (PixelOp.IsIdentity)
                {
                    switch(UseBilinear)
                    {
                         case true: _PixelsTransformImplementation.OpaquePixelsConvert(source, target, Transform, true); break;
                         case false: _PixelsTransformImplementation.OpaquePixelsDirect(source, target, Transform); break;
                    }
                    return true;
                }
                _PixelsTransformImplementation.ComposePixels(source, target, Transform, UseBilinear, PixelOp.Opacity);
                return true;
            }
        }
        
        }