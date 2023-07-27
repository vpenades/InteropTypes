using System;

using Avalonia.Controls;
using Avalonia.Threading;

namespace InteropTypes.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        _timer.Tick += _timer_Tick;

        this.Loaded += (s, e) => _timer.Start();
        this.Unloaded += (s, e) => _timer.Stop();
    }

    private readonly DispatcherTimer _timer = new() { Interval = TimeSpan.FromSeconds(1f / 30) };

    private InteropTypes._Scene2D _Scene = new _Scene2D();
    private InteropTypes._Sprites2D _Sprites = new _Sprites2D();

    private InteropTypes.BindableNoiseTexture _NoiseBitmap = new BindableNoiseTexture();

    private InteropTypes.Graphics.Drawing.ImageSource _TinyCat = InteropTypes.Graphics.Drawing.ImageSource.CreateFromBitmap("avares://InteropTypes.Graphics.Avalonia.Demo.App/Assets/tinycat.png", (31, 33), (15, 15));

    private void _timer_Tick(object? sender, EventArgs e)
    {
        myCanvas.InvalidateVisual();
    }

    void OnRender(object sender, InteropTypes.Graphics.Drawing.Canvas2DArgs args)
    {
        _Scene.DrawTo(args.Canvas);
        _Sprites.DrawTo(args.Canvas);

        args.Canvas.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(30, 30), _TinyCat);

        args.Canvas.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(100, 30), _NoiseBitmap.Sprite);
    }
}
