﻿using System.Windows;

using RENDERTARGET = System.Windows.Media.Imaging.RenderTargetBitmap;

namespace InteropDrawing.Backends
{
    partial class WPFDrawingContext2D
    {
        public static RENDERTARGET CreateRenderTargetBitmap(int width, int height, SceneView2D view, IDrawable2D scene)
        {
            var rt = new RENDERTARGET(width, height, 96, 96, System.Windows.Media.PixelFormats.Default);
            var ctx = new WPFDrawingContext2D();
            ctx.DrawScene(rt, new Size(width, height), view, scene);
            return rt;
        }

        public static RENDERTARGET CreateRenderTargetBitmap(int width, int height, SceneView3D view, IDrawable3D scene)
        {
            var rt = new RENDERTARGET(width, height, 96, 96, System.Windows.Media.PixelFormats.Default);
            var ctx = new WPFDrawingContext2D();
            ctx.DrawScene(rt, new Size(width, height), view, scene);
            return rt;
        }

        public static void SaveToBitmap(string filePath, int width, int height, SceneView2D view, IDrawable2D scene)
        {
            var rt = CreateRenderTargetBitmap(width, height, view,scene);
            SaveToPNG(rt, filePath);
        }

        public static void SaveToBitmap(string filePath, int width, int height, SceneView3D view, IDrawable3D scene)
        {
            var rt = CreateRenderTargetBitmap(width, height, view, scene);
            SaveToPNG(rt, filePath);
        }

        private static void SaveToPNG(RENDERTARGET rt, string filePath)
        {
            using (var s = System.IO.File.Create(filePath))
            {
                WritePNG(rt, s);
            }
        }

        private static void WritePNG(RENDERTARGET rt, System.IO.Stream writer)
        {
            // Save the image to a location on the disk.
            var encoder = new System.Windows.Media.Imaging.PngBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(rt));
            encoder.Save(writer);
        }

        private static void WriteJPEG(RENDERTARGET rt, System.IO.Stream writer)
        {
            // Save the image to a location on the disk.
            var encoder = new System.Windows.Media.Imaging.JpegBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(rt));
            encoder.Save(writer);
        }        
    }
}
