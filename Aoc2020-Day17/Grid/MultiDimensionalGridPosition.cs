using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Aoc2020_Day17
{
    internal sealed class MultiDimensionalGridPosition : IEquatable<MultiDimensionalGridPosition>
    {
        public bool Equals(MultiDimensionalGridPosition? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Values.Length != other.Values.Length) return false;
            for (var i = 0; i < Values.Length; i++)
                if (Values[i] != other.Values[i])
                    return false;
            return true;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MultiDimensionalGridPosition) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            foreach (var value in Values)
                hashCode.Add(value);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(MultiDimensionalGridPosition? left, MultiDimensionalGridPosition? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MultiDimensionalGridPosition? left, MultiDimensionalGridPosition? right)
        {
            return !Equals(left, right);
        }

        public MultiDimensionalGridPosition(IEnumerable<long> values)
        {
            Values = values.ToImmutableArray();
        }

        public ImmutableArray<long> Values { get; }

        public override string ToString()
        {
            return $"({string.Join(", ", Values)})";
        }
    }
}
