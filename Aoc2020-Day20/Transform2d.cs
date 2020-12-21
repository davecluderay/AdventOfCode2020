using System.Collections.Generic;
using System.Linq;

namespace Aoc2020_Day20
{
    internal static class Transform2d
    {
        public static IEnumerable<string[]> GetAllPossibleOrientations(string[] data)
        {
            var orientation = data;
            yield return orientation = RotateRight(orientation);
            yield return orientation = RotateRight(orientation);
            yield return orientation = RotateRight(orientation);
            yield return RotateRight(orientation);

            orientation = FlipHorizontal(data);
            yield return orientation = RotateRight(orientation);
            yield return orientation = RotateRight(orientation);
            yield return orientation = RotateRight(orientation);
            yield return RotateRight(orientation);

            orientation = FlipVertical(data);
            yield return orientation = RotateRight(orientation);
            yield return orientation = RotateRight(orientation);
            yield return orientation = RotateRight(orientation);
            yield return RotateRight(orientation);
        }

        private static string[] FlipHorizontal(string[] data)
            => data.Select(line => new string(Enumerable.Reverse(line).ToArray())).ToArray();

        private static string[] FlipVertical(string[] data)
            => data.Reverse().ToArray();

        private static string[] RotateRight(string[] data)
            => Enumerable.Range(0, data.Length)
                         .Select(i => new string(Enumerable.Range(0, data.Length)
                                                           .Reverse()
                                                           .Select(j => data[j][i])
                                                           .ToArray()))
                         .ToArray();
    }
}
