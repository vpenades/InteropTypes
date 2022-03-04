
// GENERATED CODE: using CodeGenUtils.t4

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace InteropTypes.Graphics
{

    partial class Pixel    
    {

        public interface IComposerApplicatorQ<TDstPixel>
        {
            void ApplyCompositionTo(ref TDstPixel dst, int opacity);
        }

        public interface IComposerApplicatorF<TDstPixel>
        {
            void ApplyCompositionTo(ref TDstPixel dst, float opacity);
        }

        partial struct BGR24
            : IComposerApplicatorQ<BGR24>
            , IComposerApplicatorQ<RGB24>
        {

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref BGR24 dst, int opacity)
            {
                var x = 16384 - opacity;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new BGR24(_R, _G, _B);
            }

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref RGB24 dst, int opacity)
            {
                var x = 16384 - opacity;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new RGB24(_R, _G, _B);
            }
        }
        partial struct RGB24
            : IComposerApplicatorQ<BGR24>
            , IComposerApplicatorQ<RGB24>
        {

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref BGR24 dst, int opacity)
            {
                var x = 16384 - opacity;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new BGR24(_R, _G, _B);
            }

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref RGB24 dst, int opacity)
            {
                var x = 16384 - opacity;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new RGB24(_R, _G, _B);
            }
        }
        partial struct BGRA32
            : IComposerApplicatorQ<BGR24>
            , IComposerApplicatorQ<RGB24>
        {

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref BGR24 dst, int opacity)
            {
                if (this.A == 0) return;
                opacity = opacity * this.A / 255;
                var x = 16384 - opacity;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new BGR24(_R, _G, _B);
            }

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref RGB24 dst, int opacity)
            {
                if (this.A == 0) return;
                opacity = opacity * this.A / 255;
                var x = 16384 - opacity;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new RGB24(_R, _G, _B);
            }
        }
        partial struct RGBA32
            : IComposerApplicatorQ<BGR24>
            , IComposerApplicatorQ<RGB24>
        {

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref BGR24 dst, int opacity)
            {
                if (this.A == 0) return;
                opacity = opacity * this.A / 255;
                var x = 16384 - opacity;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new BGR24(_R, _G, _B);
            }

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref RGB24 dst, int opacity)
            {
                if (this.A == 0) return;
                opacity = opacity * this.A / 255;
                var x = 16384 - opacity;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new RGB24(_R, _G, _B);
            }
        }
        partial struct ARGB32
            : IComposerApplicatorQ<BGR24>
            , IComposerApplicatorQ<RGB24>
        {

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref BGR24 dst, int opacity)
            {
                if (this.A == 0) return;
                opacity = opacity * this.A / 255;
                var x = 16384 - opacity;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new BGR24(_R, _G, _B);
            }

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref RGB24 dst, int opacity)
            {
                if (this.A == 0) return;
                opacity = opacity * this.A / 255;
                var x = 16384 - opacity;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new RGB24(_R, _G, _B);
            }
        }
        partial struct BGRP32
            : IComposerApplicatorQ<BGR24>
            , IComposerApplicatorQ<RGB24>
        {

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref BGR24 dst, int opacity)
            {
                if (this.A == 0) return;
                var x = 16384 - opacity;
                opacity = opacity * this.A / 255;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new BGR24(_R, _G, _B);
            }

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref RGB24 dst, int opacity)
            {
                if (this.A == 0) return;
                var x = 16384 - opacity;
                opacity = opacity * this.A / 255;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new RGB24(_R, _G, _B);
            }
        }
        partial struct RGBP32
            : IComposerApplicatorQ<BGR24>
            , IComposerApplicatorQ<RGB24>
        {

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref BGR24 dst, int opacity)
            {
                if (this.A == 0) return;
                var x = 16384 - opacity;
                opacity = opacity * this.A / 255;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new BGR24(_R, _G, _B);
            }

            /// <inheritdoc/>
            public void ApplyCompositionTo(ref RGB24 dst, int opacity)
            {
                if (this.A == 0) return;
                var x = 16384 - opacity;
                opacity = opacity * this.A / 255;
                var _R = (dst.R * x + this.R * opacity) / 16384;
                var _G = (dst.G * x + this.G * opacity) / 16384;
                var _B = (dst.B * x + this.B * opacity) / 16384;
                dst = new RGB24(_R, _G, _B);
            }
        }

    }
}