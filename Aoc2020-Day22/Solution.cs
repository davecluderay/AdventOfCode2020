using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Aoc2020_Day22
{
    internal class Solution
    {
        public string Title => "Day 22: Crab Combat";

        public object PartOne()
        {
            var decks = LoadDecks();
            return PlayGame(decks, recursive: false).winningScore;
        }

        public object PartTwo()
        {
            var decks = LoadDecks();
            return PlayGame(decks, recursive: true).winningScore;
        }

        private (int winningIndex, int winningScore) PlayGame(int[][] startingDecks, bool recursive)
        {
            var pastStateHashes = new HashSet<string>();

            var decks = startingDecks
                        .Select(d => new Queue<int>(d))
                        .ToArray();

            while (decks.Count(d => d.Any()) > 1)
            {
                // Infinite recursion results in an instant win for player 1.
                var hash = CalculateSha1Hash(decks);
                if (pastStateHashes.Contains(hash))
                {
                    return (0, CalculateScore(decks.First()));
                }
                pastStateHashes.Add(hash);

                // The players begin the round by each drawing the top card of their deck as normal.
                var draws = decks
                            .Select((d,i) => (deck: d, index: i))
                            .Where(x => x.deck.Any())
                            .Select(x => (deck: decks[x.index], drawn: x.deck.Dequeue()))
                            .ToArray();

                if (recursive && draws.All(d => d.deck.Count >= d.drawn))
                {
                    // Each player has enough cards to recurse, so the winner is decided with a sub-game.
                    var (winningIndex, _) = PlayGame(draws.Select(d => d.deck.Take(d.drawn).ToArray()).ToArray(), recursive);
                    ApplyWin(draws, winningIndex);
                }
                else
                {
                    // The winner is the player with the higher-value card.
                    ApplyWin(draws, draws.Select((draw, index) => (draw, index))
                                         .OrderByDescending(x => x.draw.drawn)
                                         .First()
                                         .index);

                }
            }

            var winningDeck = decks.Select((deck, index) => (deck, index))
                                   .Single(d => d.deck.Any());
            return (winningDeck.index, CalculateScore(winningDeck.deck));
        }

        private static void ApplyWin((Queue<int> deck, int drawn)[] draws, int winningIndex)
        {
            var winnerDeck = draws[winningIndex].deck;
            winnerDeck.Enqueue(draws[winningIndex].drawn);
            foreach (var draw in draws.Where((draw, index) => index != winningIndex))
                winnerDeck.Enqueue(draw.drawn);
        }

        private static int CalculateScore(IEnumerable<int> deck)
            => deck.Reverse()
                   .Select((card, index) => card * (index + 1))
                   .Sum();

        private static int[][] LoadDecks(string? fileName = null)
        {
            return InputFile.ReadAllLines(fileName)
                            .InSections()
                            .Select(section => section.Skip(1)
                                                      .Select(s => Convert.ToInt32(s))
                                                      .ToArray())
                            .ToArray();
        }

        private static string CalculateSha1Hash(Queue<int>[] decks)
        {
            using var stream = new MemoryStream();
            foreach (var deck in decks.Select((deck, index) => (deck, index)))
            {
                stream.Write(BitConverter.GetBytes(deck.index));
                foreach (var card in deck.deck)
                    stream.Write(BitConverter.GetBytes(card));
            }
            stream.Flush();
            return SHA1.Create()
                       .ComputeHash(stream.GetBuffer())
                       .Aggregate(new StringBuilder(),
                                  (a, v) => a.Append(v.ToString("x2")),
                                  a => a.ToString());
        }
    }
}
