using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InteropTypes.Graphics.Drawing.Collection
{
    class _ProducerConsumer<T> where T : class, new()
    {
        #region data

        private readonly ConcurrentBag<T> _Pool = new ConcurrentBag<T>();

        private readonly ConcurrentQueue<T> _Queue = new ConcurrentQueue<T>();

        #endregion

        #region properties

        public bool IsEmpty => _Queue.IsEmpty;

        #endregion

        #region API

        public void Produce(Action<T> producer)
        {
            _Queue.Enqueue(_CopyFromPool(producer));

            while (_Queue.Count > 2) // throttle
            {
                if (_Queue.TryDequeue(out var item)) _ReturnToPool(item);
            }
        }

        public bool Consume(Action<T> consumer, int repeat = int.MaxValue)
        {
            while (repeat > 0)
            {
                if (!_Queue.TryDequeue(out var xbmp)) break;

                consumer(xbmp);
                _ReturnToPool(xbmp);
                --repeat;
            }

            return !_Queue.IsEmpty;
        }

        private T _CopyFromPool(Action<T> enqueuer)
        {
            if (!_Pool.TryTake(out T newItem)) newItem = default;

            if (newItem == null) newItem = new T();

            enqueuer(newItem);

            return newItem;
        }

        private void _ReturnToPool(T item)
        {
            if (item == null) return;

            _Pool.Add(item);
        }

        #endregion
    }
}
