using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace InteropVision
{
    /// <summary>
    /// Represents the output score of an inference.
    /// </summary>    
    [System.Diagnostics.DebuggerDisplay("{IsValid?✅:❎} {Value}")]
    public readonly struct Score : IComparable<Score>
    {
        #region constructor

        public Score(bool isValid)
        {            
            IsValid = isValid;
            Value = IsValid ? 1 : 0;
        }

        public Score(float value, bool isValid)
        {            
            Value = value;
            IsValid = isValid;
        }

        #endregion

        #region operators

        public static implicit operator Score((float raw, bool valid) score) { return new Score(score.raw, score.valid); }

        public static bool operator <(Score a, Score b) => a.CompareTo(b) < 0;

        public static bool operator >(Score a, Score b) => a.CompareTo(b) > 0;

        #endregion

        #region data

        /// <summary>
        /// Arbitrary score value.
        /// </summary>
        /// <remarks>
        /// Most inference models return a "score" that can be used to determine whether the output result
        /// can be considered valid or not. The meaning of the score depends on each model and there's no
        /// standard, with some models using a Sigmoid value, while others using completely arbitrary scales.
        /// Also, the threshold for every score is also defined by every model. So this structure defines
        /// both the raw output score, and whether it passed the thresold.
        /// </remarks>
        public readonly float Value;

        /// <summary>
        /// A value indicating whether the score is considered valid or not.
        /// </summary>
        public readonly bool IsValid;

        public static readonly Score Zero = (0, false);

        public static readonly Score Ok = (1, true);

        #endregion

        #region properties

        /// <summary>
        /// Gets the sigmoid of <see cref="Value"/>.
        /// </summary>
        public float ValueSigmoid => Sigmoid(Value);

        #endregion

        #region API

        public int CompareTo(Score other) { return this.Value.CompareTo(other.Value); }

        #endregion

        #region static API

        public static float Sigmoid(float value) { return (float)(1 / (1 + Math.Exp(-value))); }
        public static double Sigmoid(double value) { return 1 / (1 + Math.Exp(-value)); }

        #endregion

        #region nested types

        /// <summary>
        /// Time Series Statistics helper
        /// </summary>
        public sealed class Accumulator
        {
            #region lifecycle

            public Accumulator() { }
            public Accumulator(string name) { _Name = name; }

            #endregion

            #region data

            private readonly string _Name;

            private readonly List<(float x,float y)> _Scores = new List<(float x, float y)>();

            #endregion

            #region properties

            public String Name => _Name;

            public IReadOnlyList<(float x, float y)> Scores => _Scores;

            #endregion

            #region API

            public void Add(FrameTime x, Score y)
            {
                Add((float)x.RelativeTime.TotalSeconds, y.Value);
            }

            public void Add(FrameTime x, float y)
            {
                Add((float)x.RelativeTime.TotalSeconds, y);
            }

            private void Add(float x, float y)
            {
                if (_Scores.Count > 0)
                {
                    var last = _Scores[_Scores.Count - 1];

                    if (x == last.x && y > last.y)
                    {
                        _Scores[_Scores.Count - 1] = (x, y);
                    }

                    if (x <= last.x) return;                    
                }

                _Scores.Add((x,y));
            }

            public (float min05, float min15, float median, float max85, float max95) Percentiles
            {
                get
                {
                    var yyy = _Scores
                        .Select(item => item.y)
                        .ToList();

                    if (yyy.Count == 0) return (0, 0, 0, 0, 0);                    

                    yyy.Sort();

                    var min05 = yyy[yyy.Count * 5 /100 ];
                    var min15 = yyy[yyy.Count * 15 / 100];
                    var median = yyy[yyy.Count * 50 / 100];
                    var max85 = yyy[yyy.Count * 85 / 100];
                    var max95 = yyy[yyy.Count * 95 / 100];

                    return (min05, min15, median, max85, max95);
                }
            }

            #endregion
        }

        #endregion
    }
}
