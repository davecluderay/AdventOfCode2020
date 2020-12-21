using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day20
{
    internal class MapInspector
    {
        private IEnumerable<(int x, int y)> _seaMonsterShape;

        public MapInspector()
        {
            _seaMonsterShape = ReadShapeMask("                  # ",
                                             "#    ##    ##    ###",
                                             " #  #  #  #  #  #   ");
        }
        public string RevealSeaMonsters(string map)
        {
            foreach (var orientation in GetPossibleMapOrientations(map))
            {
                bool revealedAnySeaMonsters = false;

                for (var y = 0; y < orientation.Length; y++)
                for (var x = 0; x < orientation.First().Length; x++)
                {
                    if (RevealSeaMonsterAt(orientation, (x, y)))
                    {
                        revealedAnySeaMonsters = true;
                    }
                }

                if (revealedAnySeaMonsters)
                    return string.Join(Environment.NewLine, orientation.Select(o => new string(o)));
            }

            return map;
        }

        private bool RevealSeaMonsterAt(char[][] data, (int x, int y) position)
        {
            foreach (var shapePosition in _seaMonsterShape)
            {
                var y = position.y + shapePosition.y;
                var x = position.x + shapePosition.x;
                if (y >= data.Length || x >= data[y].Length) return false;
                if (data[y][x] != '#') return false;
            }

            foreach (var shapePosition in _seaMonsterShape)
                data[position.y + shapePosition.y][position.x + shapePosition.x] = 'O';
            return true;
        }

        private IEnumerable<(int x, int y)> ReadShapeMask(params string[] shape)
        {
            for (var y = 0; y < shape.Length; y++)
            for (var x = 0; x < shape.First().Length; x++)
            {
                if (!char.IsWhiteSpace(shape[y][x]))
                    yield return (x, y);
            }
        }

        private IEnumerable<char[][]> GetPossibleMapOrientations(string map)
        {
            return Transform2d.GetAllPossibleOrientations(map.Split(Environment.NewLine))
                              .Select(orientation => orientation.Select(s => s.ToCharArray())
                                                                .ToArray());
        }
    }
}
