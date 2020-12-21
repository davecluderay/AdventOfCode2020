using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020_Day21
{
    internal class Food
    {
        private static readonly Regex FoodPattern = new Regex(@"^((?<Ingredient>([a-z]+))\s+)+\(contains\s+((?<Allergen>[a-z]+\b)[\s,]*)+\)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public HashSet<string> Ingredients { get; } = new HashSet<string>();
        public HashSet<string> Allergens { get; } = new HashSet<string>();

        public static Food Parse(string text)
        {
            var match = FoodPattern.Match(text);
            if (!match.Success) throw new FormatException($"Unexpected food format: {text}");

            var food = new Food();
            foreach (var ingredient in match.Groups["Ingredient"].Captures.Select(c => c.Value))
                food.Ingredients.Add(ingredient);
            foreach (var allergen in match.Groups["Allergen"].Captures.Select(c => c.Value))
                food.Allergens.Add(allergen);
            return food;
        }
    }
}
