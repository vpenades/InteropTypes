﻿// <auto-generated />
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace InteropTensors
{
    
        
    /// <summary>
    /// Represents the indices of a specific element within a tensor.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplayString(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct TensorIndices1 : IReadOnlyList<int>, IEquatable<TensorIndices1>
    {
        #region debug

        private string _GetDebuggerDisplayString()
        {
            return string.Join("×", this);
        }        

        #endregion

        #region lifecycle

        public static implicit operator TensorIndices1(ReadOnlySpan<int> indices)
        {
            return new TensorIndices1(indices);
        }        

                

        
        public TensorIndices1(int i0)
        {
             Index0 = i0;
            
        }

        public TensorIndices1(ReadOnlySpan<int> indices)
        {
            if (indices.Length != 1) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
            
        }

        public TensorIndices1(IReadOnlyList<int> indices)
        {
            if (indices.Count != 1) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
            
        }        

        #endregion

        #region data

        
        
         public int Index0;
        
        public override int GetHashCode()
        {
            int h=0;

             h ^= Index0.GetHashCode(); h *=17;
            
            return h;
        }

        public static bool AreEqual(in TensorIndices1 a, in TensorIndices1 b)
        {            
             if (a.Index0 != b.Index0) return false;
            
            return true;
        }

        public static bool operator ==(in TensorIndices1 a, in TensorIndices1 b) { return AreEqual(a,b); }

        public static bool operator !=(in TensorIndices1 a, in TensorIndices1 b) { return !AreEqual(a,b); }

        public bool Equals(TensorIndices1 other) { return AreEqual(this, other); }

        public override bool Equals(object obj) { return obj is TensorIndices1 other ? AreEqual(this, other) : false; }

        #endregion        

        #region API - List

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Count => 1;

        public int this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0: return Index0;
                                
                    default:throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        private IEnumerable<int> _Enumerate()
        {
            yield return Index0;
        }

        public IEnumerator<int> GetEnumerator() { return _Enumerate().GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return _Enumerate().GetEnumerator(); }

        public int[] ToArray() { return new int[] { Index0 }; }        

        #endregion

        #region API - Other

                

        
        #endregion
    }

        
    /// <summary>
    /// Represents the indices of a specific element within a tensor.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplayString(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct TensorIndices2 : IReadOnlyList<int>, IEquatable<TensorIndices2>
    {
        #region debug

        private string _GetDebuggerDisplayString()
        {
            return string.Join("×", this);
        }        

        #endregion

        #region lifecycle

        public static implicit operator TensorIndices2(ReadOnlySpan<int> indices)
        {
            return new TensorIndices2(indices);
        }        

        
        public static implicit operator TensorIndices2(in (int i0, int i1) indices)
        {
            return new TensorIndices2(indices);
        }

        public TensorIndices2(in (int i0, int i1) indices)
        {
             Index0 = indices.i0;
             Index1 = indices.i1;
            
        }

        
        public TensorIndices2(int i0, int i1)
        {
             Index0 = i0;
             Index1 = i1;
            
        }

        public TensorIndices2(ReadOnlySpan<int> indices)
        {
            if (indices.Length != 2) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
            
        }

        public TensorIndices2(IReadOnlyList<int> indices)
        {
            if (indices.Count != 2) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
            
        }        

        #endregion

        #region data

        
        public static readonly TensorIndices2 Invalid = (-1, -1);

        
         public int Index0;
         public int Index1;
        
        public override int GetHashCode()
        {
            int h=0;

             h ^= Index0.GetHashCode(); h *=17;
             h ^= Index1.GetHashCode(); h *=17;
            
            return h;
        }

        public static bool AreEqual(in TensorIndices2 a, in TensorIndices2 b)
        {            
             if (a.Index0 != b.Index0) return false;
             if (a.Index1 != b.Index1) return false;
            
            return true;
        }

        public static bool operator ==(in TensorIndices2 a, in TensorIndices2 b) { return AreEqual(a,b); }

        public static bool operator !=(in TensorIndices2 a, in TensorIndices2 b) { return !AreEqual(a,b); }

        public bool Equals(TensorIndices2 other) { return AreEqual(this, other); }

        public override bool Equals(object obj) { return obj is TensorIndices2 other ? AreEqual(this, other) : false; }

        #endregion        

        #region API - List

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Count => 2;

        public int this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0: return Index0;
                    case 1: return Index1;
                                
                    default:throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        private IEnumerable<int> _Enumerate()
        {
            yield return Index0;
            yield return Index1;
        }

        public IEnumerator<int> GetEnumerator() { return _Enumerate().GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return _Enumerate().GetEnumerator(); }

        public int[] ToArray() { return new int[] { Index0, Index1 }; }        

        #endregion

        #region API - Other

        
        public (int Index0, int Index1) ToValueTuple() { return (Index0, Index1); }

        
        #endregion
    }

        
    /// <summary>
    /// Represents the indices of a specific element within a tensor.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplayString(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct TensorIndices3 : IReadOnlyList<int>, IEquatable<TensorIndices3>
    {
        #region debug

        private string _GetDebuggerDisplayString()
        {
            return string.Join("×", this);
        }        

        #endregion

        #region lifecycle

        public static implicit operator TensorIndices3(ReadOnlySpan<int> indices)
        {
            return new TensorIndices3(indices);
        }        

        
        public static implicit operator TensorIndices3(in (int i0, int i1, int i2) indices)
        {
            return new TensorIndices3(indices);
        }

        public TensorIndices3(in (int i0, int i1, int i2) indices)
        {
             Index0 = indices.i0;
             Index1 = indices.i1;
             Index2 = indices.i2;
            
        }

        
        public TensorIndices3(int i0, int i1, int i2)
        {
             Index0 = i0;
             Index1 = i1;
             Index2 = i2;
            
        }

        public TensorIndices3(ReadOnlySpan<int> indices)
        {
            if (indices.Length != 3) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
            
        }

        public TensorIndices3(IReadOnlyList<int> indices)
        {
            if (indices.Count != 3) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
            
        }        

        #endregion

        #region data

        
        public static readonly TensorIndices3 Invalid = (-1, -1, -1);

        
         public int Index0;
         public int Index1;
         public int Index2;
        
        public override int GetHashCode()
        {
            int h=0;

             h ^= Index0.GetHashCode(); h *=17;
             h ^= Index1.GetHashCode(); h *=17;
             h ^= Index2.GetHashCode(); h *=17;
            
            return h;
        }

        public static bool AreEqual(in TensorIndices3 a, in TensorIndices3 b)
        {            
             if (a.Index0 != b.Index0) return false;
             if (a.Index1 != b.Index1) return false;
             if (a.Index2 != b.Index2) return false;
            
            return true;
        }

        public static bool operator ==(in TensorIndices3 a, in TensorIndices3 b) { return AreEqual(a,b); }

        public static bool operator !=(in TensorIndices3 a, in TensorIndices3 b) { return !AreEqual(a,b); }

        public bool Equals(TensorIndices3 other) { return AreEqual(this, other); }

        public override bool Equals(object obj) { return obj is TensorIndices3 other ? AreEqual(this, other) : false; }

        #endregion        

        #region API - List

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Count => 3;

        public int this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0: return Index0;
                    case 1: return Index1;
                    case 2: return Index2;
                                
                    default:throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        private IEnumerable<int> _Enumerate()
        {
            yield return Index0;
            yield return Index1;
            yield return Index2;
        }

        public IEnumerator<int> GetEnumerator() { return _Enumerate().GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return _Enumerate().GetEnumerator(); }

        public int[] ToArray() { return new int[] { Index0, Index1, Index2 }; }        

        #endregion

        #region API - Other

        
        public (int Index0, int Index1, int Index2) ToValueTuple() { return (Index0, Index1, Index2); }

        
        #endregion
    }

        
    /// <summary>
    /// Represents the indices of a specific element within a tensor.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplayString(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct TensorIndices4 : IReadOnlyList<int>, IEquatable<TensorIndices4>
    {
        #region debug

        private string _GetDebuggerDisplayString()
        {
            return string.Join("×", this);
        }        

        #endregion

        #region lifecycle

        public static implicit operator TensorIndices4(ReadOnlySpan<int> indices)
        {
            return new TensorIndices4(indices);
        }        

        
        public static implicit operator TensorIndices4(in (int i0, int i1, int i2, int i3) indices)
        {
            return new TensorIndices4(indices);
        }

        public TensorIndices4(in (int i0, int i1, int i2, int i3) indices)
        {
             Index0 = indices.i0;
             Index1 = indices.i1;
             Index2 = indices.i2;
             Index3 = indices.i3;
            
        }

        
        public TensorIndices4(int i0, int i1, int i2, int i3)
        {
             Index0 = i0;
             Index1 = i1;
             Index2 = i2;
             Index3 = i3;
            
        }

        public TensorIndices4(ReadOnlySpan<int> indices)
        {
            if (indices.Length != 4) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
             Index3 = indices[3];
            
        }

        public TensorIndices4(IReadOnlyList<int> indices)
        {
            if (indices.Count != 4) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
             Index3 = indices[3];
            
        }        

        #endregion

        #region data

        
        public static readonly TensorIndices4 Invalid = (-1, -1, -1, -1);

        
         public int Index0;
         public int Index1;
         public int Index2;
         public int Index3;
        
        public override int GetHashCode()
        {
            int h=0;

             h ^= Index0.GetHashCode(); h *=17;
             h ^= Index1.GetHashCode(); h *=17;
             h ^= Index2.GetHashCode(); h *=17;
             h ^= Index3.GetHashCode(); h *=17;
            
            return h;
        }

        public static bool AreEqual(in TensorIndices4 a, in TensorIndices4 b)
        {            
             if (a.Index0 != b.Index0) return false;
             if (a.Index1 != b.Index1) return false;
             if (a.Index2 != b.Index2) return false;
             if (a.Index3 != b.Index3) return false;
            
            return true;
        }

        public static bool operator ==(in TensorIndices4 a, in TensorIndices4 b) { return AreEqual(a,b); }

        public static bool operator !=(in TensorIndices4 a, in TensorIndices4 b) { return !AreEqual(a,b); }

        public bool Equals(TensorIndices4 other) { return AreEqual(this, other); }

        public override bool Equals(object obj) { return obj is TensorIndices4 other ? AreEqual(this, other) : false; }

        #endregion        

        #region API - List

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Count => 4;

        public int this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0: return Index0;
                    case 1: return Index1;
                    case 2: return Index2;
                    case 3: return Index3;
                                
                    default:throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        private IEnumerable<int> _Enumerate()
        {
            yield return Index0;
            yield return Index1;
            yield return Index2;
            yield return Index3;
        }

        public IEnumerator<int> GetEnumerator() { return _Enumerate().GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return _Enumerate().GetEnumerator(); }

        public int[] ToArray() { return new int[] { Index0, Index1, Index2, Index3 }; }        

        #endregion

        #region API - Other

        
        public (int Index0, int Index1, int Index2, int Index3) ToValueTuple() { return (Index0, Index1, Index2, Index3); }

        
        #endregion
    }

        
    /// <summary>
    /// Represents the indices of a specific element within a tensor.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplayString(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct TensorIndices5 : IReadOnlyList<int>, IEquatable<TensorIndices5>
    {
        #region debug

        private string _GetDebuggerDisplayString()
        {
            return string.Join("×", this);
        }        

        #endregion

        #region lifecycle

        public static implicit operator TensorIndices5(ReadOnlySpan<int> indices)
        {
            return new TensorIndices5(indices);
        }        

        
        public static implicit operator TensorIndices5(in (int i0, int i1, int i2, int i3, int i4) indices)
        {
            return new TensorIndices5(indices);
        }

        public TensorIndices5(in (int i0, int i1, int i2, int i3, int i4) indices)
        {
             Index0 = indices.i0;
             Index1 = indices.i1;
             Index2 = indices.i2;
             Index3 = indices.i3;
             Index4 = indices.i4;
            
        }

        
        public TensorIndices5(int i0, int i1, int i2, int i3, int i4)
        {
             Index0 = i0;
             Index1 = i1;
             Index2 = i2;
             Index3 = i3;
             Index4 = i4;
            
        }

        public TensorIndices5(ReadOnlySpan<int> indices)
        {
            if (indices.Length != 5) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
             Index3 = indices[3];
             Index4 = indices[4];
            
        }

        public TensorIndices5(IReadOnlyList<int> indices)
        {
            if (indices.Count != 5) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
             Index3 = indices[3];
             Index4 = indices[4];
            
        }        

        #endregion

        #region data

        
        public static readonly TensorIndices5 Invalid = (-1, -1, -1, -1, -1);

        
         public int Index0;
         public int Index1;
         public int Index2;
         public int Index3;
         public int Index4;
        
        public override int GetHashCode()
        {
            int h=0;

             h ^= Index0.GetHashCode(); h *=17;
             h ^= Index1.GetHashCode(); h *=17;
             h ^= Index2.GetHashCode(); h *=17;
             h ^= Index3.GetHashCode(); h *=17;
             h ^= Index4.GetHashCode(); h *=17;
            
            return h;
        }

        public static bool AreEqual(in TensorIndices5 a, in TensorIndices5 b)
        {            
             if (a.Index0 != b.Index0) return false;
             if (a.Index1 != b.Index1) return false;
             if (a.Index2 != b.Index2) return false;
             if (a.Index3 != b.Index3) return false;
             if (a.Index4 != b.Index4) return false;
            
            return true;
        }

        public static bool operator ==(in TensorIndices5 a, in TensorIndices5 b) { return AreEqual(a,b); }

        public static bool operator !=(in TensorIndices5 a, in TensorIndices5 b) { return !AreEqual(a,b); }

        public bool Equals(TensorIndices5 other) { return AreEqual(this, other); }

        public override bool Equals(object obj) { return obj is TensorIndices5 other ? AreEqual(this, other) : false; }

        #endregion        

        #region API - List

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Count => 5;

        public int this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0: return Index0;
                    case 1: return Index1;
                    case 2: return Index2;
                    case 3: return Index3;
                    case 4: return Index4;
                                
                    default:throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        private IEnumerable<int> _Enumerate()
        {
            yield return Index0;
            yield return Index1;
            yield return Index2;
            yield return Index3;
            yield return Index4;
        }

        public IEnumerator<int> GetEnumerator() { return _Enumerate().GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return _Enumerate().GetEnumerator(); }

        public int[] ToArray() { return new int[] { Index0, Index1, Index2, Index3, Index4 }; }        

        #endregion

        #region API - Other

        
        public (int Index0, int Index1, int Index2, int Index3, int Index4) ToValueTuple() { return (Index0, Index1, Index2, Index3, Index4); }

        
        #endregion
    }

        
    /// <summary>
    /// Represents the indices of a specific element within a tensor.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplayString(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct TensorIndices6 : IReadOnlyList<int>, IEquatable<TensorIndices6>
    {
        #region debug

        private string _GetDebuggerDisplayString()
        {
            return string.Join("×", this);
        }        

        #endregion

        #region lifecycle

        public static implicit operator TensorIndices6(ReadOnlySpan<int> indices)
        {
            return new TensorIndices6(indices);
        }        

        
        public static implicit operator TensorIndices6(in (int i0, int i1, int i2, int i3, int i4, int i5) indices)
        {
            return new TensorIndices6(indices);
        }

        public TensorIndices6(in (int i0, int i1, int i2, int i3, int i4, int i5) indices)
        {
             Index0 = indices.i0;
             Index1 = indices.i1;
             Index2 = indices.i2;
             Index3 = indices.i3;
             Index4 = indices.i4;
             Index5 = indices.i5;
            
        }

        
        public TensorIndices6(int i0, int i1, int i2, int i3, int i4, int i5)
        {
             Index0 = i0;
             Index1 = i1;
             Index2 = i2;
             Index3 = i3;
             Index4 = i4;
             Index5 = i5;
            
        }

        public TensorIndices6(ReadOnlySpan<int> indices)
        {
            if (indices.Length != 6) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
             Index3 = indices[3];
             Index4 = indices[4];
             Index5 = indices[5];
            
        }

        public TensorIndices6(IReadOnlyList<int> indices)
        {
            if (indices.Count != 6) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
             Index3 = indices[3];
             Index4 = indices[4];
             Index5 = indices[5];
            
        }        

        #endregion

        #region data

        
        public static readonly TensorIndices6 Invalid = (-1, -1, -1, -1, -1, -1);

        
         public int Index0;
         public int Index1;
         public int Index2;
         public int Index3;
         public int Index4;
         public int Index5;
        
        public override int GetHashCode()
        {
            int h=0;

             h ^= Index0.GetHashCode(); h *=17;
             h ^= Index1.GetHashCode(); h *=17;
             h ^= Index2.GetHashCode(); h *=17;
             h ^= Index3.GetHashCode(); h *=17;
             h ^= Index4.GetHashCode(); h *=17;
             h ^= Index5.GetHashCode(); h *=17;
            
            return h;
        }

        public static bool AreEqual(in TensorIndices6 a, in TensorIndices6 b)
        {            
             if (a.Index0 != b.Index0) return false;
             if (a.Index1 != b.Index1) return false;
             if (a.Index2 != b.Index2) return false;
             if (a.Index3 != b.Index3) return false;
             if (a.Index4 != b.Index4) return false;
             if (a.Index5 != b.Index5) return false;
            
            return true;
        }

        public static bool operator ==(in TensorIndices6 a, in TensorIndices6 b) { return AreEqual(a,b); }

        public static bool operator !=(in TensorIndices6 a, in TensorIndices6 b) { return !AreEqual(a,b); }

        public bool Equals(TensorIndices6 other) { return AreEqual(this, other); }

        public override bool Equals(object obj) { return obj is TensorIndices6 other ? AreEqual(this, other) : false; }

        #endregion        

        #region API - List

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Count => 6;

        public int this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0: return Index0;
                    case 1: return Index1;
                    case 2: return Index2;
                    case 3: return Index3;
                    case 4: return Index4;
                    case 5: return Index5;
                                
                    default:throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        private IEnumerable<int> _Enumerate()
        {
            yield return Index0;
            yield return Index1;
            yield return Index2;
            yield return Index3;
            yield return Index4;
            yield return Index5;
        }

        public IEnumerator<int> GetEnumerator() { return _Enumerate().GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return _Enumerate().GetEnumerator(); }

        public int[] ToArray() { return new int[] { Index0, Index1, Index2, Index3, Index4, Index5 }; }        

        #endregion

        #region API - Other

        
        public (int Index0, int Index1, int Index2, int Index3, int Index4, int Index5) ToValueTuple() { return (Index0, Index1, Index2, Index3, Index4, Index5); }

        
        #endregion
    }

        
    /// <summary>
    /// Represents the indices of a specific element within a tensor.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplayString(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct TensorIndices7 : IReadOnlyList<int>, IEquatable<TensorIndices7>
    {
        #region debug

        private string _GetDebuggerDisplayString()
        {
            return string.Join("×", this);
        }        

        #endregion

        #region lifecycle

        public static implicit operator TensorIndices7(ReadOnlySpan<int> indices)
        {
            return new TensorIndices7(indices);
        }        

        
        public static implicit operator TensorIndices7(in (int i0, int i1, int i2, int i3, int i4, int i5, int i6) indices)
        {
            return new TensorIndices7(indices);
        }

        public TensorIndices7(in (int i0, int i1, int i2, int i3, int i4, int i5, int i6) indices)
        {
             Index0 = indices.i0;
             Index1 = indices.i1;
             Index2 = indices.i2;
             Index3 = indices.i3;
             Index4 = indices.i4;
             Index5 = indices.i5;
             Index6 = indices.i6;
            
        }

        
        public TensorIndices7(int i0, int i1, int i2, int i3, int i4, int i5, int i6)
        {
             Index0 = i0;
             Index1 = i1;
             Index2 = i2;
             Index3 = i3;
             Index4 = i4;
             Index5 = i5;
             Index6 = i6;
            
        }

        public TensorIndices7(ReadOnlySpan<int> indices)
        {
            if (indices.Length != 7) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
             Index3 = indices[3];
             Index4 = indices[4];
             Index5 = indices[5];
             Index6 = indices[6];
            
        }

        public TensorIndices7(IReadOnlyList<int> indices)
        {
            if (indices.Count != 7) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
             Index3 = indices[3];
             Index4 = indices[4];
             Index5 = indices[5];
             Index6 = indices[6];
            
        }        

        #endregion

        #region data

        
        public static readonly TensorIndices7 Invalid = (-1, -1, -1, -1, -1, -1, -1);

        
         public int Index0;
         public int Index1;
         public int Index2;
         public int Index3;
         public int Index4;
         public int Index5;
         public int Index6;
        
        public override int GetHashCode()
        {
            int h=0;

             h ^= Index0.GetHashCode(); h *=17;
             h ^= Index1.GetHashCode(); h *=17;
             h ^= Index2.GetHashCode(); h *=17;
             h ^= Index3.GetHashCode(); h *=17;
             h ^= Index4.GetHashCode(); h *=17;
             h ^= Index5.GetHashCode(); h *=17;
             h ^= Index6.GetHashCode(); h *=17;
            
            return h;
        }

        public static bool AreEqual(in TensorIndices7 a, in TensorIndices7 b)
        {            
             if (a.Index0 != b.Index0) return false;
             if (a.Index1 != b.Index1) return false;
             if (a.Index2 != b.Index2) return false;
             if (a.Index3 != b.Index3) return false;
             if (a.Index4 != b.Index4) return false;
             if (a.Index5 != b.Index5) return false;
             if (a.Index6 != b.Index6) return false;
            
            return true;
        }

        public static bool operator ==(in TensorIndices7 a, in TensorIndices7 b) { return AreEqual(a,b); }

        public static bool operator !=(in TensorIndices7 a, in TensorIndices7 b) { return !AreEqual(a,b); }

        public bool Equals(TensorIndices7 other) { return AreEqual(this, other); }

        public override bool Equals(object obj) { return obj is TensorIndices7 other ? AreEqual(this, other) : false; }

        #endregion        

        #region API - List

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Count => 7;

        public int this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0: return Index0;
                    case 1: return Index1;
                    case 2: return Index2;
                    case 3: return Index3;
                    case 4: return Index4;
                    case 5: return Index5;
                    case 6: return Index6;
                                
                    default:throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        private IEnumerable<int> _Enumerate()
        {
            yield return Index0;
            yield return Index1;
            yield return Index2;
            yield return Index3;
            yield return Index4;
            yield return Index5;
            yield return Index6;
        }

        public IEnumerator<int> GetEnumerator() { return _Enumerate().GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return _Enumerate().GetEnumerator(); }

        public int[] ToArray() { return new int[] { Index0, Index1, Index2, Index3, Index4, Index5, Index6 }; }        

        #endregion

        #region API - Other

        
        public (int Index0, int Index1, int Index2, int Index3, int Index4, int Index5, int Index6) ToValueTuple() { return (Index0, Index1, Index2, Index3, Index4, Index5, Index6); }

        
        #endregion
    }

        
    /// <summary>
    /// Represents the indices of a specific element within a tensor.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplayString(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct TensorIndices8 : IReadOnlyList<int>, IEquatable<TensorIndices8>
    {
        #region debug

        private string _GetDebuggerDisplayString()
        {
            return string.Join("×", this);
        }        

        #endregion

        #region lifecycle

        public static implicit operator TensorIndices8(ReadOnlySpan<int> indices)
        {
            return new TensorIndices8(indices);
        }        

        
        public static implicit operator TensorIndices8(in (int i0, int i1, int i2, int i3, int i4, int i5, int i6, int i7) indices)
        {
            return new TensorIndices8(indices);
        }

        public TensorIndices8(in (int i0, int i1, int i2, int i3, int i4, int i5, int i6, int i7) indices)
        {
             Index0 = indices.i0;
             Index1 = indices.i1;
             Index2 = indices.i2;
             Index3 = indices.i3;
             Index4 = indices.i4;
             Index5 = indices.i5;
             Index6 = indices.i6;
             Index7 = indices.i7;
            
        }

        
        public TensorIndices8(int i0, int i1, int i2, int i3, int i4, int i5, int i6, int i7)
        {
             Index0 = i0;
             Index1 = i1;
             Index2 = i2;
             Index3 = i3;
             Index4 = i4;
             Index5 = i5;
             Index6 = i6;
             Index7 = i7;
            
        }

        public TensorIndices8(ReadOnlySpan<int> indices)
        {
            if (indices.Length != 8) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
             Index3 = indices[3];
             Index4 = indices[4];
             Index5 = indices[5];
             Index6 = indices[6];
             Index7 = indices[7];
            
        }

        public TensorIndices8(IReadOnlyList<int> indices)
        {
            if (indices.Count != 8) throw new ArgumentOutOfRangeException(nameof(indices));

             Index0 = indices[0];
             Index1 = indices[1];
             Index2 = indices[2];
             Index3 = indices[3];
             Index4 = indices[4];
             Index5 = indices[5];
             Index6 = indices[6];
             Index7 = indices[7];
            
        }        

        #endregion

        #region data

        
        public static readonly TensorIndices8 Invalid = (-1, -1, -1, -1, -1, -1, -1, -1);

        
         public int Index0;
         public int Index1;
         public int Index2;
         public int Index3;
         public int Index4;
         public int Index5;
         public int Index6;
         public int Index7;
        
        public override int GetHashCode()
        {
            int h=0;

             h ^= Index0.GetHashCode(); h *=17;
             h ^= Index1.GetHashCode(); h *=17;
             h ^= Index2.GetHashCode(); h *=17;
             h ^= Index3.GetHashCode(); h *=17;
             h ^= Index4.GetHashCode(); h *=17;
             h ^= Index5.GetHashCode(); h *=17;
             h ^= Index6.GetHashCode(); h *=17;
             h ^= Index7.GetHashCode(); h *=17;
            
            return h;
        }

        public static bool AreEqual(in TensorIndices8 a, in TensorIndices8 b)
        {            
             if (a.Index0 != b.Index0) return false;
             if (a.Index1 != b.Index1) return false;
             if (a.Index2 != b.Index2) return false;
             if (a.Index3 != b.Index3) return false;
             if (a.Index4 != b.Index4) return false;
             if (a.Index5 != b.Index5) return false;
             if (a.Index6 != b.Index6) return false;
             if (a.Index7 != b.Index7) return false;
            
            return true;
        }

        public static bool operator ==(in TensorIndices8 a, in TensorIndices8 b) { return AreEqual(a,b); }

        public static bool operator !=(in TensorIndices8 a, in TensorIndices8 b) { return !AreEqual(a,b); }

        public bool Equals(TensorIndices8 other) { return AreEqual(this, other); }

        public override bool Equals(object obj) { return obj is TensorIndices8 other ? AreEqual(this, other) : false; }

        #endregion        

        #region API - List

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        public int Count => 8;

        public int this[int index]
        {
            get
            {
                switch(index)
                {
                    case 0: return Index0;
                    case 1: return Index1;
                    case 2: return Index2;
                    case 3: return Index3;
                    case 4: return Index4;
                    case 5: return Index5;
                    case 6: return Index6;
                    case 7: return Index7;
                                
                    default:throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        private IEnumerable<int> _Enumerate()
        {
            yield return Index0;
            yield return Index1;
            yield return Index2;
            yield return Index3;
            yield return Index4;
            yield return Index5;
            yield return Index6;
            yield return Index7;
        }

        public IEnumerator<int> GetEnumerator() { return _Enumerate().GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return _Enumerate().GetEnumerator(); }

        public int[] ToArray() { return new int[] { Index0, Index1, Index2, Index3, Index4, Index5, Index6, Index7 }; }        

        #endregion

        #region API - Other

        
        public (int Index0, int Index1, int Index2, int Index3, int Index4, int Index5, int Index6, int Index7) ToValueTuple() { return (Index0, Index1, Index2, Index3, Index4, Index5, Index6, Index7); }

        
        #endregion
    }

    }