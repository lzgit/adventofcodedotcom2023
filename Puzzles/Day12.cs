namespace Puzzles;

public class Day12(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        return GetSpringMap().Rows
            .Select((mapRow, index) => GetArrangementCount(mapRow.MapRow, index, mapRow.DamagedPatterns, -1, -1, 0))
            .Sum()
            .ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        return GetSpringMap(5).Rows
            .Select((mapRow, index) => GetArrangementCount(mapRow.MapRow, index, mapRow.DamagedPatterns, -1, -1, 0))
            .Sum()
            .ToString();
    }

    private readonly Dictionary<string, long> cache = new();

    private long GetArrangementCount(string row, int rowIndex, List<int> damagedPatterns, int patternMatchStartIndex, int damagePatternIndex, int sumOfPreviousDamageCount)
    {
        var isPatternMatches = IsPatternMatches(row, damagedPatterns, patternMatchStartIndex, damagePatternIndex);

        if (isPatternMatches && damagePatternIndex != -1)
            sumOfPreviousDamageCount += damagedPatterns[damagePatternIndex];

        if (damagePatternIndex + 1 == damagedPatterns.Count)
        {
            if (isPatternMatches && !(sumOfPreviousDamageCount + row.Substring(patternMatchStartIndex + damagedPatterns[damagePatternIndex]).Count(c => c == '#') > damagedPatterns.Sum()))
                return 1;

            return 0;
        }

        if (!isPatternMatches && damagePatternIndex != -1)
            return 0;

        long totalArrangementCount = 0;
        patternMatchStartIndex += damagePatternIndex == -1 ? 1 : damagedPatterns[damagePatternIndex] + 1;
        damagePatternIndex += 1;

        var sumOfTheRestOfTheDamagePatterns = damagedPatterns.Where((_, i) => i >= damagePatternIndex + 1).Sum(dp => dp + 1) - 1;
        do
        {
            var key = $"{rowIndex}-{row}-{damagedPatterns}-{patternMatchStartIndex}-{damagePatternIndex}-{sumOfPreviousDamageCount}";
            if(!cache.TryGetValue(key, out long arrangementCount))
                arrangementCount = cache[key] = GetArrangementCount(row, rowIndex, damagedPatterns, patternMatchStartIndex, damagePatternIndex, sumOfPreviousDamageCount);

            totalArrangementCount += arrangementCount;

            patternMatchStartIndex++;

            for (int i = patternMatchStartIndex; i < row.Length; i++)
            {
                if (row[patternMatchStartIndex - 1] == '#')
                    sumOfPreviousDamageCount++;

                if (row[i - 1] != '#' && row[i] != '.')
                    break;

                patternMatchStartIndex++;
            }

        } while (patternMatchStartIndex + sumOfTheRestOfTheDamagePatterns <= row.Length);

        return totalArrangementCount;
    }

    private bool IsPatternMatches(string row, List<int> damagedPatterns, int patternMatchStartIndex, int damagePatternIndex)
    {
        if (patternMatchStartIndex < 0 || damagePatternIndex < 0)
            return false;

        var damagePattern = damagedPatterns[damagePatternIndex];
        var fragmentSize = damagePattern;
        if (row.Length < patternMatchStartIndex + fragmentSize)
            return false;

        if (row.Length >= patternMatchStartIndex + fragmentSize + 1)
            fragmentSize += 1;

        var mapFragment = row.Substring(patternMatchStartIndex, fragmentSize);

        if (mapFragment.Length == damagePattern + 1 && mapFragment[damagePattern] == '#')
            return false;

        for (int i = 0; i < damagePattern; i++)
            if (mapFragment[i] == '.')
                return false;

        return true;
    }

    private SpringMap GetSpringMap(int unfoldMultiplier = 1)
    {
        return new SpringMap(Input
            .Split(Environment.NewLine)
            .Select(l => new SpringMapRow
            (
                string.Join("?", Enumerable.Repeat(l.Split(' ').First().Trim(), unfoldMultiplier)),
                Enumerable.Repeat(l.Split(' ').Last().Trim().Split(',').Select(n => int.Parse(n.Trim())).ToList(), unfoldMultiplier).SelectMany(dp => dp).ToList()
            ))
            .ToList());
    }

    public class SpringMap(List<SpringMapRow> rows)
    {
        public List<SpringMapRow> Rows { get; set; } = rows;
    }

    public class SpringMapRow(string mapRow, List<int> damagedPatterns)
    {
        public string MapRow { get; set; } = mapRow;
        public List<int> DamagedPatterns { get; set; } = damagedPatterns;
    }
}