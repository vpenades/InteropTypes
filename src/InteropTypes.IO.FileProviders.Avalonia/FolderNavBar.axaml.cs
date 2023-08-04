using System.IO;

using Avalonia;
using Avalonia.Controls;

using DIRECTORYINFO = InteropTypes.IO.PhysicalDirectoryInfo;

namespace InteropTypes.IO
{
    public partial class FolderNavBar : UserControl
    {
        public FolderNavBar()
        {
            InitializeComponent();

            this.Loaded += FolderNavBar_Loaded;
        }

        private void FolderNavBar_Loaded(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _GoToHome();
        }

        public static readonly DirectProperty<FolderNavBar, DIRECTORYINFO> CurrentDirectoryProperty
            = AvaloniaProperty.RegisterDirect<FolderNavBar, DIRECTORYINFO>(nameof(CurrentDirectory), o => o.CurrentDirectory, (o, v) => o.CurrentDirectory = v);

        private DIRECTORYINFO _CurrentDir;

        public DIRECTORYINFO CurrentDirectory
        {
            get { return _CurrentDir; }
            set
            {
                if (SetAndRaise(CurrentDirectoryProperty, ref _CurrentDir, value))
                {
                    myCurrentPath.Text = _CurrentDir.PhysicalPath;
                }
            }
        }

        private void OnClick_FolderUp(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var pdinfo = CurrentDirectory;
            
            var dinfo = new System.IO.DirectoryInfo(pdinfo.PhysicalPath).Parent;
            if (dinfo == null) return;

            CurrentDirectory = new DIRECTORYINFO(dinfo);            
        }

        private void OnClick_GoToAppDir(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _GoToHome();
        }

        private void OnClick_FolderBrowse(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (DIRECTORYINFO.TryBrowseFolderDialog(out var dinfo, null, null, null))
            {
                CurrentDirectory = dinfo;
            }
        }


        private void _GoToHome()
        {
            // string DownloadsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);

            CurrentDirectory = DIRECTORYINFO.ApplicationDirectory;
        }
    }
}
