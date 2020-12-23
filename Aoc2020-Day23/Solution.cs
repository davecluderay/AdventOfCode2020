using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2020_Day23
{
    internal class Solution
    {
        public string Title => "Day 23: Crab Cups";

        public object PartOne()
        {
            var (currentCup, _, lookup) = ReadCups();
            for (var n = 0; n < 100; n++)
            {
                var pickedUp = PickUpNextThreeCups(currentCup);
                var destination = FindNextDestination(currentCup, pickedUp, lookup);

                pickedUp.Next.Next.Next = destination.Next;
                destination.Next = pickedUp;

                currentCup = currentCup.Next;
            }

            currentCup = lookup[1];
            var result = new StringBuilder(lookup.Count - 1);
            for (var i = 0; i < lookup.Count - 1; i++)
            {
                currentCup = currentCup.Next;
                result.Append(currentCup.Label);
            }
            return result.ToString();
        }

        public object PartTwo()
        {
            var (currentCup, lookup) = ReadAndGenerateCups();
            for (var n = 0; n < 10_000_000; n++)
            {
                var pickedUp = PickUpNextThreeCups(currentCup);
                var destination = FindNextDestination(currentCup, pickedUp, lookup);

                pickedUp.Next.Next.Next = destination.Next;
                destination.Next = pickedUp;

                currentCup = currentCup.Next;
            }

            currentCup = lookup[1];
            return currentCup.Next.Label * currentCup.Next.Next.Label;
        }

        private static Cup PickUpNextThreeCups(Cup cup)
        {
            var firstRemoved = cup.Next;
            var lastRemoved = firstRemoved.Next.Next;

            cup.Next = lastRemoved.Next;

            lastRemoved.Next = firstRemoved;

            return firstRemoved;
        }

        private static Cup FindNextDestination(Cup currentCup, Cup pickedUp, IDictionary<long, Cup> lookup)
        {
            long destinationLabel = currentCup.Label;
            while (true)
            {
                if (--destinationLabel < 1)
                    destinationLabel = lookup.Count;

                if (pickedUp.Label != destinationLabel &&
                    pickedUp.Next.Label != destinationLabel &&
                    pickedUp.Next.Next.Label != destinationLabel)
                    break;
            }

            return lookup[destinationLabel];
        }

        private static (Cup first, Cup last, IDictionary<long, Cup> lookup) ReadCups(string? fileName = null)
        {
            var labels = InputFile.ReadAllLines(fileName)
                                  .Single()
                                  .Select(s => (long) (s - '0'));

            var lookup = new Dictionary<long, Cup>();
            var first = (Cup?) null;
            var last = first;
            foreach (var label in labels)
            {
                var cup = new Cup(label);
                cup.Next = first ??= cup;
                if (last != null)
                    last.Next = cup;
                last = cup;

                lookup[label] = cup;
            }

            if (first is null || last is null) throw new Exception("No cups found.");

            return (first, last, lookup);
        }

        private static (Cup first, IDictionary<long, Cup> lookup) ReadAndGenerateCups(string? fileName = null)
        {
            var cups = ReadCups(fileName);
            var first = cups.first;
            var last = cups.last;
            foreach (var label in Enumerable.Range(cups.lookup.Count + 1, 1_000_000 - cups.lookup.Count))
            {
                last.Next = new Cup(label) { Next = first };
                last = last.Next;

                cups.lookup[label] = last;
            }

            return (first, cups.lookup);
        }
    }
}
