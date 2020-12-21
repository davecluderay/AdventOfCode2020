using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day20
{
    internal class MapBuilder
    {
        public string ReconstructMap(IEnumerable<Tile> tiles)
        {
            var tileMap = new TileMap();

            var unplacedTiles = tiles.ToDictionary(t => t.Id);
            while (unplacedTiles.Count > 0)
            {
                foreach (var tile in unplacedTiles.Values)
                {
                    if (tileMap.TryPlace(tile))
                    {
                        unplacedTiles.Remove(tile.Id);
                        break;
                    }
                }
            }

            return tileMap.RenderMap(trimBorders: true);
        }

        private class TileMap
        {
            private readonly IDictionary<(int x, int y), Tile> _tileData = new Dictionary<(int x, int y), Tile>();

            public bool TryPlace(Tile tile)
            {
                if (!_tileData.Any())
                {
                    _tileData[(0, 0)] = tile;
                    return true;
                }

                var positions = _tileData.Keys
                                         .SelectMany(GetAdjacentFreePositions)
                                         .ToArray();
                foreach (var position in positions)
                {
                    if (TryPlaceAt(tile, position))
                        return true;
                }

                return false;
            }

            public string RenderMap(bool trimBorders)
            {
                var tileDataTrim = trimBorders ? 1 : 0;

                var tileMapBounds = (left: _tileData.Keys.Min(k => k.x),
                                     top: _tileData.Keys.Min(k => k.y),
                                     right: _tileData.Keys.Max(k => k.x),
                                     bottom: _tileData.Keys.Max(k => k.y));
                var tileSize = _tileData.Values.First().Data.Length;
                var mapDimensions = (width: (tileMapBounds.right + 1 - tileMapBounds.left) * (tileSize - 2 * tileDataTrim),
                                     height: (tileMapBounds.bottom + 1 - tileMapBounds.top) * (tileSize - 2 * tileDataTrim));
                var map = Enumerable.Range(0, mapDimensions.height)
                                    .Select(_ => Enumerable.Repeat(' ', mapDimensions.width).ToArray())
                                    .ToArray();

                foreach (var tile in _tileData)
                {
                    var tilePosition = tile.Key;
                    var tileData = tile.Value.Data;
                    for (var y = tileDataTrim; y < tileSize - tileDataTrim; y++)
                    for (var x = tileDataTrim; x < tileSize - tileDataTrim; x++)
                    {
                        var mapY = (tilePosition.y - tileMapBounds.top) * (tileSize - 2 * tileDataTrim) + y - tileDataTrim;
                        var mapX = (tilePosition.x - tileMapBounds.left) * (tileSize - 2 * tileDataTrim) + x - tileDataTrim;
                        map[mapY][mapX] = tileData[y][x];
                    }
                }

                return string.Join(Environment.NewLine, map.Select(l => new string(l)));
            }

            private bool TryPlaceAt(Tile tile, (int x, int y) position)
            {
                if (_tileData.ContainsKey(position)) return false;

                var neighbours = GetAdjacentTiles(position).ToArray();
                if (!neighbours.Any()) return false;

                foreach (var orientation in tile.GetAllPossibleOrientations())
                {
                    var isValid = true;
                    foreach (var neighbour in neighbours)
                    {
                        if (!IsValidAdjoiningEdge(neighbour, (position, orientation)))
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        _tileData[position] = orientation;
                        return true;
                    }
                }

                return false;
            }

            private bool IsValidAdjoiningEdge(((int x, int y) position, Tile tile) tile1, ((int x, int y) position, Tile tile) tile2)
            {
                var delta = (x: tile2.position.x - tile1.position.x, y: tile2.position.y - tile1.position.y);
                return delta switch
                       {
                           (0, -1) => tile1.tile.Border.Top    == tile2.tile.Border.Bottom,
                           (1, 0)  => tile1.tile.Border.Right  == tile2.tile.Border.Left,
                           (0, 1)  => tile1.tile.Border.Bottom == tile2.tile.Border.Top,
                           (-1, 0) => tile1.tile.Border.Left   == tile2.tile.Border.Right,
                           _       => false
                       };
            }

            private IEnumerable<(int x, int y)> GetAdjacentFreePositions((int x, int y) position)
                => GetAdjacentPositions(position).Where(p => !_tileData.ContainsKey(p));

            private IEnumerable<((int x, int y) position, Tile tile)> GetAdjacentTiles((int x, int y) position)
                => GetAdjacentPositions(position).Where(p => _tileData.ContainsKey(p))
                                                 .Select(p => (p, _tileData[p]));

            private IEnumerable<(int x, int y)> GetAdjacentPositions((int x, int y) position)
            {
                yield return (position.x - 1, position.y);
                yield return (position.x + 1, position.y);
                yield return (position.x, position.y - 1);
                yield return (position.x, position.y + 1);
            }
        }
    }
}
