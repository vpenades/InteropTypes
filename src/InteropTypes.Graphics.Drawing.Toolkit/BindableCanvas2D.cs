using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    public class BindableCanvas2D : INotifyPropertyChanged, IDrawingBrush<ICanvas2D>
    {
        #region data

        private readonly Collection._ProducerConsumer<Record2D> _Queue = new Collection._ProducerConsumer<Record2D>();

        private Record2D _Canvas;

        #endregion

        #region properties

        public event PropertyChangedEventHandler PropertyChanged;


        private static readonly PropertyChangedEventArgs _CanvasProperty = new PropertyChangedEventArgs(nameof(Canvas));

        [Bindable(BindableSupport.Yes)]
        public ICanvas2D Canvas => _Canvas;

        #endregion

        #region API

        /// <summary>
        /// Updates the drawing from a thread other than the UI thread.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Under the hood, a copy of <paramref name="drawing"/> is created and enqueued, so the dispatcher can dequeue it.
        /// </para>
        /// <para>
        /// The returned action must be executed in the UI thread either by a dispatcher,
        /// or by the main loop in sequence.
        /// </para>        
        /// </remarks>
        /// <example>
        /// <code>
        /// System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke( bindable.Enqueue(bitmap) );
        /// </code>
        /// </example>
        /// <param name="drawing">The drawing to enqueue.</param>
        /// <returns>An action that needs to be executed by a dispatcher in the UI thread in order to complete the update.</returns>
        public Action Enqueue(IDrawingBrush<ICanvas2D> drawing)
        {
            if (drawing == null) return () => { };            

            _Queue.Produce(record => { record.Clear(); drawing.DrawTo(record); });

            void _update() { UpdateFromQueue(); }

            return _update;
        }

        public Action Enqueue(Action<ICanvas2D> contextAction)
        {
            if (contextAction == null) return () => { };

            _Queue.Produce(record => { record.Clear(); contextAction(record); });

            void _update() { UpdateFromQueue(); }

            return _update;
        }

        /// <summary>
        /// If we have enqueued a drawing from another thread, we can call this method from the UI thread to dequeue it.
        /// </summary>
        /// <remarks>
        /// Always call from UI THREAD
        /// </remarks>
        /// <returns>true if there's still more drawing into queue, false otherwise</returns>
        public bool UpdateFromQueue(int repeat = int.MaxValue)
        {
            return _Queue.Consume(record => Update(record), repeat);
        }

        /// <summary>
        /// Updates the underlaying drawing.
        /// </summary>
        /// <remarks>
        /// Always call from UI THREAD
        /// </remarks>
        /// <param name="drawing">the input image</param>            
        public void Update(IDrawingBrush<ICanvas2D> drawing)
        {
            if (_Canvas == null) _Canvas = new Record2D();

            _Canvas.Clear();
            drawing?.DrawTo(_Canvas);

            Invalidate();
        }

        /// <remarks>
        /// Always call from UI THREAD
        /// </remarks>
        public virtual void Invalidate()
        {
            PropertyChanged?.Invoke(this, _CanvasProperty);
        }

        public void DrawTo(ICanvas2D context)
        {
            if (context == null) return;
            this._Canvas?.DrawTo(context);
        }

        #endregion
    }    
}
