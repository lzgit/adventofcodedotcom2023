namespace Puzzles;

public class Day13(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        return GetMirrors().Sum(m => GetMirrorScore(m)).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        return GetMirrors().Sum(m => GetMirrorScore(m, 1)).ToString();
    }

    private long GetMirrorScore(List<string> mirrorRows, int smudgeCount = 0)
    {
        var maxHorizontalMirrorScore = Enumerable.Range(1, mirrorRows.Count)
            .Select(i => i * 2)
            .Where(mirrorSize => mirrorSize < mirrorRows.Count)
            .Select(mirrorSize => GetHorizontalMirrorScore(mirrorRows, mirrorSize, smudgeCount))
            .Max();

        if (maxHorizontalMirrorScore > 0)
            return maxHorizontalMirrorScore;

        //Rotate
        var mirrorColumns = new List<string>();
        for (int col = 0; col < mirrorRows[0].Length; col++)
        {
            var columnString = string.Empty;
            for (var row = 0; row < mirrorRows.Count; row++)
                columnString += mirrorRows[row][col];

            mirrorColumns.Add(columnString);
        }

        var maxVerticalMirrorScore = Enumerable.Range(1, mirrorColumns.Count)
            .Select(i => i * 2)
            .Where(mirrorSize => mirrorSize < mirrorColumns.Count)
            .Select(mirrorSize => GetVerticalMirrorScore(mirrorColumns, mirrorSize, smudgeCount))
            .Max();

        if (maxVerticalMirrorScore > 0)
            return maxVerticalMirrorScore;

        return 0;
    }

    private static long GetHorizontalMirrorScore(List<string> mirrorRows, int mirrorSize, int smudgeCount)
    {
        var inverse = mirrorRows.ToList();
        inverse.Reverse();
        if (IsSymmetricHorizontally(inverse.Take(mirrorSize).ToList(), smudgeCount))
            return (mirrorRows.Count - mirrorSize / 2) * 100;

        if (IsSymmetricHorizontally(mirrorRows.Take(mirrorSize).ToList(), smudgeCount))
            return mirrorSize / 2 * 100;

        return -1;
    }

    private static long GetVerticalMirrorScore(List<string> mirrorColumns, int mirrorSize, int smudgeCount)
    {
        var inverse = mirrorColumns.ToList();
        inverse.Reverse();
        if (IsSymmetricVertically(inverse.Take(mirrorSize).ToList(), smudgeCount))
            return mirrorColumns.Count - mirrorSize / 2;

        if (IsSymmetricVertically(mirrorColumns.Take(mirrorSize).ToList(), smudgeCount))
            return mirrorSize / 2;

        return -1;
    }

    private static bool IsSymmetricHorizontally(List<string> mirrorPartRows, int smudgeCount)
    {
        var currentSmudgeCount = 0;
        for (int col = 0; col < mirrorPartRows[0].Length; col++)
            for (int row = 0; row < mirrorPartRows.Count / 2; row++)
                if (mirrorPartRows[row][col] != mirrorPartRows[mirrorPartRows.Count - row - 1][col])
                {
                    currentSmudgeCount++;
                    if (currentSmudgeCount > smudgeCount)
                        return false;
                }

        return currentSmudgeCount == smudgeCount;
    }

    private static bool IsSymmetricVertically(List<string> mirrorPartColumns, int smudgeCount)
    {
        var currentSmudgeCount = 0;
        for (int row = 0; row < mirrorPartColumns[0].Length; row++)
            for (int col = 0; col < mirrorPartColumns.Count / 2; col++)
                if (mirrorPartColumns[col][row] != mirrorPartColumns[mirrorPartColumns.Count - col - 1][row])
                {
                    currentSmudgeCount++;
                    if(currentSmudgeCount > smudgeCount)
                        return false;
                }

        return currentSmudgeCount == smudgeCount;
    }

    public List<List<string>> GetMirrors()
    {
        var mirrors = new List<List<string>> { new() };
        var lines = Input.Split(Environment.NewLine);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
                mirrors.Add(new List<string>());
            else
                mirrors.Last().Add(line);
        }

        return mirrors;
    }
}