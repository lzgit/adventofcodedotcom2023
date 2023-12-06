namespace Puzzles;

public class Day05(string input) : DailyPuzzleBase(input)
{
    public override string GetPuzzleOneSolution()
    {
        var lines = Input.Split(Environment.NewLine);
        var seeds = GetSeeds(lines);
        var maps = GetMaps(lines);

        return seeds.Min(s => GetDestination(maps, "seed", s)).ToString();
    }

    public override string GetPuzzleTwoSolution()
    {
        var lines = Input.Split(Environment.NewLine);
        var seedRanges = GetSeedRanges(lines);
        var maps = GetMaps(lines);

        return IntersectRecursive(maps, "seed", seedRanges).Min(sr => sr.Start).ToString();
    }

    private List<SourceRange> SplitBy(SourceRange source, List<long> inclusiveStartBorders)
    {
        var sourceRanges = new List<SourceRange>();

        var current = new SourceRange
        {
            Start = source.Start
        };

        foreach (var border in inclusiveStartBorders.Where(b => b >= source.Start && b <= source.End).OrderBy(b => b))
        {
            current.End = border - 1;
            sourceRanges.Add(current);

            current = new SourceRange();
            current.Start = border;
        }

        current.End = source.End;
        sourceRanges.Add(current);

        return sourceRanges.Where(sr => sr.Range != 0).ToList();
    }

    private List<SourceRange> Intersect(List<SourceRange> source, List<SourceRange> target)
    {
        var minStart = source.Min(r => r.Start);
        var maxEnd = source.Max(r => r.End);

        //Exclusive End Border
        var inclusiveStartBorders = new List<long>();
        inclusiveStartBorders.AddRange(target.Where(sr => sr.Start >= minStart && sr.Start <= maxEnd).Select(sr => sr.Start));
        inclusiveStartBorders.AddRange(target.Where(sr => sr.End >= minStart && sr.End <= maxEnd).Select(sr => sr.End + 1));
        inclusiveStartBorders = inclusiveStartBorders.DistinctBy(b => b).ToList();

        return source.SelectMany(sr => SplitBy(sr, inclusiveStartBorders)).ToList();
    }

    private List<SourceRange> IntersectRecursive(List<Map> maps, string sourceCategory, List<SourceRange> sourceRanges)
    {
        var map = maps.SingleOrDefault(m => string.Equals(m.SourceCategory, sourceCategory, StringComparison.InvariantCultureIgnoreCase));
        if (map == null)
            return sourceRanges;

        var sourceRangesFromMapping = map.Mappings.Select(m => new SourceRange { Start = m.SourceRangeStart, Range = m.Range }).ToList();
        var destinationRanges = Intersect(sourceRanges, sourceRangesFromMapping)
            .Select(sr => new SourceRange { Start = map.GetDestination(sr.Start), Range = sr.Range })
            .ToList();

        return IntersectRecursive(maps, map.DestinationCategory, destinationRanges).ToList();
    }

    long GetDestination(List<Map> maps, string sourceCategory, long source)
    {
        var map = maps.SingleOrDefault(m => string.Equals(m.SourceCategory, sourceCategory, StringComparison.InvariantCultureIgnoreCase));
        return map == null ? source : GetDestination(maps, map.DestinationCategory, map.GetDestination(source));
    }

    private static List<Map> GetMaps(string[] lines)
    {
        var maps = new List<Map>();
        var map = new Map();

        for (int i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.Contains("map:"))
            {
                var mappingOfCategories = line.Split(' ').First().Trim().Split("-");
                map = new Map
                {
                    SourceCategory = mappingOfCategories.First().Trim(),
                    DestinationCategory = mappingOfCategories.Last().Trim(),
                    Mappings = []
                };
                maps.Add(map);
            }
            else if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
            {
                var mappingInfo = line.Split(' ').Select(n => n.Trim()).ToArray();
                map.Mappings.Add(new Mapping
                {
                    DestinationRangeStart = long.Parse(mappingInfo[0]),
                    SourceRangeStart = long.Parse(mappingInfo[1]),
                    Range = long.Parse(mappingInfo[2])
                });
            }
        }

        return maps;
    }

    private static List<long> GetSeeds(string[] lines)
    {
        return lines.First()
            .Split(':').Last().Trim()
            .Split(' ').Select(s => s.Trim()).Where(n => !string.IsNullOrEmpty(n))
            .Select(long.Parse)
            .ToList();
    }

    private static List<SourceRange> GetSeedRanges(string[] lines)
    {
        var seedRanges = new List<SourceRange>();
        var seedRange = new SourceRange();

        var numbers = lines.First()
            .Split(':').Last().Trim()
            .Split(' ').Select(s => s.Trim()).Where(n => !string.IsNullOrEmpty(n))
            .Select(long.Parse)
            .ToList();

        for (int i = 0; i < numbers.Count; i++)
        {
            if (i % 2 == 0)
            {
                seedRange = new SourceRange
                {
                    Start = numbers[i]
                };
            }
            else
            {
                seedRange.Range = numbers[i];
                seedRanges.Add(seedRange);
            }
        }

        return seedRanges;
    }

    private class SourceRange
    {
        public long Start { get; set; }
        public long Range { get; set; }

        public long End
        {
            get => Start + Range - 1;
            set => Range = value - Start + 1;
        }
    }

    private class Map
    {
        public string SourceCategory { get; init; }
        public string DestinationCategory { get; init; }
        public List<Mapping> Mappings { get; init; }

        public long GetDestination(long source) => Mappings.SingleOrDefault(m => m.IsMapped(source))?.GetDestination(source) ?? source;
    }

    private class Mapping
    {
        public long DestinationRangeStart { get; init; }
        public long SourceRangeStart { get; init; }

        public long Range { get; init; }

        public bool IsMapped(long source) => SourceRangeStart <= source && source <= SourceRangeStart + Range - 1;
        public long GetDestination(long source)
        {
            return DestinationRangeStart + (source - SourceRangeStart);
        }
    }
}