using System;
using System.Collections.Generic;
using System.Text;

using Android.App;
using Android.Runtime;

using Avalonia;
using Avalonia.Android;

// Avalonia 12 Android example
// https://github.com/AvaloniaUI/Avalonia/blob/master/samples/ControlCatalog.Android/MainActivity.cs

namespace InteropTypes
{
    [Application]
    public class Application : AvaloniaAndroidApplication<App>
    {
        protected Application(nint javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
        {
            return base.CustomizeAppBuilder(builder);

                /*
                 .AfterSetup(_ =>
                 {
                     Pages.EmbedSample.Implementation = new EmbedSampleAndroid();
                 });*/
        }
    }
}
