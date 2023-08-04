using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Drawing;

using XFORM = System.Numerics.Matrix3x2;
using COLOR = System.Drawing.Color;
using Avalonia.Metadata;
using System.Linq;

namespace InteropTypes
{
    public abstract class GroupBox : IDrawingBrush<ICanvas2D>
    {
        public static void ArrangeAtlas(int maxWidth, params GroupBox.Collection[] collections)
        {
            var boxes = collections.SelectMany(item => item.Boxes).ToArray();

            ArrangeAtlas(boxes, maxWidth);
        }

        public static void ArrangeAtlas(int maxWidth, params GroupBox[] boxes)
        {
            ArrangeAtlas(boxes, maxWidth);
        }

        public static void ArrangeAtlas(IEnumerable<GroupBox> boxes, int maxWidth)
        {
            var depth = new int[maxWidth];
 
            bool _probeEmpty(int x, int width) // probes a segment of depth to be empty
            {
                if (x > depth.Length - width) return false;

                var d = depth[x];

                for(int i=0; i < width; ++i)
                {
                    if (depth[x + i] > d) return false;
                }

                return true;
            }

            int _findSlot(int width)
            {
                int d = int.MaxValue;
                int x = 0;                

                for(int i=0; i < depth.Length - width; ++i)
                {
                    if (_probeEmpty(i, width) && d > depth[i]) { x = i; d = depth[i]; }
                }

                return x;
            }

            boxes = boxes.OrderByDescending(item => item.Width);

            foreach(var box in boxes)
            {
                var w = (int)(box.Width + 4);
                var h = (int)(box.Height + 4);
                var x = _findSlot(w);
                var y = depth[x];

                for(int i=0; i < w; ++i) depth[x+i] = y + h;

                box.HintX = x;
                box.HintY = y;
            }

        }

        protected GroupBox(float width, float height)
        {
            Width= width; Height = height;
        }

        public float HintX { get; set; }
        public float HintY { get; set; }

        public float Width { get; }
        public float Height { get; }

        private static readonly System.Diagnostics.Stopwatch _Timer = System.Diagnostics.Stopwatch.StartNew();

        public float ElapsedTime => (float)_Timer.Elapsed.TotalSeconds;

        public void DrawTo(ICanvas2D dc, float x, float y)
        {
            HintX = x;
            HintY = y;
            
            DrawTo(dc);
        }

        public void DrawTo(ICanvas2D dc)
        {
            dc = dc.CreateTransformed2D(XFORM.CreateTranslation(HintX, HintY));

            dc.DrawRectangle((0, 0), (Width, Height), (COLOR.Yellow, 3), 6);
            dc = dc.CreateTransformed2D(XFORM.CreateTranslation(10, 10));
            DrawContentTo(dc);
        }

        protected abstract void DrawContentTo(ICanvas2D dc);

        public abstract class Collection : IDrawingBrush<ICanvas2D>
        {
            protected Collection()
            {
                Initialize();
                GroupBox.ArrangeAtlas(_Boxes, 800);
            }

            protected abstract void Initialize();

            private readonly List<GroupBox> _Boxes = new List<GroupBox>();

            public IEnumerable<GroupBox> Boxes => _Boxes;

            private static readonly System.Diagnostics.Stopwatch _Timer = System.Diagnostics.Stopwatch.StartNew();

            public float ElapsedTime => (float)_Timer.Elapsed.TotalSeconds;


            protected void Register(GroupBox box)
            {
                if (box == null) return;
                _Boxes.Add(box);
            }

            protected void Render(float w, float h, Action<ICanvas2D> func)
            {
                w += 20;
                h += 20;

                var b = GroupBoxLambda.Create(w, h, func);
                Register(b);
            }

            public void DrawTo(ICanvas2D dc)
            {
                foreach (var box in _Boxes) box.DrawTo(dc);
            }
        }
    }

    internal sealed class GroupBoxLambda : GroupBox
    {
        public static GroupBox Create(float width, float height, Action<ICanvas2D> func)
        {
            return new GroupBoxLambda(width, height, func);
        }

        public GroupBoxLambda(float width, float height, Action<ICanvas2D> func) : base(width, height)
        {
            _action = func;
        }

        private readonly Action<ICanvas2D> _action;

        protected override void DrawContentTo(ICanvas2D dc)
        {
            _action.Invoke(dc);
        }
    }
}
