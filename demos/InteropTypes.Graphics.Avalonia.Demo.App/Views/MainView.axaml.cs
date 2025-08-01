using System;

using Avalonia.Controls;
using Avalonia.Threading;

using InteropTypes.Graphics.Backends.Bitmaps;
using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        this.DataContext = new InteropTypes.AppMVVM();

        this.Loaded += MainView_Loaded;
    }

    private async void MainView_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        System.Threading.Tasks.Task.Run(_AsyncUpdateBitmap);

        await _Sprites.RunDynamicsAsync();        
    }

    private void _AsyncUpdateBitmap()
    {
        var bmp = new MemoryBitmap(256, 256, Pixel.BGR24.Format);
        var rnd = new Random();

        while (true)
        {
            System.Threading.Thread.Sleep(1000 / 100);

            if (bmp.TryGetBuffer(out var buffer))
            {
                var data = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, int>(buffer.Array);

                for (int i = 0; i < data.Length; ++i)
                {
                    data[i] = rnd.Next();
                }
            }

            ClientBitmap.Update(bmp);
        }
    }

    public AvaloniaBitmapSwapChain ClientBitmap { get; } = new AvaloniaBitmapSwapChain();

    private InteropTypes._Scene2D _Scene = new _Scene2D();
    private InteropTypes._Sprites2D _Sprites = new _Sprites2D();    

    private InteropTypes.Graphics.Drawing.ImageSource _TinyCat = InteropTypes.Graphics.Drawing.ImageSource.CreateFromBitmap("avares://InteropTypes.Graphics.Avalonia.Demo.App/Assets/tinycat.png", (31, 33), (15, 15));    

    void OnRender(object sender, InteropTypes.Graphics.Drawing.Canvas2DArgs args)
    {
        GroupBox.ArrangeAtlas(800, _Scene, _Sprites);
        _Scene.DrawTo(args.Canvas);
        _Sprites.DrawTo(args.Canvas);

        args.Canvas.DrawImage(System.Numerics.Matrix3x2.CreateTranslation(30, 30), _TinyCat);        
    }    
}
