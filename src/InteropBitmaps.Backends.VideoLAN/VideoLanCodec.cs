using System;
using System.Linq;
using System.Collections.Concurrent;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using LibVLCSharp.Shared;
using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Codecs
{
    // https://code.videolan.org/mfkl/libvlcsharp-samples/tree/master/PreviewThumbnailExtractor

    public class VideoLanCodecAsync
    {
        #region lifecycle

        public VideoLanCodecAsync(Uri uri)
        {
            _SourceURI = uri;

            Pitch = Align(Width * BytePerPixel);
            Lines = Align(Height);

            uint Align(uint size)
            {
                if (size % 32 == 0)
                {
                    return size;
                }

                return ((size / 32) + 1) * 32;// Align on the next multiple of 32
            }
        }

        #endregion

        #region data        

        private const uint Width = 320;
        private const uint Height = 180;

        private Uri _SourceURI;

        /// <summary>
        /// RGBA is used, so 4 byte per pixel, or 32 bits.
        /// </summary>
        private const uint BytePerPixel = 4;

        /// <summary>
        /// the number of bytes per "line"
        /// For performance reasons inside the core of VLC, it must be aligned to multiples of 32.
        /// </summary>
        private readonly uint Pitch;

        /// <summary>
        /// The number of lines in the buffer.
        /// For performance reasons inside the core of VLC, it must be aligned to multiples of 32.
        /// </summary>
        private readonly uint Lines;

        private MemoryMappedFile CurrentMappedFile;
        private MemoryMappedViewAccessor CurrentMappedViewAccessor;
        private readonly ConcurrentQueue<(MemoryMappedFile file, MemoryMappedViewAccessor accessor)> FilesToProcess = new ConcurrentQueue<(MemoryMappedFile file, MemoryMappedViewAccessor accessor)>();
        private long FrameCounter = 0;

        #endregion

        #region API

        

        // https://docs.microsoft.com/en-us/archive/msdn-magazine/2019/november/csharp-iterating-with-async-enumerables-in-csharp-8

        // new Uri("http://www.caminandes.com/download/03_caminandes_llamigos_1080p.mp4")

        public async Task DecodeAsync(Action<MemoryBitmap> frameOut)
        {
            // Load native libvlc library
            Core.Initialize();

            using (var libvlc = new LibVLC())
            using (var mediaPlayer = new MediaPlayer(libvlc))
            {
                // Listen to events
                var processingCancellationTokenSource = new CancellationTokenSource();
                mediaPlayer.Stopped += (s, e) => processingCancellationTokenSource.CancelAfter(1);
                mediaPlayer.EndReached += (s, e) => processingCancellationTokenSource.CancelAfter(1);

                // Create new media
                using var media = new Media(libvlc, _SourceURI);

                media.AddOption(":no-audio");
                // Set the size and format of the video here.
                mediaPlayer.SetVideoFormat("RV32", Width, Height, Pitch);
                mediaPlayer.SetVideoCallbacks(_Lock, null, _Display);

                // Start recording
                mediaPlayer.Play(media);

                // Waits for the processing to stop
                try
                {
                    await ProcessThumbnailsAsync(frameOut, processingCancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                { }

                Console.WriteLine("Done. Press any key to exit.");
                Console.ReadKey();
            }
        }

        private async Task ProcessThumbnailsAsync(Action<MemoryBitmap> frameOut, CancellationToken token)
        {
            var frameWidth = (int)(Pitch / BytePerPixel);
            var frameHeight = (int)Lines;
            var frameData = new Byte[frameWidth * frameHeight * 4];
            var frameBitmap = new MemoryBitmap(frameData, frameWidth, frameHeight, Pixel.BGRA32.Format);

            var frameNumber = 0;

            while (!token.IsCancellationRequested)
            {
                if (FilesToProcess.TryDequeue(out var file))
                {                    
                    using (var sourceStream = file.file.CreateViewStream())
                    {
                        sourceStream.Read(frameData,0, frameData.Length); // TODO, must read until full
                        frameOut(frameBitmap);
                    }

                    file.accessor.Dispose();
                    file.file.Dispose();
                    frameNumber++;
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), token);
                }
            }
        }

        private IntPtr _Lock(IntPtr opaque, IntPtr planes)
        {
            CurrentMappedFile = MemoryMappedFile.CreateNew(null, Pitch * Lines);
            CurrentMappedViewAccessor = CurrentMappedFile.CreateViewAccessor();
            Marshal.WriteIntPtr(planes, CurrentMappedViewAccessor.SafeMemoryMappedViewHandle.DangerousGetHandle());
            return IntPtr.Zero;
        }

        private void _Display(IntPtr opaque, IntPtr picture)
        {
            if (FrameCounter % 100 == 0)
            {
                FilesToProcess.Enqueue((CurrentMappedFile, CurrentMappedViewAccessor));
                CurrentMappedFile = null;
                CurrentMappedViewAccessor = null;
            }
            else
            {
                CurrentMappedViewAccessor.Dispose();
                CurrentMappedFile.Dispose();
                CurrentMappedFile = null;
                CurrentMappedViewAccessor = null;
            }
            FrameCounter++;
        }

        #endregion
    }
}
