using System.Numerics;

namespace Aoc2020_Day13
{
    internal static class Calculate
    {
        public static long Modulo(long a, long m)
        {
            var result = a % m;
            return result < 0 ? result + m : result;
        }

        public static long LowestCommonMultiple(long a, long b)
        {
            var (num1, num2) = (a > b) ? (a, b) : (b, a);

            for (var i = 1L; i < num2; i++)
            {
                var mult = num1 * i;
                if (mult % num2 == 0)
                {
                    return mult;
                }
            }
            return num1 * num2;
        }
    }
}
