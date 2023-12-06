using static System.Char;

namespace Puzzles;

public class Day01(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        return Input
            .Split(Environment.NewLine)
            .Sum(l => int.Parse(l.ToCharArray().First(IsDigit).ToString()) * 10 +
                      int.Parse(l.ToCharArray().Last(IsDigit).ToString()))
            .ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        var digits = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };

        for (int i = 0; i < 10; i++)
            digits.Add(i.ToString(), i);

        var lines = Input
            .Split(Environment.NewLine);

        int sum = 0;
        foreach (var line in lines)
        {
            var firstIndexOfDigits = digits
                .Select(d => new { Index = line.IndexOf(d.Key, StringComparison.Ordinal), Number = d.Value })
                .Where(dwp => dwp.Index > -1)
                .OrderBy(dwp => dwp.Index)
                .First();

            var lastIndexOfDigits = digits
                .Select(d => new { Index = line.LastIndexOf(d.Key, StringComparison.Ordinal), Number = d.Value })
                .Where(dwp => dwp.Index > -1)
                .OrderBy(dwp => dwp.Index)
                .Last();

            var number = firstIndexOfDigits.Number * 10 + lastIndexOfDigits.Number;
            sum += number;
        }

        return sum.ToString();
    }
}