namespace Aoc2020_Day03
{
    internal class Solution
    {
        public string Title => "Day 3: Toboggan Trajectory";

        public object PartOne()
        {
            return Calculate((right: 3, down: 1));
        }
        
        public object PartTwo()
        {
            return Calculate((right: 1, down: 1),
                             (right: 3, down: 1),
                             (right: 5, down: 1),
                             (right: 7, down: 1),
                             (right: 1, down: 2));
        }

        private long Calculate(params (int right, int down)[] routes)
            => Calculate(routes, null);

        private long Calculate((int right, int down)[] routes, string fileName)
        {
            var lines = InputFile.ReadAllLines(fileName);
            long product = 1;
            foreach (var (right, down) in routes)
            {
                var x = 0;
                var hits = 0;
                for (var y = 0; y < lines.Length; y += down)
                {
                    if (lines[y][x % lines[y].Length] == '#')
                        hits++;
                    x += right;
                }

                product *= hits;
            }

            return product;
        }
    }
}