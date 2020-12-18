namespace Aoc2020_Day18
{
    internal sealed class Number : MathsExpression
    {
        public Number(long value)
            => Value = value;
        public long Value { get; }
        public override long Evaluate() => Value;
        public override string ToString() => $"[Number {Value}]";
    }
}