using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    [DefaultProperty(nameof(Bitmap))]
    [System.Diagnostics.DebuggerDisplay("{Info.ToDebuggerDisplayString(),nq}")]
    public class BindableBitmap : INotifyPropertyChanged, SpanBitmap.ISource
    {
        #region lifecycle

        public static implicit operator BindableBitmap(MemoryBitmap bmp) { return new BindableBitmap(bmp); }

        public BindableBitmap() { }

        public BindableBitmap(MemoryBitmap bmp)
        {
            _Bitmap = bmp;
        }

        #endregion

        #region data

        private MemoryBitmap _Bitmap;

        #endregion

        #region properties

        [Bindable(BindableSupport.Yes)]
        public MemoryBitmap Bitmap
        {
            get => _Bitmap;
            set
            {
                if (_Bitmap.Equals(value)) return;
                _Bitmap = value;
                Invalidate();
            }
        }

        [Bindable(BindableSupport.Yes)]
        public BitmapInfo Info => _Bitmap.Info;
        

        public event PropertyChangedEventHandler PropertyChanged;        

        private static readonly PropertyChangedEventArgs _AllProperties = new PropertyChangedEventArgs(null);
        private static readonly PropertyChangedEventArgs _InfoProperty = new PropertyChangedEventArgs(nameof(Info));
        private static readonly PropertyChangedEventArgs _BitmapProperty = new PropertyChangedEventArgs(nameof(Bitmap));

        #endregion

        #region API

        public void Update(MemoryBitmap bmp)
        {
            Update(bmp.AsSpanBitmap());
        }

        public void Update(SpanBitmap bmp)
        {
            bmp.CopyTo(ref _Bitmap);
            Invalidate();
        }       

        public virtual void Invalidate()
        {
            PropertyChanged?.Invoke(this, _AllProperties);            
            PropertyChanged?.Invoke(this, _InfoProperty);
            PropertyChanged?.Invoke(this, _BitmapProperty);         
        }

        public SpanBitmap AsSpanBitmap()
        {
            return Bitmap.AsSpanBitmap();
        }

        #endregion
    }
}
