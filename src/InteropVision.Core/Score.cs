using System;
using System.Collections.Generic;
using System.Linq;

namespace InteropVision
{
    /// <summary>
    /// Represents the output score of an inference.
    /// </summary>    
    [System.Diagnostics.DebuggerDisplay("{_ToDebuggerDisplayString(),nq}")]
    public readonly struct Score :
        IEquatable<Score>,
        IComparable<Score>,
        IComparable        
    {
        #region diagnostics

        internal string _ToDebuggerDisplayString()
        {
            var r = IsValid
                ? "✅"
                : "❎";

            return this.Type == ResultType.Sigmoid
                ? $"{r} {NormalizedValue}"
                : $"{r} {Value}";
        }

        #endregion

        #region constants

        public static readonly Score Zero = (0, false);

        public static readonly Score Ok = (1, true);

        #endregion

        #region constructor

        public static implicit operator Score((float value, bool valid) score)
        {
            return new Score(score.value, score.valid);
        }

        public static implicit operator Score((float value, ResultType result) score)
        {
            return new Score(score.value, score.result);
        }

        public Score(bool isValid)
        {            
            Type = isValid ? ResultType.Valid : ResultType.Invalid;
            Value = isValid ? 1 : 0;
        }

        public Score(float value, bool isValid)
        {
            Type = isValid ? ResultType.Valid : ResultType.Invalid;
            Value = value;            
        }

        public Score(float value, ResultType result)
        {
            if (float.IsNaN(value)) throw new ArgumentException("NaN", nameof(value));

            if (result == ResultType.Normalized)
            {
                if (value < 0 || value > 1) throw new ArgumentOutOfRangeException(nameof(value));
            }

            Type = result;
            Value = value;
        }

        #endregion

        #region data


        /// <summary>
        /// Defines how <see cref="Value"/> has to be interpreted.
        /// </summary>
        public readonly ResultType Type;        

        /// <summary>
        /// Arbitrary score value.
        /// </summary>        
        public readonly float Value;

        /// <inheritdoc />
        public override int GetHashCode() { return Type.GetHashCode() ^ Value.GetHashCode(); }

        /// <inheritdoc />
        public bool Equals(Score other) { return this.Type == other.Type && this.Value == other.Value; }

        /// <inheritdoc />
        public override bool Equals(object obj) { return obj is Score other && this.Equals(other); }

        /// <inheritdoc />
        public static bool operator ==(Score a, Score b) => a.Equals(b);

        /// <inheritdoc />
        public static bool operator !=(Score a, Score b) => !a.Equals(b);

        /// <inheritdoc />
        public int CompareTo(Score other)
        {
            return this.IsValid == other.IsValid
                ? this.Value.CompareTo(other.Value)
                : this.IsValid ? 1 : -1;
        }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            return obj is Score other
                ? this.CompareTo(other)
                : throw new ArgumentException("invalid type", nameof(obj));
        }

        /// <inheritdoc />
        public static bool operator <(Score a, Score b) => a.CompareTo(b) < 0;

        /// <inheritdoc />
        public static bool operator <=(Score a, Score b) => a.CompareTo(b) <= 0;

        /// <inheritdoc />
        public static bool operator >(Score a, Score b) => a.CompareTo(b) > 0;

        /// <inheritdoc />
        public static bool operator >=(Score a, Score b) => a.CompareTo(b) >= 0;

        #endregion

        #region properties

        public bool IsValid
        {
            get
            {
                switch(Type)
                {
                    case ResultType.Invalid: return false;
                    case ResultType.Valid: return true;
                    case ResultType.Normalized: return Value >= 0.5f;
                    case ResultType.Sigmoid: return Sigmoid(Value) >= 0.5f;
                    default: throw new NotImplementedException();                        
                }
            }
        }
        
        public float NormalizedValue
        {
            get
            {
                switch (Type)
                {
                    case ResultType.Invalid: return 0;
                    case ResultType.Valid: return 1;
                    case ResultType.Normalized: return Value;
                    case ResultType.Sigmoid: return Sigmoid(Value);
                    default: throw new NotImplementedException();
                }
            }
        }

        #endregion        

        #region static API

        public static float Sigmoid(float value) { return (float)(1 / (1 + Math.Exp(-value))); }
        public static double Sigmoid(double value) { return 1 / (1 + Math.Exp(-value)); }

        #endregion

        #region nested types

        public enum ResultType
        {
            /// <summary>
            /// Result is invalid regardless of the value
            /// </summary>
            Invalid,

            /// <summary>
            /// Score is valid regardless of the value
            /// </summary>
            Valid,

            /// <summary>
            /// Score is a value between 0 and 1, where 0.5 is the threshold between invalid/valid
            /// </summary>
            Normalized,

            /// <summary>
            /// Score is a sigmoid value.
            /// </summary>
            /// <remarks>
            /// negative values are rendered as invalid, 0 and positive values are rendered as valid.        
            /// </remarks>
            Sigmoid,

        }

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
            private DateTime? _BaseTime;

            private readonly List<(float x,float y)> _Scores = new List<(float x, float y)>();

            #endregion

            #region properties

            public String Name => _Name;

            public IReadOnlyList<(float x, float y)> Scores => _Scores;

            #endregion

            #region API

            public void Add(DateTime t, Score y)
            {
                Add(t, y.Value);
            }

            public void Add(DateTime t, float y)
            {
                if (!_BaseTime.HasValue) _BaseTime = t;

                var x = t - _BaseTime.Value;

                Add((float)x.TotalSeconds, y);
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
