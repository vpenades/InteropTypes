using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Vision
{    
    /// <summary>
    /// Combines a broad and a narrow detector.
    /// </summary>
    public class TrackingDetector : IInferenceContext<PointerBitmap, DetectedObject.Collection>
    {
        #region lifecycle
        public TrackingDetector(string keyword)
        {
            _ObjectFilter = keyword;
        }

        public TrackingDetector WithBroad(IInferenceContext<PointerBitmap, DetectedObject.Collection> broadDetector)
        {
            _BroadDetector = broadDetector;            
            return this;
        }

        public TrackingDetector WithNarrow(INarrowInference<PointerBitmap, DetectedObject.Collection> narrowDetector)
        {
            _NarrowDetector = narrowDetector;
            return this;
        }

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _BroadDetector, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _NarrowDetector, null)?.Dispose();
        }

        #endregion

        #region data

        private string _ObjectFilter;

        private IInferenceContext<PointerBitmap, DetectedObject.Collection> _BroadDetector;
        private INarrowInference<PointerBitmap, DetectedObject.Collection> _NarrowDetector;

        private float _BroadOutputScale;

        private readonly List<_TrackedObject> _BroadTracked = new List<_TrackedObject>();        
        private readonly DetectedObject.Collection _NarrowTracked = new DetectedObject.Collection();

        sealed class _TrackedObject
        {
            public _TrackedObject(DetectedFrame detection) { AddDetection(detection); }

            public DetectedFrame? _DetectionT0; // frame t-2
            public DetectedFrame? _DetectionT1; // frame t-1

            public void AddDetection(DetectedFrame d)
            {
                if (_DetectionT1.HasValue)
                {
                    if (_DetectionT1.Value.Time >= d.Time)
                    {
                        _DetectionT1 = d;
                        return;
                    }
                }

                _DetectionT0 = _DetectionT1;
                _DetectionT1 = d;
            }
            
            public Rectangle GetDetectionWindow(float upScale)
            {
                var r = _DetectionT0.HasValue
                    ?
                    DetectedFrame.GetNextDetectionWindow(_DetectionT0.Value, _DetectionT1.Value, upScale)
                    :
                    DetectedFrame.GetNextDetectionWindow(_DetectionT1.Value, upScale);

                return Rectangle.Round(r);
            }            
        }

        #endregion

        #region debug stats

        #if DEBUG

        public readonly Score.Accumulator _BroadScores = new Score.Accumulator();
        public readonly Score.Accumulator _NarrowScores = new Score.Accumulator();        

        #endif

        [Conditional("DEBUG")]
        private void _Stats_AddBroadScore(DateTime t, Score score)
        {
            #if DEBUG
            _BroadScores.Add(t,score);
            #endif
        }

        [Conditional("DEBUG")]
        private void _Stats_AddNarrowScore(DateTime t, Score score)
        {
            #if DEBUG
            _NarrowScores.Add(t,score);
            #endif
        }

        #endregion

        #region API

        public void Inference(DetectedObject.Collection result, InferenceInput<PointerBitmap> input, Rectangle? inputWindow = null)
        {
            // if we don't have any tracked face
            // or the tracked faces have a very low confidence,
            // try find new ones:

            // result.Clear();

            _Stats_AddNarrowScore(input.CaptureTime, Score.Zero);
            _Stats_AddBroadScore(input.CaptureTime, Score.Zero);

            if (_BroadTracked.Count == 0)
            {
                var seeds = new DetectedObject.Collection(new SizeF(input.Content.Width,input.Content.Height));                
                _BroadDetector.Inference(seeds, input, inputWindow);

                var tracked = seeds.Objects
                    .Where(item => item.Name == _ObjectFilter)
                    .Where(item => item.Score.IsValid)
                    .Select(item => new DetectedFrame(item.Rect, _BroadOutputScale, input.CaptureTime, item.Score))
                    .ToList();

                DetectedFrame.RemoveOverlapping(tracked);

                _BroadTracked.Clear();
                _BroadTracked.AddRange(tracked.Select(item => new _TrackedObject(item)));                

                foreach (var o in _BroadTracked.ToList())
                {
                    var r = o.GetDetectionWindow(1);
                    result.AddObject(r, o._DetectionT1.Value.Score, "Detected");

                    _Stats_AddBroadScore(input.CaptureTime, o._DetectionT1.Value.Score);
                }                
            }

            foreach (var o in _BroadTracked.ToList())
            {
                var broadRect = Rectangle.Round(_NarrowDetector.GetNextDetectionWindow(o._DetectionT1.Value, o._DetectionT0));

                _NarrowTracked.Clear();
                _NarrowTracked.SetFrameSize(input);
                _NarrowDetector.Inference(_NarrowTracked, input, broadRect);

                var item = _NarrowTracked
                    .Objects
                    .Where(ttt => ttt.Name == _ObjectFilter)
                    .Where(ttt => ttt.Score.IsValid)
                    .FirstOrDefault();

                if (item.Name == null || item.Area < 16) // tracking lost
                {
                    _BroadTracked.Remove(o);
                }
                else
                {
                    result.Add(_NarrowTracked, result.AddObject(broadRect, item.Score, "Window"));

                    // update the broad tracking window for this face
                    o.AddDetection(new DetectedFrame(item.Rect, input.CaptureTime, item.Score));                    
                }

                _Stats_AddNarrowScore(input.CaptureTime, item.Score);
            }
        }

        #endregion
    }
}
