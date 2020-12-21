using System.Linq;

namespace Aoc2020_Day21
{
    internal class Solution
    {
        public string Title => "Day 21: Allergen Assessment";

        public object PartOne()
        {
            var foods = ReadFoods();
            var possibleAllergens = foods.SelectMany(f => f.Allergens.Select(a => (allergen: a,
                                                                                   ingredients: f.Ingredients)))
                                         .GroupBy(x => x.allergen,
                                                  x => x.ingredients)
                                         .ToDictionary(g => g.Key,
                                                       g => g.Aggregate(g.SelectMany(a => a),
                                                                        (a, v) => a.Intersect(v),
                                                                        a => a.Distinct().ToHashSet()));
            var safeIngredients = foods.SelectMany(f => f.Ingredients)
                                       .Where(i => !possibleAllergens.Any(pi => pi.Value.Contains(i)))
                                       .Distinct()
                                       .ToHashSet();
            return foods.SelectMany(f => f.Ingredients.Where(i => safeIngredients.Contains(i)))
                        .Count();
        }

        public object PartTwo()
        {
            var foods = ReadFoods();
            var possibleAllergens = foods.SelectMany(f => f.Allergens.Select(a => (allergen: a,
                                                                                   ingredients: f.Ingredients)))
                                         .GroupBy(x => x.allergen,
                                                  x => x.ingredients)
                                         .ToDictionary(g => g.Key,
                                                       g => g.Aggregate(g.SelectMany(a => a),
                                                                        (a, v) => a.Intersect(v),
                                                                        a => a.Distinct().ToHashSet()));

            while (true)
            {
                var knownAllergens = possibleAllergens.Where(x => x.Value.Count == 1)
                                                      .Select(x => (allergen: x.Key,
                                                                    ingredient: x.Value.Single()))
                                                      .ToArray();
                var unresolvedAllergens = possibleAllergens.Where(x => x.Value.Count > 1)
                                                                       .ToArray();
                if (!unresolvedAllergens.Any())
                {
                    return string.Join(",", knownAllergens.OrderBy(x => x.allergen)
                                                          .Select(x => x.ingredient));
                }

                foreach (var unresolvedAllergen in unresolvedAllergens)
                foreach (var knownAllergen in knownAllergens)
                {
                    unresolvedAllergen.Value.Remove(knownAllergen.ingredient);
                }
            }
        }

        private Food[] ReadFoods(string? fileName = null)
            => InputFile.ReadAllLines(fileName)
                        .Select(Food.Parse)
                        .ToArray();
    }
}
