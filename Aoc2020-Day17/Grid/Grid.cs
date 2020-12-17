using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day17
{
    internal abstract class Grid
    {
        public const char ActiveState = '#';
        public const char InactiveState = '.';
    }

    internal abstract class Grid<TPosition> : Grid where TPosition : notnull
    {
        protected readonly IDictionary<TPosition, char> GridData = new Dictionary<TPosition, char>();

        private (TPosition min, TPosition max) _bounds;

        public (TPosition min, TPosition max) Bounds => _bounds;

        public bool IsActive(TPosition position)
            => Get(position) == ActiveState;

        public int CountActiveNeighbours(TPosition position)
            => GetAdjacentPositions(position).Count(IsActive);

        public int CountActivePositions()
            => GridData.Count(e => e.Value == ActiveState);

        public char Get(TPosition position)
        {
            return GridData.ContainsKey(position) ? GridData[position] : InactiveState;
        }

        public void Set(TPosition position, char state)
        {
            GridData[position] = state;

            var bounds = _bounds.Equals(default)
                             ? (min: position, max: position)
                             : _bounds;
            _bounds = AddToBounds(bounds, position);
        }

        public IEnumerable<TPosition> GetAllPotentialPositionsForNextCycle()
        {
            return GetPositionsInsideBounds(Expand(_bounds));
        }

        public IEnumerable<TPosition> GetAdjacentPositions(TPosition position)
            => GetPositionsInsideBounds(Expand((position, position))).Where(p => !EqualityComparer<TPosition>.Default.Equals(p, position));

        public IEnumerable<TPosition> GetAllPositions()
            => GetPositionsInsideBounds(Bounds);

        protected abstract (TPosition min, TPosition max) AddToBounds((TPosition min, TPosition max) bounds, TPosition position);

        protected abstract (TPosition min, TPosition max) Expand((TPosition min, TPosition max) bounds, int amount = 1);

        protected abstract IEnumerable<TPosition> GetPositionsInsideBounds((TPosition min, TPosition max) bounds);
    }
}
