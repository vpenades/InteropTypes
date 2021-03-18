using System;
using System.Collections.Generic;
using System.Text;

namespace InteropVision
{
    /// <summary>
    /// Represents an instant in time, expressed as a Delta Time from a base time initialized at startup.
    /// </summary>
    /// <remarks>
    /// <see cref="FrameTime"/> is essentially the same as <see cref="DateTime"/> but with the precission of a <see cref="TimeSpan"/>
    /// </remarks>
    [System.Diagnostics.DebuggerDisplay("{RelativeTime}")]
    public readonly struct FrameTime
    {
        #region static        

        public FrameTime(TimeSpan relative) { RelativeTime = relative; }        

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static System.Diagnostics.Stopwatch _Timer;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static DateTime _BaseTime;        

        private static void _EnsureRunning()
        {
            if (_Timer != null) return;

            _Timer = new System.Diagnostics.Stopwatch();

            var a = DateTime.UtcNow;
            _Timer.Restart();
            var b = DateTime.UtcNow;

            _BaseTime = a + new TimeSpan((b - a).Ticks / 2);
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

        public readonly TimeSpan RelativeTime;        
        public override int GetHashCode() => RelativeTime.GetHashCode();

        #endregion

        #region properties

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
        public static DateTime BaseTime
        {
            get
            {
                _EnsureRunning();
                return _BaseTime;
            }
        }

        /// <summary>
        /// Time in UTC
        /// </summary>
        public DateTime AbsoluteTime => BaseTime + RelativeTime;

        #endregion
    }
}
