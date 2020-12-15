namespace Aoc2020_Day15
{
    internal class RecentHistory<T>
    {
        private readonly int _capacity;
        private readonly T[] _buffer;
        private int _nextPosition;
        private int _count;

        public bool IsFull => _count == _capacity;

        public RecentHistory(int capacity, T[]? values = null)
        {
            _capacity = capacity;
            _nextPosition = 0;
            _count = 0;
            _buffer = new T[capacity];

            if (values == null) return;

            foreach (var value in values)
                Record(value);
        }

        public void Record(T entry)
        {
            _buffer[_nextPosition] = entry;
            if (_count < _capacity) _count++;
            _nextPosition = Modulo(_nextPosition + 1, _capacity);
        }

        public T Get(int position)
        {
            return _buffer[Modulo(_nextPosition - _count + position, _capacity)];
        }

        private static int Modulo(int value, int modulus)
        {
            var result = value % modulus;
            return result < 0 ? modulus + result : result;
        }
    }
}
