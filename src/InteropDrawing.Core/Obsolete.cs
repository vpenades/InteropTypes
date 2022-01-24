using System;
using System.Collections.Generic;
using System.Text;

namespace InteropDrawing
{
    [Obsolete("Use ImageAsset", true)]
    public class SpriteAsset
    {
        #region lifecycle

        public static IEnumerable<SpriteAsset> CreateGrid(Object source, Point2 size, Point2 pivot, int count, int stride)
        {
            throw new Exception("Obsolete");
        }

        public static SpriteAsset CreateFromBitmap(Object source, Point2 size, Point2 pivot)
        {
            throw new Exception("Obsolete");
        }

        public SpriteAsset(Object source, Point2 origin, Point2 size, Point2 pivot)
        {
            throw new Exception("Obsolete");
        }

        public SpriteAsset() { }

        public SpriteAsset WithPivot(int x, int y)
        {
            throw new Exception("Obsolete");
        }

        public SpriteAsset WithScale(float scale)
        {
            throw new Exception("Obsolete");
        }

        public void CopyTo(SpriteAsset other, Point2 pivotOffset)
        {
            throw new Exception("Obsolete");
        }

        #endregion        
    }
}
