using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Aoc2020_Day18
{
    internal class Solution
    {
        public string Title => "Day 18: Operation Order";

        public object? PartOne()
        {
            return InputFile.ReadAllLines().Sum(
                l => MathsExpression.Read(l).Evaluate());

        }

        public object? PartTwo()
        {
            var operatorPrecedence = new[] { '+', '*' };
            return InputFile.ReadAllLines().Sum(
                text => MathsExpression.Read(text, operatorPrecedence).Evaluate());
        }

    }

}
