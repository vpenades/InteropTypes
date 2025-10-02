using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InteropTypes.IO
{
    public class FilePreviewOptions
    {
        public static bool ShouldExtractAssociatedIcon(FILEINFO finfo, FilePreviewOptions options)
        {
            options ??= _Default;

            if (options.ExtensionAssociatedIcon) return true;

            var ext = finfo.Extension.TrimStart('.').ToLowerInvariant();

            switch(ext)
            {
                case "exe": return true;
                case "dll": return true;
                case "txt": return true;
                case "md": return true;
                case "xml": return true;
                case "json": return true;
                case "nfo": return true;
                case "bin": return true;
            }            

            return false;
        }

        internal static FilePreviewOptions _Default { get; } = new FilePreviewOptions();

        #region properties

        /// <summary>
        /// If set to true it will retrieve the icon associated with the extension
        /// </summary>
        public bool ExtensionAssociatedIcon { get; set; }

        public bool AllowBigger { get; set; } = true;
        public bool CachedOnly { get; set; } = false;

        public bool PrefferTransparency { get; set; } = false;

        public int Width { get; set; } = 128;

        public int Height { get; set; } = 128;

        #endregion

        #region helpers

        [SupportedOSPlatform("windows")]
        internal System.Drawing.Imaging.PixelFormat GetPixelFormat()
        {
            if (PrefferTransparency) return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            return System.Drawing.Imaging.PixelFormat.Format24bppRgb;
        }

        #endregion

        #region throttling

        public void SetSemaphore(int maxInstances, int timeout = int.MaxValue)
        {
            if (maxInstances <= 0) { _Semaphore = null; return; }

            _Semaphore = new SemaphoreSlim(maxInstances);
            _SemaphoreTimeout = timeout;
        }


        private SemaphoreSlim _Semaphore;
        private int _SemaphoreTimeout = int.MaxValue;

        internal static async Task<T> _ThrottleAsync<T>(FilePreviewOptions options, Func<T> function)
        {
            return await _ThrottleAsync(options, ()=> Task.Run(function));
        }

        internal static async Task<T> _ThrottleAsync<T>(FilePreviewOptions options, Func<Task<T>> function)
        {
            if (options == null) return await function.Invoke();
            return await options._ThrottleAsync(function);
        }

        private async Task<T> _ThrottleAsync<T>(Func<Task<T>> function)
        {
            if (_Semaphore == null) return await function.Invoke();

            if (!await _Semaphore.WaitAsync(_SemaphoreTimeout)) return default;

            try
            {
                return await function.Invoke();
            }
            finally
            {
                _Semaphore.Release();
            }
        }

        #endregion
    }
}
