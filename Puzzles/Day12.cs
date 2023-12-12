namespace Puzzles;

public class Day12(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var springMap = GetSpringMap();
        
        return springMap.Rows.Select(mapRow => GetArrangementCount(mapRow.MapRow, mapRow.DamagedPatterns, -1, -1)).Sum().ToString();
    }

    private int GetArrangementCount(string row, List<int> damagedPatterns, int patternMatchStartIndex, int damagePatternIndex)
    {
        var isPatternMatches = IsPatternMatches(row, damagedPatterns, patternMatchStartIndex, damagePatternIndex);

        var updatedRow = row;
        if (isPatternMatches && damagePatternIndex != -1)
            updatedRow = row.Substring(0, patternMatchStartIndex)
                         + string.Join("", Enumerable.Repeat('#', damagedPatterns[damagePatternIndex]))
                         + row.Substring(patternMatchStartIndex + damagedPatterns[damagePatternIndex]);

        if (damagePatternIndex + 1 == damagedPatterns.Count)
        {
            if (isPatternMatches && !(updatedRow.Count(c => c == '#') > damagedPatterns.Sum()))
                return 1;

            return 0;
        }

        if (!isPatternMatches && damagePatternIndex != -1)
            return 0;
        
        var totalArrangementCount = 0;
        patternMatchStartIndex += damagePatternIndex == -1 ? 1 : damagedPatterns[damagePatternIndex] + 1;
        damagePatternIndex += 1;

        do
        {
            var arrangementCount = GetArrangementCount(updatedRow, damagedPatterns, patternMatchStartIndex, damagePatternIndex);
            totalArrangementCount += arrangementCount;

            patternMatchStartIndex++;
            for (int i = patternMatchStartIndex; i < updatedRow.Length; i++)
            {
                if (updatedRow[i - 1] != '#' && updatedRow[i] != '.')
                    break;

                patternMatchStartIndex++;
            }

        } while (patternMatchStartIndex + damagedPatterns[damagePatternIndex] <= updatedRow.Length);
        
        //ToDo: Optimalization can be done via adding up the damages + 1, as it does not fit !

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

    public override string GetPuzzleTwoSolution()
    {
        return "";
    }

    private SpringMap GetSpringMap()
    {
        SpringMap map = new SpringMap();
        map.Rows = Input
            .Split(Environment.NewLine)
            .Select(l => new SpringMapRow
            {
                MapRow = l.Split(' ').First().Trim(),
                DamagedPatterns = l.Split(' ').Last().Trim().Split(',').Select(n => int.Parse(n.Trim())).ToList()
            })
            .ToList();

        return map;
    }

    public class SpringMap
    {
        public List<SpringMapRow> Rows { get; set; }
    }

    public class SpringMapRow
    {
        public string MapRow { get; set; }
        public List<int> DamagedPatterns { get; set; } = new();
    }
}