﻿using System.Windows;

using InteropTypes.Graphics.Drawing;

using WPFRENDERTARGET = System.Windows.Media.Imaging.RenderTargetBitmap;

namespace InteropTypes.Graphics.Backends
{
    partial class Canvas2DFactory
    {
        public static WPFRENDERTARGET CreateRenderTargetBitmap(int width, int height, SceneView2D view, IDrawingBrush<ICanvas2D> scene)
        {
            var rt = new WPFRENDERTARGET(width, height, 96, 96, System.Windows.Media.PixelFormats.Default);
            var ctx = new Canvas2DFactory();
            ctx.DrawScene(rt, new Size(width, height), view, scene);
            return rt;
        }

        public static WPFRENDERTARGET CreateRenderTargetBitmap(int width, int height, SceneView3D view, IDrawingBrush<IScene3D> scene)
        {
            var rt = new WPFRENDERTARGET(width, height, 96, 96, System.Windows.Media.PixelFormats.Default);
            var ctx = new Canvas2DFactory();
            ctx.DrawScene(rt, new Size(width, height), view, scene);
            return rt;
        }

        public static void SaveToBitmap(string filePath, int width, int height, SceneView2D view, IDrawingBrush<ICanvas2D> scene)
        {
            var rt = CreateRenderTargetBitmap(width, height, view,scene);
            SaveToPNG(rt, filePath);
        }

        public static void SaveToBitmap(string filePath, int width, int height, SceneView3D view, IDrawingBrush<IScene3D> scene)
        {
            var rt = CreateRenderTargetBitmap(width, height, view, scene);
            SaveToPNG(rt, filePath);
        }

        private static void SaveToPNG(WPFRENDERTARGET rt, string filePath)
        {
            using (var s = System.IO.File.Create(filePath))
            {
                WritePNG(rt, s);
            }
        }

        private static void WritePNG(WPFRENDERTARGET rt, System.IO.Stream writer)
        {
            // Save the image to a location on the disk.
            var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(rt));
            encoder.Save(writer);
        }

        private static void WriteJPEG(WPFRENDERTARGET rt, System.IO.Stream writer)
        {
            // Save the image to a location on the disk.
            var encoder = new System.Windows.Media.Imaging.JpegBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(rt));
            encoder.Save(writer);
        }        
    }
}
