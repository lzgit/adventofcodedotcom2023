namespace Puzzles;

public class Day09(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        return Input.Split(Environment.NewLine)
            .Select(l => l.Split(' ').Select(n => int.Parse(n.Trim())).ToArray())
            .ToList()
            .Sum(GetSubsequentExtrapolation).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        return Input.Split(Environment.NewLine)
            .Select(l => l.Split(' ').Select(n => int.Parse(n.Trim())).ToArray())
            .ToList()
            .Sum(GetPrecedingExtrapolation).ToString();
    }

    private int GetSubsequentExtrapolation(int[] history)
    {
        var differences = GetDifferences(history);

        int extrapolatedValue = 0;
        for (int i = differences.Count - 1; i > 0; i--)
            extrapolatedValue += differences[i - 1].Last();

        return extrapolatedValue;
    }

    private int GetPrecedingExtrapolation(int[] history)
    {
        var differences = GetDifferences(history);

        int extrapolatedValue = 0;
        for (int i = differences.Count - 1; i > 0; i--)
            extrapolatedValue = differences[i - 1].First() - extrapolatedValue;

        return extrapolatedValue;
    }

    private static List<int[]> GetDifferences(int[] history)
    {
        var differences = new List<int[]>();

        int[] current = history;
        differences.Add(current);
        do
        {
            current = new int[current.Length - 1];
            var previous = differences.Last();

            for (int i = 0; i < current.Length; i++)
                current[i] = previous[i + 1] - previous[i];

            differences.Add(current);
        } while (current.Any(d => d != 0));

        return differences;
    }
}