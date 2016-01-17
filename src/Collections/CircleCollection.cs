using System.Collections.Generic;

namespace Metronome.Collections
{
    class CircleCollection<TData>
    {
        private readonly List<TData> _list = new List<TData>();
        private int _elementIndex;

        public void Add(TData element)
        {
            _list.Add(element);
        }

        public IEnumerable<TData> GetItems()
        {
            return _list;
        }

        public TData GetNext()
        {
            _elementIndex = (_elementIndex+1) % _list.Count;
            return _list[_elementIndex];
        }
    }
}
