using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;

using InteropTypes.IO.Controls;

namespace InteropTypes.Views;

public partial class ThumbnailsDemoView : UserControl
{
    public ThumbnailsDemoView()
    {
        InitializeComponent();
    }

    private async void _OnClick_PickDirectory(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var parentWindow = TopLevel.GetTopLevel(this)!;
        var storageProvider = parentWindow.StorageProvider;

        var folders = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "select folder",
            AllowMultiple = false // Cambiar a true si deseas permitir selección múltiple
        });

        if (folders.Count != 1) return;

        // Obtener el objeto de la carpeta
        var folder = folders[0];

        // Obtener la ruta absoluta en el disco local
        string folderPath = folder.Path.LocalPath;

        var dir = new System.IO.DirectoryInfo(folderPath);

        var btn = sender as Button;
        btn.IsEnabled = false;
        myDirectoryFiles.ItemsSource = null;

        void work()
        {
            var files = dir.GetFileSystemInfos();            

            Avalonia.Threading.Dispatcher.UIThread.Post(() => { myDirectoryFiles.ItemsSource = files; btn.IsEnabled = true; });
        }

        await Task.Run(work);
    }

    private void FileThumbnailBox_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (sender is FileThumbnailBox ftbox)
        {
            var fs = ftbox.FileSystemSource;
            var psi = new System.Diagnostics.ProcessStartInfo(fs.FullName);
            psi.UseShellExecute = true;
            System.Diagnostics.Process.Start(psi);
        }
    }
}