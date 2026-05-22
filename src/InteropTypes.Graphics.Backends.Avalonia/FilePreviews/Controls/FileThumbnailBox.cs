using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

using InteropTypes.IO.Mvvm;

using AVLIMAGE = Avalonia.Media.IImage;
using XFILE = Microsoft.Extensions.FileProviders.IFileInfo;

namespace InteropTypes.IO.Controls
{
    /// <summary>
    /// An avalonia control that takes a <see cref="System.IO.FileInfo"/> or <see cref="XFILE"/> and displays it as an image thumbnail
    /// </summary>
    public class FileThumbnailBox : TemplatedControl , IFileThumbnailClient<AVLIMAGE>
    {
        #region lifecycle
        public FileThumbnailBox()
        {
            this.Template = new FuncControlTemplate<FileThumbnailBox>(_BuidControl);
        }        

        private static Panel _BuidControl(FileThumbnailBox target, INameScope scope)
        {
            var image = new Image();
            image.Stretch = Avalonia.Media.Stretch.Uniform;
            image.IsVisible = false;
            scope.Register("PART_Thumbnail", image);
            ToolTip.SetTip(image, new FileThumbnailToolTip());

            var wait = new WaitIcon();
            wait.MaxWidth = 40;
            wait.MaxHeight = 40;
            wait.Opacity = 0.2f;
            wait.HorizontalAlignment = HorizontalAlignment.Center;
            wait.VerticalAlignment = VerticalAlignment.Center;
            // wait IsVisible can bind to image.IsVisible here
            scope.Register("PART_Wait", wait);

            var panel = new Panel();
            panel.Children.Add(image);
            panel.Children.Add(wait);            

            return panel;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _Image = e.NameScope.Get<Image>("PART_Thumbnail");
            _Wait = e.NameScope.Get<Visual>("PART_Wait");

            _UpdateBitmap(Source);
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            _UpdateBitmap(Source);
        }

        #endregion

        #region data

        private Image _Image;        
        private Visual _Wait;

        public static IFileThumbnailServer<AVLIMAGE> ThumbnailFactory { get; set; } = _FileThumbnailFactory.Create().WrapWithQueue();

        #endregion

        #region properties

        public static readonly DirectProperty<FileThumbnailBox, System.IO.FileSystemInfo> FileSystemSourceProperty
            = AvaloniaProperty.RegisterDirect<FileThumbnailBox, System.IO.FileSystemInfo>(nameof(FileSystemSource), c => c.FileSystemSource, (c, v) => c.FileSystemSource = v);


        private System.IO.FileSystemInfo _FileSystemSource;

        public System.IO.FileSystemInfo FileSystemSource
        {
            get => _FileSystemSource;
            set
            {
                if (!this.SetAndRaise(FileSystemSourceProperty, ref _FileSystemSource, value)) return;

                Source = _FileSystemItem.Create(_FileSystemSource);
            }
        }

        public static readonly DirectProperty<FileThumbnailBox, XFILE> SourceProperty
            = AvaloniaProperty.RegisterDirect<FileThumbnailBox, XFILE>(nameof(Source), c => c.Source, (c, v) => c.Source = v);

        private XFILE _Source;

        public XFILE Source
        {
            get => _Source;
            set
            {
                if (!this.SetAndRaise(SourceProperty, ref _Source, value)) return;                
                _UpdateBitmap(_Source);
            }
        }

        public static readonly DirectProperty<FileThumbnailBox, AVLIMAGE> BitmapProperty
            = AvaloniaProperty.RegisterDirect<FileThumbnailBox, AVLIMAGE>(nameof(Bitmap), c => c.Bitmap);

        private AVLIMAGE _Bitmap;

        public AVLIMAGE Bitmap
        {
            get => _Bitmap;
            private set => this.SetAndRaise(BitmapProperty, ref _Bitmap, value);
        }

        #endregion

        #region API        

        XFILE IFileThumbnailClient<AVLIMAGE>.GetSource()
        {
            return Source;
        }

        void IFileThumbnailClient<AVLIMAGE>.SetImage(AVLIMAGE image)
        {
            _SetThumbnail(image);
        }

        private void _UpdateBitmap(XFILE src)
        {
            if (_Image == null) return;

            if (!this.IsLoaded) return;

            if (src == null)
            {                
                _Image.IsVisible = false;
                _Image.Source = null;
                _Wait.IsVisible = true;
                return;
            }

            if (_Bitmap != null) { _SetThumbnail(_Bitmap); return; }

            async Task _work()
            {
                await ThumbnailFactory?.LoadAndAssignImageAsync(this);
            }

            Task.Run(_work);
        }        

        private void _SetThumbnail(AVLIMAGE image)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {                
                _Image.Source = image;
                _Image.IsVisible = image != null;
                _Wait.IsVisible = !_Image.IsVisible;
                Bitmap = image;
            });
        }

        

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("{PhysicalPath}")]
    record _FileSystemItem : XFILE
    {
        public static _FileSystemItem Create(FileSystemInfo file)
        {
            if (file == null) return null;
            return new _FileSystemItem(file);
        }

        private _FileSystemItem(FileSystemInfo file)
        {
            _File = file;
        }

        private readonly System.IO.FileSystemInfo _File;        

        public Stream CreateReadStream()
        {
            return _File is System.IO.FileInfo finfo
                ? finfo.OpenRead()
                : throw new NotImplementedException();
        }

        public bool Exists => _File.Exists;

        public long Length => throw new NotImplementedException();

        public string PhysicalPath => _File.FullName;

        public string Name => _File.Name;

        public DateTimeOffset LastModified => _File.LastWriteTime;

        public bool IsDirectory => _File is System.IO.DirectoryInfo;
    }


    
}
