using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2020_Day18
{
    internal abstract class MathsExpression
    {
        public abstract long Evaluate();

        public static MathsExpression Read(string text, char[]? operatorPrecedence = null)
        {
            return Read(Tokenize(text).GetEnumerator(), operatorPrecedence ?? Array.Empty<char>());
        }

        private static MathsExpression Read(IEnumerator<string> tokens, char[] operatorPrecedence)
        {
            static bool IsOperator(string token) => token == "*" || token == "+";
            static bool IsNumber(string token) => long.TryParse(token, out _);
            static bool IsEnterGroup(string token) => token == "(";
            static bool IsLeaveGroup(string token) => token == ")";

            List<(MathsExpression left, char @operator)> terms = new List<(MathsExpression, char)>();

            while (tokens.MoveNext())
            {
                var token = tokens.Current;

                if (IsNumber(token))
                {
                    if (!terms.Any())
                    {
                        terms.Add((new Number(Convert.ToInt64(token)), '\0'));
                    }
                    else if (terms.Last().@operator == '\0')
                    {
                        throw new ArgumentException("Number was not expected at this position.", nameof(tokens));
                    }
                    else
                    {
                        terms.Add((new Number(Convert.ToInt64(token)), '\0'));
                    }
                    continue;
                }

                if (IsOperator(token))
                {
                    if (!terms.Any())
                        throw new ArgumentException("Operator was not expected at this position.", nameof(tokens));
                    else if (terms.Last().@operator == '\0')
                        terms[^1] = (terms[^1].left, token.Single());
                    else
                        throw new ArgumentException("Operator was not expected at this position.", nameof(tokens));
                    continue;
                }

                if (IsEnterGroup(token))
                {
                    if (!terms.Any())
                    {
                        terms.Add((Read(tokens, operatorPrecedence), '\0'));
                    }
                    else if (terms.Last().@operator == '\0')
                    {
                        throw new ArgumentException("Group was not expected at this position.", nameof(tokens));
                    }
                    else
                    {
                        terms.Add((Read(tokens, operatorPrecedence), '\0'));
                    }
                    continue;
                }

                if (IsLeaveGroup(token))
                    break;
            }

            if (!terms.Any()) throw new ArgumentException("Empty expression encountered.", nameof(tokens));
            if (terms.Last().@operator != '\0') throw new ArgumentException("Operator does not have a right operand.", nameof(tokens));

            void ExpandOperations(List<(MathsExpression left, char @operator)> allTerms, char? @operator = null)
            {
                for (var index = 0; index < allTerms.Count; index++)
                {
                    while (allTerms[index].@operator != '\0' && (@operator is null || allTerms[index].@operator == @operator))
                    {
                        allTerms[index] = (new Operation(allTerms[index].left,
                                                         allTerms[index + 1].left,
                                                         allTerms[index].@operator),
                                           allTerms[index + 1].@operator);
                        allTerms.RemoveAt(index + 1);
                    }
                }
            }

            // Expand the operations in precedence order, then any that are left.
            foreach (var op in operatorPrecedence)
                ExpandOperations(terms, op);

            ExpandOperations(terms);

            return terms.Single().left;
        }

        private static IEnumerable<string> Tokenize(string text)
        {
            var nestLevel = 0;
            var acceptedSymbols = new[] { '(', ')', '+', '*' };
            var number = new StringBuilder();

            foreach (var @char in text)
            {
                if (!char.IsNumber(@char) && number.Length > 0)
                {
                    yield return number.ToString();
                    number.Clear();
                }

                if (char.IsWhiteSpace(@char))
                {
                    continue;
                }

                if (!char.IsNumber(@char) && !acceptedSymbols.Contains(@char))
                    throw new ArgumentException("Text contains unexpected characters.", nameof(text));

                if (@char == '(') nestLevel++;
                if (@char == ')') nestLevel--;
                if (nestLevel < 0)
                    throw new ArgumentException("Text contains unexpected close parentheses.", nameof(text));

                if (char.IsNumber(@char))
                {
                    number.Append(@char);
                    continue;
                }

                yield return @char.ToString();
            }

            if (number.Length > 0)
                yield return number.ToString();

            if (nestLevel > 0)
                throw new ArgumentException("Text contains unclosed parentheses.", nameof(text));
        }
    }
}
