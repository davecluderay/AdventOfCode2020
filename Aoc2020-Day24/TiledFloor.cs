using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day24
{
    internal class TiledFloor
    {
        private readonly IDictionary<(int x, int y), Tile> _tiles = new Dictionary<(int x, int y), Tile>();
        public TiledFloor(IEnumerable<(int x, int y)> blackTilePositions)
        {
            foreach (var position in blackTilePositions)
                _tiles[position] = Tile.Black;
        }

        public bool IsWhite((int x, int y) position)
            => Get(position) == Tile.White;

        public bool IsBlack((int x, int y) position)
            => Get(position) == Tile.Black;

        public Tile Get((int x, int y) position)
            => _tiles.ContainsKey(position)
                   ? _tiles[position]
                   : Tile.White;

        public void Set((int x, int y) position, Tile tile)
            => _tiles[position] = tile;

        public (int x, int y)[] GetAdjacentWhiteTilePositions((int x, int y) position)
            => GetAdjacentTilePositions(position)
               .Where(IsWhite)
               .ToArray();

        public int CountAdjacentWhiteTiles((int x, int y) position)
            => GetAdjacentWhiteTilePositions(position).Length;

        public (int x, int y)[] GetAdjacentBlackTilePositions((int x, int y) position)
            => GetAdjacentTilePositions(position)
               .Where(IsBlack)
               .ToArray();

        public int CountAdjacentBlackTiles((int x, int y) position)
            => GetAdjacentBlackTilePositions(position).Length;

        public (int x, int y)[] GetAdjacentTilePositions((int x, int y) position)
            => new[]
               {
                   (position.x - 1, position.y - 1),
                   (position.x + 1, position.y - 1),
                   (position.x + 2, position.y),
                   (position.x + 1, position.y + 1),
                   (position.x - 1, position.y + 1),
                   (position.x - 2, position.y)
               };

        public (int x, int y)[] GetBlackTilePositions()
            => _tiles.Where(e => e.Value == Tile.Black)
                     .Select(e => e.Key)
                     .ToArray();
    }
}