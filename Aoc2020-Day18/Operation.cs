namespace Aoc2020_Day18
{
    internal class Operation : MathsExpression
    {
        public Operation(MathsExpression left, MathsExpression right, char @operator)
            => (Left, Right, Operator) = (left, right, @operator);
        public MathsExpression Left { get; }
        public MathsExpression Right { get; }
        public char Operator { get; }

        public override long Evaluate()
            => Operator switch
               {
                   '+' => Left.Evaluate() + Right.Evaluate(),
                   '*' => Left.Evaluate() * Right.Evaluate(),
                   _   => 0L
               };
        public override string ToString() => $"[Operation {Left} {Operator} {Right}]";
    }
}
