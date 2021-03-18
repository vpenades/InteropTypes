using System;
using System.Collections.Generic;
using System.Text;

namespace InteropVision.IO
{
    [System.Diagnostics.DebuggerDisplay("Fps:{FrameRate}")]
    public class FrameRateCounter
    {
        #region static

        private static readonly System.Diagnostics.Stopwatch _Timer = System.Diagnostics.Stopwatch.StartNew();

        private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);

        #endregion

        #region data

        private System.Collections.Concurrent.ConcurrentQueue<TimeSpan> _Elapsed = new System.Collections.Concurrent.ConcurrentQueue<TimeSpan>();

        #endregion

        #region API

        public int FrameRate => _Elapsed.Count;

        private void _RemoveOld(TimeSpan curr)
        {
            while(_Elapsed.TryPeek(out TimeSpan last))            
            {                
                if (curr - last <= OneSecond) break;
                if (!_Elapsed.TryDequeue(out _)) break;
            }
        }

        public void AddFrame()
        {
            var t = _Timer.Elapsed;
            _Elapsed.Enqueue(t);

            _RemoveOld(t);
        }

        #endregion
    }
}
