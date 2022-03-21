using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Vision.IO
{
    /// <summary>
    /// Represents the information of a capture device object.
    /// </summary>
    /// <remarks>
    /// Must be implemented by classed derived from <see cref="VideoCaptureDevice"/>.
    /// </remarks>
    public interface ICaptureDeviceInfo
    {
        /// <summary>
        /// Represents the name of the device, or the image source.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Represents the time at which the device has started capturing
        /// </summary>
        DateTime CaptureStart { get; }        
    }

    readonly struct _CaptureDeviceInfo : ICaptureDeviceInfo
    {
        public _CaptureDeviceInfo(string name, DateTime time)
        {
            _Name = name;
            _Time = time;
        }

        private readonly string _Name;
        private readonly DateTime _Time;

        public string Name => _Name;

        public DateTime CaptureStart => _Time;
    }

    // capturedevicemirroring  Horizontal Vertical
    // capturedevice orientation

    // interface ICameraDeviceIntrinsics {}
}
