using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{    
    public class BindableScene3D : INotifyPropertyChanged, IDrawingBrush<IScene3D>
    {
        #region data

        private readonly Collection._ProducerConsumer<Record3D> _Queue = new Collection._ProducerConsumer<Record3D>();

        private Record3D _Scene;

        #endregion

        #region properties

        public event PropertyChangedEventHandler PropertyChanged;


        private static readonly PropertyChangedEventArgs _SceneProperty = new PropertyChangedEventArgs(nameof(Scene));

        [Bindable(BindableSupport.Yes)]
        public IScene3D Scene => _Scene;

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
        public Action Enqueue(IDrawingBrush<IScene3D> drawing)
        {
            if (drawing == null) return () => { };            

            _Queue.Produce(record => { record.Clear(); drawing.DrawTo(record); });

            void _update() { UpdateFromQueue(); }

            return _update;
        }

        public Action Enqueue(Action<IScene3D> contextAction)
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
        public void Update(IDrawingBrush<IScene3D> drawing)
        {
            if (_Scene == null) _Scene = new Record3D();

            _Scene.Clear();
            drawing?.DrawTo(_Scene);

            Invalidate();
        }

        /// <remarks>
        /// Always call from UI THREAD
        /// </remarks>
        public virtual void Invalidate()
        {
            PropertyChanged?.Invoke(this, _SceneProperty);
        }

        public void DrawTo(IScene3D context)
        {
            if (context == null) return;
            this._Scene?.DrawTo(context);
        }

        #endregion
    }    
}
