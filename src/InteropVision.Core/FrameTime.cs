﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InteropVision
{
    /// <summary>
    /// Represents an instant in time, expressed as a Delta Time from a base time initialized at startup.
    /// </summary>
    /// <remarks>
    /// <see cref="FrameTime"/> is essentially the same as <see cref="DateTime"/>
    /// but with the precission of a <see cref="TimeSpan"/>
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{RelativeTime}")]
    public readonly struct FrameTime
    {
        #region static        

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static System.Diagnostics.Stopwatch _Timer;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static DateTime _BaseTimeUTC;        

        private static void _EnsureRunning()
        {
            if (_Timer != null) return;

            _Timer = new System.Diagnostics.Stopwatch();

            var a = DateTime.UtcNow;
            _Timer.Restart();
            var b = DateTime.UtcNow;

            _BaseTimeUTC = a + new TimeSpan((b - a).Ticks / 2);

            // remove two hours to prevent the extremely rare case of instancing this value
            // within the Saturday night hour when we switch from summer time to winter time,
            // where we "travel back in time one hour", which would cause the relative time
            // to be negative.
            _BaseTimeUTC -= TimeSpan.FromHours(2);
        }

        #endregion

        #region constructor

        public FrameTime(TimeSpan relative) { RelativeTime = relative; }

        public FrameTime(DateTime absolute)
        {
            _EnsureRunning();
            RelativeTime = absolute.ToUniversalTime() - _BaseTimeUTC;
        }

        #endregion

        #region operators

        public static FrameTime operator +(FrameTime a, TimeSpan b) { return new FrameTime(a.RelativeTime.Add(b)); }

        public static TimeSpan operator -(FrameTime a, FrameTime b) { return a.RelativeTime.Subtract(b.RelativeTime); }

        public static FrameTime Lerp(FrameTime a, FrameTime b, float amount)
        {
            var aa = a.RelativeTime.Ticks;
            var bb = b.RelativeTime.Ticks;

            var t = aa + (long)((float)(bb - aa) * amount);

            return new FrameTime(TimeSpan.FromTicks(t));
        }

        #endregion

        #region data

        /// <summary>
        /// Time relative to <see cref="BaseTimeUTC"/>
        /// </summary>
        public readonly TimeSpan RelativeTime;        

        /// <inheritdoc />        
        public override int GetHashCode() => RelativeTime.GetHashCode();

        #endregion

        #region properties

        /// <summary>
        /// Gets current the current time.        
        /// </summary>
        public static FrameTime Now
        {
            get
            {
                _EnsureRunning();
                return new FrameTime(_Timer.Elapsed);
            }
        }

        /// <summary>
        /// Time Origin, in UTC
        /// </summary>
        public static DateTime BaseTimeUTC
        {
            get
            {
                _EnsureRunning();
                return _BaseTimeUTC;
            }
        }

        /// <summary>
        /// Time in UTC
        /// </summary>
        public DateTime AbsoluteTimeUTC => BaseTimeUTC + RelativeTime;

        #endregion
    }
}
