﻿using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Drawing;

using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDemo
{
    class _DynamicTexture : IDrawingBrush<ICanvas2D>
    {
        public _DynamicTexture(GraphicsDevice device)
        {
            _Device = device;
        }

        private readonly GraphicsDevice _Device;

        private ImageSource _DynAsset;
        private Texture2D _DynTexture;
        private readonly Byte[] _DynData = new byte[64 * 64 * 4];

        private static readonly Random _Randomizer = new Random();

        public void DrawTo(ICanvas2D dc)
        {
            if (_DynTexture == null)
            {
                _DynTexture = new Texture2D(_Device, 64, 64);
                _DynAsset = new ImageSource(_DynTexture, (0, 0), (64, 64), (32, 32));
            }

            _Randomizer.NextBytes(_DynData);
            _DynTexture.SetData(_DynData);
            dc.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(400, 50), _DynAsset);
        }
    }
}
