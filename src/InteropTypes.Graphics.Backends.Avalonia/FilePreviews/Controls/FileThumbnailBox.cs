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

using InteropTypes.Graphics;
using InteropTypes.IO;

using AVLIMAGE = Avalonia.Media.IImage;
using XFILE = Microsoft.Extensions.FileProviders.IFileInfo;

namespace InteropTypes.IO.Controls
{
    /// <summary>
    /// An avalonia control that takes a <see cref="System.IO.FileInfo"/> or <see cref="XFILE"/> and displays it as an image thumbnail
    /// </summary>
    public class FileThumbnailBox : TemplatedControl , IFileThumbnailBitmap
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

        public static IFileThumbnailFactory ThumbnailFactory { get; set; } = new _QueuedThumbnailFactory(new _FileThumbnailFactory());

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

        XFILE IFileThumbnailBitmap.GetSource()
        {
            return Source;
        }

        void IFileThumbnailBitmap.SetBitmap(AVLIMAGE image)
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
                await ThumbnailFactory?.LoadAndAssignThumbnailAsync(this);
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


    public interface IFileThumbnailBitmap
    {
        XFILE GetSource();
        void SetBitmap(AVLIMAGE image);
    }

    public interface IFileThumbnailFactory
    {
        public Task<bool> LoadAndAssignThumbnailAsync(IFileThumbnailBitmap item);
    }

    class _SemaphoreThumbnailFactory : IFileThumbnailFactory
    {
        public _SemaphoreThumbnailFactory(IFileThumbnailFactory baseFactory)
        {
            _BaseFactory = baseFactory;
        }

        private readonly IFileThumbnailFactory _BaseFactory;

        private static readonly SemaphoreSlim _Throttle = new SemaphoreSlim(1, 1);        

        public async Task<bool> LoadAndAssignThumbnailAsync(IFileThumbnailBitmap item)
        {
            try
            {
                await _Throttle.WaitAsync();
                return await _BaseFactory.LoadAndAssignThumbnailAsync(item);
            }
            finally
            {
                _Throttle.Release();
            }
        }
    }


    class _QueuedThumbnailFactory: IFileThumbnailFactory
    {
        #region lifecycle
        public _QueuedThumbnailFactory(IFileThumbnailFactory baseFactory)
        {
            _BaseFactory = baseFactory;
        }

        #endregion

        #region data

        private readonly IFileThumbnailFactory _BaseFactory;
        private readonly ConcurrentQueue<IFileThumbnailBitmap> jobs = new ConcurrentQueue<IFileThumbnailBitmap>();
        private readonly object lockObj = new object();

        private Task? workerTask;
        private CancellationTokenSource? cts;

        #endregion

        #region API

        public async Task<bool> LoadAndAssignThumbnailAsync(IFileThumbnailBitmap item)
        {            
            EnqueueAndRun(item);

            return true;
        }

        private void EnqueueAndRun(IFileThumbnailBitmap job)
        {
            jobs.Enqueue(job);

            lock (lockObj)
            {
                // Start worker if not already running
                if (workerTask == null || workerTask.IsCompleted)
                {
                    cts?.Dispose();
                    cts = new CancellationTokenSource();
                    workerTask = ProcessJobsAsync(cts.Token);
                }
            }
        }

        private async Task ProcessJobsAsync(CancellationToken cancellationToken)
        {
            bool firstTry = true;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (jobs.TryDequeue(out var job))
                    {                        
                        await _BaseFactory.LoadAndAssignThumbnailAsync(job);
                        firstTry = true;
                    }
                    else
                    {
                        // No jobs left, wait a bit before checking again
                        if (firstTry) await Task.Delay(100, cancellationToken);
                        else break;

                        firstTry = false;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
            }
        }

        #endregion        
    }


    class _FileThumbnailFactory : _FileThumbnailFallbackFactory
    {
        // a stategy would be to request

        public FilePreviewOptions Options { get; set; }

        public override async Task<bool> LoadAndAssignThumbnailAsync(IFileThumbnailBitmap item)
        {
            ArgumentNullException.ThrowIfNull(item);            

            var info = item.GetSource();
            if (info == null) return false;
            
            if (!string.IsNullOrWhiteSpace(info.PhysicalPath))
            {
                switch (info.IsDirectory)
                {
                    case false: item.SetBitmap(GetThumbnail(new System.IO.FileInfo(info.PhysicalPath))); return true;
                    case true: item.SetBitmap(GetThumbnail(new System.IO.DirectoryInfo(info.PhysicalPath))); return true;
                }
            }

            return await base.LoadAndAssignThumbnailAsync(item);
        }

        public AVLIMAGE GetThumbnail(System.IO.FileSystemInfo info)
        {
            if (info is not System.IO.FileInfo finfo) return null;

            var bmp = FilePreviewFactory.GetPreviewOrDefault(finfo, Options);
            if (bmp == null) return null;

            return ConvertToBitmap(bmp);
        }

        private static Avalonia.Media.Imaging.Bitmap ConvertToBitmap(WindowsBitmap srcBmp)
        {
            if (srcBmp == null) return null;

            using (var m = srcBmp.OpenRead())
            {
                if (m == null) return null;
                var avlbmp = new Avalonia.Media.Imaging.Bitmap(m);

                if (avlbmp.PixelSize.Width != srcBmp.Width || avlbmp.PixelSize.Height != srcBmp.Height)
                {
                    avlbmp.Dispose();
                    return null;
                }                

                return avlbmp;
            }
        }        
    }


    class _FileThumbnailFallbackFactory : IFileThumbnailFactory
    {
        public virtual async Task<bool> LoadAndAssignThumbnailAsync(IFileThumbnailBitmap item)
        {
            ArgumentNullException.ThrowIfNull(item);

            var info = item.GetSource();
            if (info == null) return false;
            if (info.IsDirectory) return false;

            // ToDo: exit if is not image            

            try
            {
                using var s = info.CreateReadStream();
                if (s == null) return false;
                var img = Avalonia.Media.Imaging.Bitmap.DecodeToHeight(s, 256);
                item.SetBitmap(img);
                
            }
            catch (NullReferenceException) // this is the exception produced by DecodeToHeight when it does not have a codec
            {
                return false;
            }

            return true;
        }
    }
}
