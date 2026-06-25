using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

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
    using BMPSERVER = IFileThumbnailServer<AVLIMAGE>;

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

        private void _UpdateView()
        {
            if (_ThumbnailView == null) return;

            var image = _Thumbnail;

            _ThumbnailView.Source = image;
            _ThumbnailView.IsVisible = image != null;
            if (_LoadingView != null) _LoadingView.IsVisible = image == null;
        }

        private Image _ThumbnailView;
        private Visual _LoadingView;

        #endregion

        #region data        

        private System.IO.FileSystemInfo _FileSystemSource;
        private XFILE _FileSource;        

        private BMPSERVER _ThumbnailFactory = _FileThumbnailFactory.Create().WrapWithSemaphore();
        private AVLIMAGE _Thumbnail;
        private ICommand _ThumbnailChangedCommand;

        private Func<XFILE, Object> _PreviewContentEvaluator;

        private Func<XFILE, Object> _PreviewInfoEvaluator;
        private Lazy<Object> _EvaluatedPreviewInfo;

        #endregion        

        #region properties        

        public static readonly DirectProperty<FileThumbnailBox, System.IO.FileSystemInfo> FileSystemSourceProperty
            = AvaloniaProperty.RegisterDirect<FileThumbnailBox, System.IO.FileSystemInfo>(nameof(FileSystemSource), c => c.FileSystemSource, (c, v) => c.FileSystemSource = v);        

        /// <summary>
        /// Gets or sets the source object from where the thumbnail will be retrieved
        /// </summary>
        /// <remarks>
        /// Changing this source will trigger an asynchronous operation that will update <see cref="Thumbnail"/>
        /// </remarks>
        public System.IO.FileSystemInfo FileSystemSource
        {
            get => _FileSystemSource;
            set
            {
                if (!this.SetAndRaise(FileSystemSourceProperty, ref _FileSystemSource, value)) return;

                FileSource = _FileSystemItem.Create(_FileSystemSource);
            }
        }

        public static readonly DirectProperty<FileThumbnailBox, XFILE> FileSourceProperty
            = AvaloniaProperty.RegisterDirect<FileThumbnailBox, XFILE>(nameof(FileSource), c => c.FileSource, (c, v) => c.FileSource = v);        

        /// <summary>
        /// Gets or sets the source object from where the thumbnail will be retrieved
        /// </summary>
        /// <remarks>
        /// Changing this source will trigger an asynchronous operation that will update <see cref="Thumbnail"/>
        /// </remarks>
        public XFILE FileSource
        {
            get => _FileSource;
            set
            {
                if (!this.SetAndRaise(FileSourceProperty, ref _FileSource, value)) return;

                _EvaluatedPreviewInfo = new Lazy<object>(EvaluatePreviewInfo);

                // Collection virtualization reuses containers,
                // so we must clear the cached image if datacontext changed.
                Thumbnail = null;

                // trigger image update, may request a thumbnail from windows cache or load the file itself if required.
                _UpdateImage();
            }
        }

        

        public static readonly DirectProperty<FileThumbnailBox, BMPSERVER> FileThumbnailFactoryProperty
            = AvaloniaProperty.RegisterDirect<FileThumbnailBox, BMPSERVER>(nameof(FileThumbnailFactory), c => c.FileThumbnailFactory, (c, v) => c.FileThumbnailFactory = v);

        /// <summary>
        /// When set, it overrides <see cref="_DefaultImageFactory"/>
        /// </summary>
        public BMPSERVER FileThumbnailFactory
        {
            get => _ThumbnailFactory;
            set
            {
                if (this.SetAndRaise(FileThumbnailFactoryProperty, ref _ThumbnailFactory, value))
                {
                    _UpdateImage();
                }
            }
        }

        public static readonly DirectProperty<FileThumbnailBox, AVLIMAGE> ThumbnailProperty
            = AvaloniaProperty.RegisterDirect<FileThumbnailBox, AVLIMAGE>(nameof(Thumbnail), c => c.Thumbnail);        

        /// <summary>
        /// Gets the image currently set as the thumbnail
        /// </summary>
        public AVLIMAGE Thumbnail
        {
            get => _Thumbnail;
            private set
            {
                if (this.SetAndRaise(ThumbnailProperty, ref _Thumbnail, value))
                {
                    _UpdateView();
                }
            }
        }

        public static readonly DirectProperty<FileThumbnailBox, ICommand> ThumbnailChangedCommandProperty
            = AvaloniaProperty.RegisterDirect<FileThumbnailBox, ICommand>(nameof(ThumbnailChangedCommand), c => c.ThumbnailChangedCommand, (c,v) => c.ThumbnailChangedCommand = v);        

        /// <summary>
        /// A command that is automatically executed when <see cref="Thumbnail"/> changes.
        /// </summary>
        /// <remarks>
        /// this can be used to brifly call the MVVM to let it analyze the bitmap. ToDo: Do not pass a AVLBITMAP, but a ITensorBitmapSource
        /// </remarks>
        public ICommand ThumbnailChangedCommand
        {
            get => _ThumbnailChangedCommand;
            set
            {
                if (this.SetAndRaise(ThumbnailChangedCommandProperty, ref _ThumbnailChangedCommand, value))
                {
                    RaiseImageChanged(_Thumbnail);
                }
            }
        }

        public static readonly DirectProperty<FileThumbnailBox, Func<XFILE, Object>> PreviewContentEvaluatorProperty
            = AvaloniaProperty.RegisterDirect<FileThumbnailBox, Func<XFILE, Object>>(nameof(PreviewContentEvaluator), c => c.PreviewContentEvaluator, (c, v) => c.PreviewContentEvaluator = v);

        /// <summary>
        /// Sets the content info evaluator lambda that will be called when hovering the cursor over the thumbnail for extra information.
        /// </summary>
        public Func<XFILE, Object> PreviewContentEvaluator
        {
            get => _PreviewInfoEvaluator;
            set => SetAndRaise(PreviewContentEvaluatorProperty, ref _PreviewContentEvaluator, value);
        }

        public static readonly DirectProperty<FileThumbnailBox, Func<XFILE, Object>> PreviewInfoEvaluatorProperty
            = AvaloniaProperty.RegisterDirect<FileThumbnailBox, Func<XFILE, Object>>(nameof(PreviewInfoEvaluator), c => c.PreviewInfoEvaluator, (c, v) => c.PreviewInfoEvaluator = v);

        /// <summary>
        /// Sets the file info evaluator lambda that will be called when hovering the cursor over the thumbnail for extra information.
        /// </summary>
        public Func<XFILE, Object> PreviewInfoEvaluator
        {
            get => _PreviewInfoEvaluator;
            set => SetAndRaise(PreviewInfoEvaluatorProperty, ref _PreviewInfoEvaluator, value);
        }

        #endregion

        #region API - Image        

        private void _UpdateImage()
        {
            // defaults

            if (_Thumbnail != null) return;            

            if (_FileSource == null) { this.Thumbnail = null; return; }

            // find factory

            var factory = _ThumbnailFactory;
            if (factory == null) return;            

            async Task _work()
            {
                await factory?.UpdateClientAsync(this);
            }

            Task.Run(_work);
        }
        
        XFILE IFileThumbnailClient<AVLIMAGE>.GetSource()
        {
            return FileSource;
        }

        void IFileThumbnailClient<AVLIMAGE>.SetImage(AVLIMAGE image)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                Thumbnail = image;
                RaiseImageChanged(image);
            });
        }

        protected virtual void RaiseImageChanged(AVLIMAGE image)
        {
            var cmd = ThumbnailChangedCommand;
            if (cmd == null) return;

            if (cmd.CanExecute(image)) cmd.Execute(image);
        }

        #endregion

        #region API - Preview
        [Bindable(true)]
        internal Object EvaluatedPreviewContent => EvaluatePreviewContent();

        protected virtual Object EvaluatePreviewContent()
        {
            var f = _FileSource;            

            if (f != null)
            {
                var eval = _PreviewContentEvaluator;
                var content = eval?.Invoke(f);
                if (content != null) return content;

                if (f.Name.EndsWith(".txt"))
                {
                    using(var s = f.CreateReadStream())
                    {
                        using var ss = new System.IO.StreamReader(s);
                        return ss.ReadToEnd();
                    }
                }
            }

            return this.Thumbnail;
        }

        [Bindable(true)]
        internal Object EvaluatedPreviewInfo => _EvaluatedPreviewInfo?.Value ?? null;        

        protected virtual Object EvaluatePreviewInfo()
        {
            var f = FileSource;
            if (f == null) return null;

            var def = $"Length: {f.Length}";

            var eval = _PreviewInfoEvaluator;
            if (eval != null) return eval(f) ?? def;

            return def;
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
