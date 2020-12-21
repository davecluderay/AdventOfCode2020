using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020_Day20
{
    internal class Tile
    {
        private static readonly Regex HeaderPattern = new Regex(@"^Tile (?<Id>\d+):$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public int Id { get; }
        public string[] Data { get; }
        public string[] Borders => new[] { Border.Top, Border.Right, Border.Bottom, Border.Left };
        public (string Top, string Right, string Bottom, string Left) Border { get; }

        public Tile(int id, string[] data)
            => (Id, Data, Border) = (id, data, GetBorder(data));

        public IEnumerable<Tile> GetAllPossibleOrientations()
        =>  Transform2d.GetAllPossibleOrientations(Data)
                     .Select(data => new Tile(Id, data));

        public static Tile Read(string[] text)
        {
            var headerText = text.First();

            var match = HeaderPattern.Match(headerText);
            if (!match.Success) throw new FormatException($"Unexpected header line: {headerText}");

            var id = Convert.ToInt32(match.Groups["Id"].Value);
            var data = text.Skip(1).ToArray();
            return new Tile(id, data);
        }

        private static (string top, string right, string bottom, string left) GetBorder(string[] data)
            => (data[0],
                new string(Enumerable.Range(0, data.Length)
                                     .Select(i => data[i][^1])
                                     .ToArray()),
                data[^1],
                new string(Enumerable.Range(0, data.Length)
                                     .Select(i => data[i][0])
                                     .ToArray()));

    }
}
