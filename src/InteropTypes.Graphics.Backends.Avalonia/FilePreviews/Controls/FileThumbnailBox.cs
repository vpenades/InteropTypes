using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
            scope.Register("PART_Loading", wait);

            var panel = new Panel();
            panel.Children.Add(image);
            panel.Children.Add(wait);            

            return panel;
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _ThumbnailView = e.NameScope.Get<Image>("PART_Thumbnail");
            _LoadingView = e.NameScope.Get<Visual>("PART_Loading");

            _UpdateView();
        }        

        #endregion

        #region data

        private Image _ThumbnailView;        
        private Visual _LoadingView;

        public static IFileThumbnailServer<AVLIMAGE> ThumbnailFactory { get; set; } = _FileThumbnailFactory.Create().WrapWithSemaphore();

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

                // Collection virtualization reuses containers,
                // so we must clear the cached image if datacontext changed.
                Image = null;

                // trigger image update, may request a thumbnail from windows cache or load the file itself if required.
                _UpdateImage();
            }
        }

        public static readonly DirectProperty<FileThumbnailBox, AVLIMAGE> ImageProperty
            = AvaloniaProperty.RegisterDirect<FileThumbnailBox, AVLIMAGE>(nameof(Image), c => c.Image);

        private AVLIMAGE _Image;

        public AVLIMAGE Image
        {
            get => _Image;
            private set
            {
                if (this.SetAndRaise(ImageProperty, ref _Image, value))
                {
                    _UpdateView();
                }
            }
        }        

        #endregion

        #region API        

        private void _UpdateImage()
        {
            // factory not set
            if (ThumbnailFactory == null) return;

            // processing

            if (_Source == null) { this.Image = null; return; }
            if (_Image != null) return;

            async Task _work()
            {
                await ThumbnailFactory?.LoadAndAssignImageAsync(this);
            }

            Task.Run(_work);
        }
        
        XFILE IFileThumbnailClient<AVLIMAGE>.GetSource()
        {
            return Source;
        }

        void IFileThumbnailClient<AVLIMAGE>.SetImage(AVLIMAGE image)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                Image = image;
            });
        }

        private void _UpdateView()
        {
            if (_ThumbnailView == null) return;

            _ThumbnailView.Source = _Image;
            _ThumbnailView.IsVisible = _Image != null;
            if (_LoadingView != null) _LoadingView.IsVisible = _Image == null;
        }

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("{PhysicalPath}")]
    record _FileSystemItem : XFILE
    {
        #region lifecycle

        public static _FileSystemItem Create(FileSystemInfo file)
        {
            if (file == null) return null;
            return new _FileSystemItem(file);
        }

        private _FileSystemItem(FileSystemInfo file)
        {
            _Entry = file;
        }

        #endregion

        #region data

        private readonly System.IO.FileSystemInfo _Entry;

        #endregion

        #region properties

        public bool Exists => _Entry.Exists;

        public long Length => _Entry is System.IO.FileInfo finfo ? finfo.Length : throw new NotImplementedException();

        public string PhysicalPath => _Entry.FullName;

        public string Name => _Entry.Name;

        public DateTimeOffset LastModified => _Entry.LastWriteTime;

        public bool IsDirectory => _Entry is not System.IO.FileInfo;

        #endregion

        #region API

        public Stream CreateReadStream()
        {
            return _Entry is System.IO.FileInfo finfo
                ? finfo.OpenRead()
                : throw new NotImplementedException();
        }

        #endregion
    }

}
