using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.IO.Controls;
using InteropTypes.IO.Mvvm;

using AVLIMAGE = Avalonia.Media.IImage;

namespace InteropTypes.IO.Controls
{
    /// <summary>
    /// used to cache the thumbnails of file objects.
    /// </summary>
    /// <remarks>
    /// Declare in xaml as a resource, and then reference it with <see cref="FileThumbnailBox.FileThumbnailFactory"/>
    /// </remarks>
    public class FileThumbnailCache : FileThumbnailServerCache<AVLIMAGE>
    {
        public FileThumbnailCache()
            : base(new _FileThumbnailFactory()) { }
    }
}
